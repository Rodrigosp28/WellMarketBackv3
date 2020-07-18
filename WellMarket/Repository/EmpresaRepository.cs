using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WellMarket.Entities;
using WellMarket.Entities.Responses;
using WellMarket.Helpers;
using WellMarket.Responses;

namespace WellMarket.Repository
{
    public interface IEmpresa
    {
        Task<ResponseBase> cambiarestadoEmpresa(Empresa empresa);
        Task<Response<Dashboard>> obtenerDashboard(int idEmpresa, string fecha);
        Task<Response<Empresa>> obtenerEmpresaPorId(int idEmpresa);
        Task<ResponseBase> actualizarLogo(Logo logo);
        Task<ResponseBase> ActualizarDatosEmpresa(int idEmpresa,Empresa empresa);
    }
    public class EmpresaRepository:IEmpresa
    {
        private readonly IConnection con;

        public EmpresaRepository(IConnection con)
        {
            this.con = con;
        }

        public async Task<ResponseBase> ActualizarDatosEmpresa(int idEmpresa, Empresa empresa)
        {
            var response = new ResponseBase();
            try
            {
                using(var connection = new SqlConnection(con.getConnection()))
                {
                    using(var command = new SqlCommand("Seguridad.spActualizarEmpresaDatos", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idEmpresa",idEmpresa);
                        command.Parameters.AddWithValue("@direccion",empresa.direccion);
                        command.Parameters.AddWithValue("@rfc",empresa.rfc);
                        command.Parameters.AddWithValue("@encargado",empresa.encargado);
                        command.Parameters.AddWithValue("@telefono",empresa.telefono);
                        command.Parameters.AddWithValue("@idRolEmpresa",empresa.idRolEmpresa);
                        command.Parameters.AddWithValue("@vision",empresa.vision);
                        command.Parameters.AddWithValue("@mision",empresa.mision);
                        connection.Open();
                        var result = await command.ExecuteNonQueryAsync();
                        if (result > 0)
                        {
                            response.success = true;
                            response.message = "Datos actualizados correctamente";
                            response.id = idEmpresa;
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

        public async Task<ResponseBase> actualizarLogo(Logo logo)
        {
            var response = new ResponseBase();
            try
            {
                using(var connection = new SqlConnection(con.getConnection()))
                {
                    using(var command = new SqlCommand("Seguridad.spActualizarLogoEmpresaTableLogo", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idLogo", logo.idLogo);
                        command.Parameters.AddWithValue("@idEmpresa", logo.idEmpresa);
                        command.Parameters.AddWithValue("@imagen", logo.imagen);
                        command.Parameters.AddWithValue("@url", logo.url);
                        connection.Open();
                        var result = await command.ExecuteNonQueryAsync();
                        if (result > 0)
                        {
                            response.success = true;
                            response.message = "Datos Actualizados";
                            response.id = logo.idLogo;
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

        public async Task<ResponseBase> cambiarestadoEmpresa(Empresa empresa)
        {
            var response = new ResponseBase();
            try
            {
                using(var connection = new SqlConnection(con.getConnection()))
                {
                    using(var command = new SqlCommand("Seguridad.spAbrirEmpresa", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@estado", empresa.abierto);
                        command.Parameters.AddWithValue("@idEmpresa", empresa.idEmpresa);
                        connection.Open();
                        var result = await command.ExecuteNonQueryAsync();
                        if (result > 0)
                        {
                            response.id = empresa.idEmpresa;
                            response.success = true;
                            response.message = "Datos Actualizados Correctamente";
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

        public async Task<Response<Dashboard>> obtenerDashboard(int idEmpresa, string fecha)
        {
            var response = new Response<Dashboard>();
            try
            {
                using(var connection = new SqlConnection(con.getConnection()))
                {
                    using(var command = new SqlCommand("Reporte.spDashboard1", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idEmpresa", idEmpresa);
                        command.Parameters.AddWithValue("@fecha", fecha);
                        connection.Open();
                        using(var reader = await command.ExecuteReaderAsync())
                        {
                            var dashboard = new Dashboard();
                            while (reader.Read())
                            {
                                dashboard.totalVendido = reader.GetDouble("totalVendido");
                                dashboard.articuloMasVendido = reader.GetString("articuloMasVendido");
                                dashboard.totalArticulos = reader.GetInt32("totalArticulos");
                                
                            }

                            response.success = true;
                            response.message = "Datos obtenidos Correctamente";
                            response.Data = dashboard;
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

        public async Task<Response<Empresa>> obtenerEmpresaPorId(int idEmpresa)
        {
            var response = new Response<Empresa>();
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using (var command = new SqlCommand("Seguridad.spObtenerEmpresaPorId", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idEmpresa", idEmpresa);
                        connection.Open();
                        using(var reader = await command.ExecuteReaderAsync())
                        {
                            var empresa = new Empresa();
                            while (reader.Read())
                            {
                                empresa.idEmpresa = reader.GetInt32("idEmpresa");
                                empresa.nombre = reader.GetString("nombre");
                                empresa.direccion = reader.GetString("direccion");
                                empresa.rfc = reader.GetString("rfc");
                                empresa.encargado = reader.GetString("encargado");
                                empresa.vision = reader.GetString("vision");
                                empresa.mision = reader.GetString("mision");
                                empresa.telefono = reader.GetString("telefono");
                                empresa.urlLogo = reader.GetString("urlLogo");
                                empresa.idRolEmpresa = reader.GetInt32("idRolEmpresa");
                                empresa.nombreRol = reader.GetString("nombreRol");
                                empresa.fecha = reader.GetString("fecha");
                                empresa.abierto = reader.GetBoolean("abierto");
                                empresa.logo = new Logo
                                {
                                    idLogo = reader.GetInt32("idLogo"),
                                    idEmpresa = reader.GetInt32("idEmpresa"),
                                    imagen = reader.GetString("imagen"),
                                    url = reader.GetString("url")
                                };

                            }
                            response.success = true;
                            response.message = "Datos Obtenidos Correctamente";
                            response.Data = empresa;
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
