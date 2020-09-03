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
    public interface IMesa
    {
        Task<ResponseBase> InsertarMesa(Mesa mesa);
        Task<Response<List<Mesa>>>ObtenerMesasPorEmpresa(int idEmpresa, int idTipo);
        Task<Response<List<TipoMesa>>> ObtenerTiposMesa();
        Task<Response<List<Mesa>>> ObtenerMesasPorEmpresaAll(int idEmpresa);
        Task<ResponseBase> EliminarMesa(int idMesa);
        Task<ResponseBase> ActualizarNombreMesa(int idMesa, string nombre);
    }
    public class MesaRepository:IMesa
    {
        private readonly IConnection con;

        public MesaRepository(IConnection con)
        {
            this.con = con;
        }

        // actualiza el nombre de la empresa
        public async Task<ResponseBase> ActualizarNombreMesa(int idMesa, string nombre)
        {
            var response = new ResponseBase();
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using (var command = new SqlCommand("Reporte.spActualizarNombreMesa", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idMesa", idMesa);
                        command.Parameters.AddWithValue("@nombre", nombre);
                        connection.Open();
                        var result = await command.ExecuteNonQueryAsync();
                        if (result > 0)
                        {
                            response.id = idMesa;
                            response.success = true;
                            response.message = "Datos Actualizados Correctamente";
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

        //elimina una mesa de forma logica en la base de datos
        public async Task<ResponseBase> EliminarMesa(int idMesa)
        {
            var response = new ResponseBase();
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using (var command = new SqlCommand("exec Reporte.spEliminarMesa", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idMesa", idMesa);
                        connection.Open();
                        var result = await command.ExecuteNonQueryAsync();
                        if (result > 0)
                        {
                            response.id = idMesa;
                            response.success = true;
                            response.message = "Datos Eliminados Correctamente";
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

        //ingresa una mesa con su tipo y que pertenezca a una empresa
        public async Task<ResponseBase> InsertarMesa(Mesa mesa)
        {
            var response = new ResponseBase();
            try
            {
                using(var connection = new SqlConnection(con.getConnection()))
                {
                    using(var command = new SqlCommand("Reporte.spInsertarMesa", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idMesa",0);
                        command.Parameters.AddWithValue("@nombre",mesa.nombre);
                        command.Parameters.AddWithValue("@descripcion",mesa.descripcion);
                        command.Parameters.AddWithValue("@idEmpresa",mesa.idEmpresa);
                        command.Parameters.AddWithValue("@idTipoMesa",mesa.idTipoMesa);
                        command.Parameters["@idMesa"].Direction = ParameterDirection.Output;
                        connection.Open();
                        var result = await command.ExecuteNonQueryAsync();
                        if (result > 0)
                        {
                            response.id = Convert.ToInt32(command.Parameters["@idMesa"].Value);
                            response.success = true;
                            response.message = "Datos Insertados Correctamente";
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

        //obtiene todas las mesas dependiendo de su tipo por el idEmpresa y su idTipo
        public async Task<Response<List<Mesa>>> ObtenerMesasPorEmpresa(int idEmpresa, int idTipo)
        {
            var response = new Response<List<Mesa>>();
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using (var command = new SqlCommand("Reporte.spObtenerMesaPorEmpresa", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@idEmpresa", idEmpresa);
                        command.Parameters.AddWithValue("@idTipo", idTipo);
                        connection.Open();
                        using(var reader1 = await command.ExecuteReaderAsync())
                        {
                            var mesas = new List<Mesa>();
                            while (reader1.Read())
                            {
                                var mesa = new Mesa();
                                mesa.idMesa = reader1.GetInt32("idMesa");
                                mesa.nombre = reader1.GetString("nombre");
                                mesa.descripcion = reader1.GetString("descripcion");
                                mesa.idEmpresa = reader1.GetInt32("idEmpresa");
                                mesa.ocupado = reader1.GetBoolean("ocupado");
                                mesa.idTipoMesa = reader1.GetInt32("idTipoMesa");
                                //obtiene los tickets que esten activos en esa mesa
                                using (var command2 = new SqlCommand("Reporte.spObtenerMesasOcupadas", connection))
                                {
                                    command2.CommandType = CommandType.StoredProcedure;
                                    command2.Parameters.AddWithValue("@idMesa", mesa.idMesa);
                                    using(var reader2 = await command2.ExecuteReaderAsync())
                                    {
                                        var mesast = new List<MesaTicket>();
                                        while (reader2.Read())
                                        {
                                            var mesat = new MesaTicket();
                                            mesat.idMesaTicket = reader2.GetInt32("idMesaTicket");
                                            mesat.idTicket = reader2.GetInt32("idTicket");
                                            mesat.idMesa = reader2.GetInt32("idMesa");
                                            mesat.ocupado = reader2.GetBoolean("ocupado");
                                            mesast.Add(mesat);
                                        }
                                        mesa.mesas = mesast;
                                    }
                                }
                                mesas.Add(mesa);
                            }
                            response.Data = mesas;
                            response.success = true;
                            response.message = "Datos Obtenidos Correctamente";
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

        // obtiene todas las mesas por el idEmpresa
        public async Task<Response<List<Mesa>>> ObtenerMesasPorEmpresaAll(int idEmpresa)
        {
            var response = new Response<List<Mesa>>();
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using (var command = new SqlCommand("Reporte.spObtenerMesaPorEmpresaAll", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@idEmpresa", idEmpresa);
                        connection.Open();
                        using (var reader1 = await command.ExecuteReaderAsync())
                        {
                            var mesas = new List<Mesa>();
                            while (reader1.Read())
                            {
                                var mesa = new Mesa();
                                mesa.idMesa = reader1.GetInt32("idMesa");
                                mesa.nombre = reader1.GetString("nombre");
                                mesa.descripcion = reader1.GetString("descripcion");
                                mesa.idEmpresa = reader1.GetInt32("idEmpresa");
                                mesa.ocupado = reader1.GetBoolean("ocupado");
                                mesa.idTipoMesa = reader1.GetInt32("idTipoMesa");
                                mesa.descripcionTipo = reader1.GetString("descripcionTipo");
                                mesas.Add(mesa);
                            }
                            response.Data = mesas;
                            response.success = true;
                            response.message = "Datos Obtenidos Correctamente";
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

        //obtiene del catalogo los tipos de mesas
        public async Task<Response<List<TipoMesa>>> ObtenerTiposMesa()
        {
            var response = new Response<List<TipoMesa>>();
            try
            {
                using(var connection = new SqlConnection(con.getConnection()))
                {
                    using(var command = new SqlCommand("Catalogos.spObtenerTipoMesa", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        connection.Open();
                        using(var reader = await command.ExecuteReaderAsync())
                        {
                            var list = new List<TipoMesa>();
                            while (reader.Read())
                            {
                                list.Add(new TipoMesa
                                {
                                    idTipoMesa = reader.GetInt32("idTipoMesa"),
                                    descripcion = reader.GetString("descripcion")
                                });
                            }
                            response.success = true;
                            response.message = "datos obtenidos correctamente";
                            response.Data = list;
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
    }
}
