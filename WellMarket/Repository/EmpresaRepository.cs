using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Metadata;
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
        Task<Response<List<Empresa>>> ObtenerEmpresaPorMunicipio(int idMunicipio,int pag);
        Task<Response<List<Empresa>>> ObtenerEmpresaPorMunicipioBusqueda(int idMunicipio,int pag,string busqueda);
        Task<Response<List<Empresa>>> ObtenerEmpresaPorZona(int idZona, int pag);
        Task<Response<List<Empresa>>> ObtenerEmpresasaAll();
        Task<ResponseBase> ActivarDesactivarEmpresa(int idEmpresa, Boolean opt);
        Task<Response<Empresa>> ObtenerUbicacionEmpresa(int idEmpresa);
        Task<ResponseBase> ActualizarUbicacionEmpresa(int idEmpresa, string lat, string lng);
    }
    public class EmpresaRepository:IEmpresa
    {
        private readonly IConnection con;
        private readonly IConfiguration _configuration;

        public EmpresaRepository(IConnection con, IConfiguration configuration)
        {
            this.con = con;
            _configuration = configuration;
        }

        public async Task<ResponseBase> ActivarDesactivarEmpresa(int idEmpresa, bool opt)
        {
            var response = new ResponseBase();
            try
            {
                using(var connection = new SqlConnection(con.getConnection()))
                {
                    using(var command = new SqlCommand("Seguridad.spActivarDesactivarEmpresa", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idEmpresa", idEmpresa);
                        command.Parameters.AddWithValue("@opt", opt);
                        connection.Open();
                        var result = await command.ExecuteNonQueryAsync();
                        if (result > 0)
                        {
                            if (opt == true)
                            {
                                response.message = "Empresa Activada Correctamente";
                               
                            }
                            else
                            {
                                response.message = "Empresa Desactivada Correctamente";
                            }
                            response.success = true;
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
                        command.Parameters.AddWithValue("@horaInicio",empresa.horaInicio);
                        command.Parameters.AddWithValue("@horaCerrado",empresa.horaCerrado);
                        command.Parameters.AddWithValue("@diaInicio",empresa.diaInicio);
                        command.Parameters.AddWithValue("@diaCerrado",empresa.diaCerrado);
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


        //abrir o cerrar empresa
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
                                empresa.horaInicio = reader.GetString("horaInicio");
                                empresa.horaCerrado = reader.GetString("horaCerrado");
                                empresa.diaInicio = reader.GetString("diaInicio");
                                empresa.diaCerrado = reader.GetString("diaCerrado");
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

        public async Task<Response<List<Empresa>>> ObtenerEmpresaPorMunicipio(int idMunicipio, int pag)
        {
            var response = new Response<List<Empresa>>();
            try
            {
                using(var connection = new SqlConnection(con.getConnection()))
                {
                    using(var command = new SqlCommand("Seguridad.spObtenerEmpresaPorMunicipio", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idZona", idMunicipio);
                        command.Parameters.AddWithValue("@paginasTotal", 0);
                        command.Parameters.AddWithValue("@pagina", pag);
                        command.Parameters["@paginasTotal"].Direction = ParameterDirection.Output;
                        connection.Open();
                        using(var reader = await command.ExecuteReaderAsync())
                        {
                            var list = new List<Empresa>();
                            while (reader.Read())
                            {
                                var empresa = new Empresa();
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
                                empresa.horaInicio = reader.GetString("horaInicio");
                                empresa.horaCerrado = reader.GetString("horaCerrado");
                                empresa.diaInicio = reader.GetString("diaInicio");
                                empresa.diaCerrado = reader.GetString("diaCerrado");
                                empresa.latitud = reader.GetString("latitud");
                                empresa.longitud = reader.GetString("longitud");
                                if (empresa.longitud.Equals("0"))
                                {
                                    empresa.tieneUbicacion = false;
                                }
                                else
                                {
                                    empresa.tieneUbicacion = true;
                                }
                                empresa.logo = new Logo
                                {
                                    idLogo = reader.GetInt32("idLogo"),
                                    idEmpresa = reader.GetInt32("idEmpresa"),
                                    imagen = reader.GetString("imagen"),
                                    url = reader.GetString("url")
                                };
                                list.Add(empresa);
                            }
                            response.success = true;
                            response.message = "Datos Obtenido Correctamente";
                            response.Data = list;
                        }
                        response.paginas = Convert.ToInt32(command.Parameters["@paginasTotal"].Value);

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

        public async Task<Response<List<Empresa>>> ObtenerEmpresaPorZona(int idZona, int pag)
        {
            var response = new Response<List<Empresa>>();
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using (var command = new SqlCommand("Seguridad.spObtenerEmpresaPorZona", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idZona", idZona);
                        command.Parameters.AddWithValue("@paginasTotal", 0);
                        command.Parameters.AddWithValue("@pagina", pag);
                        command.Parameters["@paginasTotal"].Direction = ParameterDirection.Output;
                        connection.Open();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            var list = new List<Empresa>();
                            while (reader.Read())
                            {
                                var empresa = new Empresa();
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
                                empresa.horaInicio = reader.GetString("horaInicio");
                                empresa.horaCerrado = reader.GetString("horaCerrado");
                                empresa.diaInicio = reader.GetString("diaInicio");
                                empresa.diaCerrado = reader.GetString("diaCerrado");
                                empresa.latitud = reader.GetString("latitud");
                                empresa.longitud = reader.GetString("longitud");
                                if (empresa.longitud.Equals("0"))
                                {
                                    empresa.tieneUbicacion = false;
                                }
                                else
                                {
                                    empresa.tieneUbicacion = true;
                                }
                                empresa.logo = new Logo
                                {
                                    idLogo = reader.GetInt32("idLogo"),
                                    idEmpresa = reader.GetInt32("idEmpresa"),
                                    imagen = reader.GetString("imagen"),
                                    url = reader.GetString("url")
                                };
                                list.Add(empresa);
                            }
                            response.success = true;
                            response.message = "Datos Obtenido Correctamente";
                            response.Data = list;
                        }
                            response.paginas = Convert.ToInt32(command.Parameters["@paginasTotal"].Value);
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

        public async Task<Response<List<Empresa>>> ObtenerEmpresasaAll()
        {
            var response = new Response<List<Empresa>>();
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using (var command = new SqlCommand("Seguridad.spObtenerEmpresas", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        connection.Open();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            var list = new List<Empresa>();
                            while (reader.Read())
                            {
                                var empresa = new Empresa();
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
                                empresa.horaInicio = reader.GetString("horaInicio");
                                empresa.horaCerrado = reader.GetString("horaCerrado");
                                empresa.diaInicio = reader.GetString("diaInicio");
                                empresa.diaCerrado = reader.GetString("diaCerrado");
                                empresa.activo = reader.GetBoolean("activo");
                                empresa.logo = new Logo
                                {
                                    idLogo = reader.GetInt32("idLogo"),
                                    idEmpresa = reader.GetInt32("idEmpresa"),
                                    imagen = reader.GetString("imagen"),
                                    url = reader.GetString("url")
                                };
                                list.Add(empresa);
                            }
                            response.success = true;
                            response.message = "Datos Obtenido Correctamente";
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

        public async Task<Response<Empresa>> ObtenerUbicacionEmpresa(int idEmpresa)
        {
            var response = new Response<Empresa>();
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using (var command = new SqlCommand("Seguridad.spMapaEmpresa", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idEmpresa", idEmpresa);
                        command.Parameters.AddWithValue("@opt", 1);
                        connection.Open();
                        using(var reader = await command.ExecuteReaderAsync())
                        {
                            var empresa = new Empresa();
                            while (reader.Read())
                            {
                                empresa.idEmpresa = idEmpresa;
                                empresa.latitud = reader.GetString("latitud");
                                empresa.longitud = reader.GetString("longitud");
                            }
                            response.success = true;
                            response.message = "Datos obtenidos correctamente";
                            response.id = idEmpresa;
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

        public async Task<ResponseBase> ActualizarUbicacionEmpresa(int idEmpresa, string lat, string lng)
        {
            var response = new ResponseBase();
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using (var command = new SqlCommand("Seguridad.spMapaEmpresa", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idEmpresa", idEmpresa);
                        command.Parameters.AddWithValue("@latitud", lat);
                        command.Parameters.AddWithValue("@longitud", lng);
                        command.Parameters.AddWithValue("@opt", 2);
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
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return response;
        }

        public async Task<Response<List<Empresa>>> ObtenerEmpresaPorMunicipioBusqueda(int idMunicipio, int pag, string busqueda)
        {
            var response = new Response<List<Empresa>>();
            var linkback = _configuration.GetValue<string>("backlink");
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using (var command = new SqlCommand("Seguridad.spObtenerEmpresaPorMunicipioBusqueda", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idZona", idMunicipio);
                        command.Parameters.AddWithValue("@paginasTotal", 0);
                        command.Parameters.AddWithValue("@pagina", pag);
                        command.Parameters.AddWithValue("@busqueda", busqueda);
                        command.Parameters["@paginasTotal"].Direction = ParameterDirection.Output;
                        connection.Open();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            var list = new List<Empresa>();
                            while (reader.Read())
                            {
                                var empresa = new Empresa();
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
                                empresa.horaInicio = reader.GetString("horaInicio");
                                empresa.horaCerrado = reader.GetString("horaCerrado");
                                empresa.diaInicio = reader.GetString("diaInicio");
                                empresa.diaCerrado = reader.GetString("diaCerrado");
                                empresa.latitud = reader.GetString("latitud");
                                empresa.longitud = reader.GetString("longitud");
                                if (empresa.longitud.Equals("0"))
                                {
                                    empresa.tieneUbicacion = false;
                                }
                                else
                                {
                                    empresa.tieneUbicacion = true;
                                }
                                empresa.logo = new Logo
                                {
                                    idLogo = reader.GetInt32("idLogo"),
                                    idEmpresa = reader.GetInt32("idEmpresa"),
                                    imagen = reader.GetString("imagen"),
                                };
                                empresa.logo.url = $"http://{linkback}/api/empresa/logo/" + empresa.idEmpresa + "/" + empresa.logo.imagen;
                                list.Add(empresa);
                            }
                            response.success = true;
                            response.message = "Datos Obtenido Correctamente";
                            response.Data = list;
                        }
                        response.paginas = Convert.ToInt32(command.Parameters["@paginasTotal"].Value);

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
