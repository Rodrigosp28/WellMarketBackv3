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
    public interface ICocina
    {
        Task<ResponseBase> CocinaEntrada(Cocina c);
        Task<ResponseBase> CocinaSalida(Cocina c);
        Task<Response<List<Cocina>>> ObtenerProductoCocina(int idEmpresa, string fecha);
        Task<Response<List<Cocina>>> ObtenerProductoCocinaDelDia(int idEmpresa, string fecha);
        Task<ResponseBase> ObtenerCantidadProductosCocina(int idEmpresa, string fecha);
    }
    public class CocinaRepository:ICocina
    {
        private readonly IConnection con;
        public CocinaRepository(IConnection con)
        {
            this.con = con;
        }

        public async Task<ResponseBase> CocinaEntrada(Cocina c)
        {
            var response = new ResponseBase();
            try
            {
                using(var connection = new SqlConnection(con.getConnection()))
                {
                    using(var command = new SqlCommand("Reporte.spCocinaEntrada", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idCocina",0);
                        command.Parameters.AddWithValue("@idVenta",c.idVenta);
                        command.Parameters.AddWithValue("@fecha",c.fecha);
                        command.Parameters.AddWithValue("@horaEntrada",c.horaEntrada);
                        command.Parameters.AddWithValue("@nota",c.nota);
                        command.Parameters["@idCocina"].Direction = ParameterDirection.Output;
                        connection.Open();
                        var result = await command.ExecuteNonQueryAsync();
                        if (result > 1)
                        {
                            response.success = true;
                            response.id = Convert.ToInt32(command.Parameters["@idCocina"].Value);
                            response.message = "Datos obtenidos correctamente";
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

        public async Task<ResponseBase> CocinaSalida(Cocina c)
        {
            var response = new ResponseBase();
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using (var command = new SqlCommand("Reporte.spCocinaSalida", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idCocina",c.idCocina);
                        command.Parameters.AddWithValue("@horaSalida",c.horaSalida);
                        connection.Open();
                        var result = await command.ExecuteNonQueryAsync();
                        if (result > 0)
                        {
                            response.success = true;
                            response.id = c.idCocina;
                            response.message = "Datos obtenidos correctamente";
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

        public async Task<ResponseBase> ObtenerCantidadProductosCocina(int idEmpresa, string fecha)
        {
            var response = new ResponseBase();
            try
            {
                using(var connection= new SqlConnection(con.getConnection()))
                {
                    using (var command = new SqlCommand("Reporte.spCocinaCantidad", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idEmpresa", idEmpresa);
                        command.Parameters.AddWithValue("@fecha", fecha);
                        connection.Open();
                        using(var reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                response.id = reader.GetInt32("cantidad");
                            }
                            response.success = true;
                            response.message = "Datos obtenidos correctamente";
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

        public async Task<Response<List<Cocina>>> ObtenerProductoCocina(int idEmpresa, string fecha)
        {
            var response = new Response<List<Cocina>>();
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using (var command = new SqlCommand("Reporte.spCocinaObtenerPorEmpresa", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idEmpresa",idEmpresa);
                        command.Parameters.AddWithValue("@fecha",fecha);
                        connection.Open();
                        using(var reader = await command.ExecuteReaderAsync())
                        {
                            var list = new List<Cocina>();
                            while (reader.Read())
                            {
                                var cocina = new Cocina();
                                cocina.idCocina = reader.GetInt32("idCocina");
                                cocina.fecha = reader.GetString("fecha");
                                cocina.horaEntrada = reader.GetString("horaEntrada");
                                cocina.nota = reader.GetString("nota");
                                cocina.producto = reader.GetString("nombre");
                                cocina.cantidad = reader.GetInt32("cantidad");
                                cocina.idTicket = reader.GetInt32("idTicket");
                                list.Add(cocina);
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

        public async Task<Response<List<Cocina>>> ObtenerProductoCocinaDelDia(int idEmpresa, string fecha)
        {
            var response = new Response<List<Cocina>>();
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using (var command = new SqlCommand("Reporte.spCocinaPorEmpresaDelDia", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idEmpresa", idEmpresa);
                        command.Parameters.AddWithValue("@fecha", fecha);
                        connection.Open();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            var list = new List<Cocina>();
                            while (reader.Read())
                            {
                                var cocina = new Cocina();
                                cocina.idCocina = reader.GetInt32("idCocina");
                                cocina.fecha = reader.GetString("fecha");
                                cocina.horaEntrada = reader.GetString("horaEntrada");
                                cocina.nota = reader.GetString("nota");
                                cocina.producto = reader.GetString("nombre");
                                cocina.cantidad = reader.GetInt32("cantidad");
                                cocina.idTicket = reader.GetInt32("idTicket");
                                list.Add(cocina);
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
