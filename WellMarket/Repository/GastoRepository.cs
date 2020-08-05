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
    public interface IGasto
    {
        Task<ResponseBase> InsertarGasto(Gasto gasto);
        Task<Response<List<Gasto>>> ObtenerGastosPorIdEmpresa(int idEmpresa, string fecha);
        Task<ResponseBase> EliminarGasto(int idGasto);
    }
    public class GastoRepository:IGasto
    {
        private readonly IConnection con;

        public GastoRepository(IConnection con)
        {
            this.con = con;
        }

        public async Task<ResponseBase> EliminarGasto(int idGasto)
        {
            var response = new ResponseBase();
            try
            {
                using(var connection =new SqlConnection(con.getConnection()))
                {
                    using(var command = new SqlCommand("Reporte.spEliminarGasto", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idGasto", idGasto);
                        connection.Open();
                        var result = await command.ExecuteNonQueryAsync();
                        if (result > 0)
                        {
                            response.success = true;
                            response.message = "Datos eliminados correctamente";
                            response.id = idGasto;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseBase> InsertarGasto(Gasto gasto)
        {
            var response = new ResponseBase();
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using (var command = new SqlCommand("Reporte.spInsertarGastos", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idGasto", 0);
                        command.Parameters.AddWithValue("@idEmpresa", gasto.idEmpresa);
                        command.Parameters.AddWithValue("@concepto", gasto.concepto);
                        command.Parameters.AddWithValue("@descripcion", gasto.descripcion);
                        command.Parameters.AddWithValue("@cantidad", gasto.cantidad);
                        command.Parameters.AddWithValue("@fecha", gasto.fecha);
                        command.Parameters.AddWithValue("@hora", gasto.hora);
                        command.Parameters["@idGasto"].Direction = ParameterDirection.Output;
                        connection.Open();
                        var result = await command.ExecuteNonQueryAsync();
                        if (result > 0)
                        {
                            response.success = true;
                            response.message = "Datos insertados correctamente";
                            response.id = Convert.ToInt32(command.Parameters["@idGasto"].Value);
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

        public async Task<Response<List<Gasto>>> ObtenerGastosPorIdEmpresa(int idEmpresa, string fecha)
        {
            var response = new Response<List<Gasto>>();
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using (var command = new SqlCommand("Reporte.spObtenerGastosPorIdEmpresa", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idEmpresa", idEmpresa);
                        command.Parameters.AddWithValue("@fecha", fecha);
                        connection.Open();
                        using(var reader = await command.ExecuteReaderAsync())
                        {
                            var list = new List<Gasto>();
                            while (reader.Read())
                            {
                                var gasto = new Gasto();
                                gasto.idGasto = reader.GetInt32("idGasto");
                                gasto.concepto = reader.GetString("concepto");
                                gasto.descripcion = reader.GetString("descripcion");
                                gasto.cantidad = reader.GetDouble("cantidad");
                                gasto.fecha = reader.GetString("fecha");
                                gasto.hora = reader.GetString("hora");
                                gasto.idEmpresa = reader.GetInt32("idEmpresa");
                                list.Add(gasto);
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
