using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WellMarket.Entities;
using WellMarket.Helpers;
using WellMarket.Responses;

namespace WellMarket.Repository
{
    public interface IIngreso
    {
        Task<ResponseBase> InsertarIngreso(Ingreso ingreso);
        Task<Response<List<Ingreso>>> ObtenerIngresosPorIdEmpresa(int idEmpresa, string fecha);
        Task<ResponseBase> EliminarIngreso(int idIngreso);
    }
    public class IngresoRepository: IIngreso
    {
        private readonly IConnection con;
        public IngresoRepository(IConnection con)
        {
            this.con = con;
        }

        public async Task<ResponseBase> EliminarIngreso(int idIngreso)
        {
            var response = new ResponseBase();
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using (var command = new SqlCommand("Reporte.spEliminarIngreso", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idIngreso", idIngreso);
                        connection.Open();
                        var result = await command.ExecuteNonQueryAsync();
                        if (result > 0)
                        {
                            response.success = true;
                            response.message = "Datos eliminados correctamente";
                            response.id = idIngreso;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseBase> InsertarIngreso(Ingreso ingreso)
        {
            var response = new ResponseBase();
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using (var command = new SqlCommand("Reporte.spInsertarIngreso", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idIngreso", 0);
                        command.Parameters.AddWithValue("@idEmpresa", ingreso.idEmpresa);
                        command.Parameters.AddWithValue("@concepto", ingreso.concepto);
                        command.Parameters.AddWithValue("@descripcion", ingreso.descripcion);
                        command.Parameters.AddWithValue("@cantidad", ingreso.cantidad);
                        command.Parameters.AddWithValue("@fecha", ingreso.fecha);
                        command.Parameters.AddWithValue("@hora", ingreso.hora);
                        command.Parameters["@idIngreso"].Direction = ParameterDirection.Output;
                        connection.Open();
                        var result = await command.ExecuteNonQueryAsync();
                        if (result > 0)
                        {
                            response.success = true;
                            response.message = "Datos insertados correctamente";
                            response.id = Convert.ToInt32(command.Parameters["@idIngreso"].Value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return response;
        }

        public async Task<Response<List<Ingreso>>> ObtenerIngresosPorIdEmpresa(int idEmpresa, string fecha)
        {
            var response = new Response<List<Ingreso>>();
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using (var command = new SqlCommand("Reporte.spObtenerIngresosPoIdEmpresa", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idEmpresa", idEmpresa);
                        command.Parameters.AddWithValue("@fecha", fecha);
                        connection.Open();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            var list = new List<Ingreso>();
                            while (reader.Read())
                            {
                                var ingreso = new Ingreso();
                                ingreso.idIngreso = reader.GetInt32("idIngreso");
                                ingreso.concepto = reader.GetString("concepto");
                                ingreso.descripcion = reader.GetString("descripcion");
                                ingreso.cantidad = reader.GetDouble("cantidad");
                                ingreso.fecha = reader.GetString("fecha");
                                ingreso.hora = reader.GetString("hora");
                                ingreso.idEmpresa = reader.GetInt32("idEmpresa");
                                list.Add(ingreso);
                            }
                            response.success = true;
                            response.message = "Datos obtenidos correctamente";
                            response.Data = list;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return response;
        }
    }
}
