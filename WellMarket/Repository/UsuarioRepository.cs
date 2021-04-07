﻿using Microsoft.Extensions.Configuration;
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
    public interface IUsuarioRepository
    {
        Task<ResponseBase> InsertarUsuario(Usuario user);
        Task<Response<List<Usuario>>> ObtenerUsuarios();
        Task<Response<Usuario>> ObtenerUsuarioPorId(int id);
        Task<Response<Usuario>> ObtenerUsuarioPorTelefono(string tel);
        Task<ResponseBase> ActivarUsuario(int id, Boolean activo);
        Task<ResponseBase> RegistrarUsuarioEmpresa(Usuario user, Empresa empresa);
        Task<ResponseBase> actualizarLogo(int id,string logo, string img);
        Task<ResponseBase> RegistrarUsuario(Usuario user,string codVerificacion);
        Task<ResponseBase> VerificacionUsuario(int idUsuario);
        Task<Response<List<HistorialUsuario>>> ObtenerHistorialCompraUsuario(int idUsuario);
    }
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly IConnection con;
        private readonly IConfiguration _configuration;

        public UsuarioRepository(IConnection con, IConfiguration configuration)
        {
            this.con = con;
            _configuration = configuration;
        }
        public async Task<ResponseBase> InsertarUsuario(Usuario user)
        {
            var response = new ResponseBase();
            try
            {
                using (var connection = new SqlConnection(this.con.getConnection()))
                {
                    using(var command = new SqlCommand("Seguridad.spInsertarUsuario", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idUsuario", user.idUsuario);
                        command.Parameters.AddWithValue("@usuario", user.usuario);
                        command.Parameters.AddWithValue("@password", user.password);
                        command.Parameters.AddWithValue("@nombre", user.nombre);
                        command.Parameters.AddWithValue("@apellido", user.apellido);
                        command.Parameters.AddWithValue("@direccion", user.direccion);
                        command.Parameters.AddWithValue("@telefono", user.telefono);
                        command.Parameters.AddWithValue("@idZona", user.idZona);
                        command.Parameters.AddWithValue("@idRol", user.idRol);
                        command.Parameters.AddWithValue("@idTipoUsuario", user.idTipoUsuario);
                        command.Parameters.AddWithValue("@correo", user.correo);
                        command.Parameters["@idUsuario"].Direction = ParameterDirection.Output;
                        connection.Open();
                        var result = await command.ExecuteNonQueryAsync();
                        if(result > 0)
                        {
                            response.id = Convert.ToInt32(command.Parameters["@idUsuario"].Value);
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

        //verificacion de usuario para que pueda iniciar sesion
        public async Task<ResponseBase> VerificacionUsuario(int idUsuario)
        {
            var response = new ResponseBase();
            try
            {
                using (var connection = new SqlConnection(this.con.getConnection()))
                {
                    using (var command = new SqlCommand("Seguridad.spVerificacionDeUsuario", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idUsuario", idUsuario);
                        connection.Open();
                        var result = await command.ExecuteNonQueryAsync();
                        if (result > 0)
                        {
                            response.id = idUsuario;
                            response.success = true;
                            response.message = "Usuario Verificado Correctamente";
                        }
                        else
                        {
                            response.success = false;
                            response.message = "Usuario no verificado";
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

        //codVerificacion se refiere al codigo para verificacion del correo
        //este metodo registra al usuario desde la pagina register user
        public async Task<ResponseBase> RegistrarUsuario(Usuario user, string codVerificacion)
        {
            var response = new ResponseBase();
            try
            {
                using (var connection = new SqlConnection(this.con.getConnection()))
                {
                    using (var command = new SqlCommand("Seguridad.spRegistrarUsuario", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idUsuario", 0);
                        command.Parameters.AddWithValue("@usuario", user.usuario);
                        command.Parameters.AddWithValue("@password", user.password);
                        command.Parameters.AddWithValue("@nombre", user.nombre);
                        command.Parameters.AddWithValue("@apellido", user.apellido);
                        command.Parameters.AddWithValue("@direccion", user.direccion);
                        command.Parameters.AddWithValue("@telefono", user.telefono);
                        command.Parameters.AddWithValue("@idZona", user.idZona);
                        command.Parameters.AddWithValue("@correo", user.correo); 
                        command.Parameters.AddWithValue("@codVerificacion", codVerificacion);
                        command.Parameters["@idUsuario"].Direction = ParameterDirection.Output;
                        connection.Open();
                        var result = await command.ExecuteNonQueryAsync();
                        if (result > 0)
                        {
                            response.id = Convert.ToInt32(command.Parameters["@idUsuario"].Value);
                            response.success = true;
                            response.message = "Datos Insertados Correctamente";
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

        public async Task<Response<Usuario>> ObtenerUsuarioPorId(int id)
        {
            var response = new Response<Usuario>();
            try
            {
                using(var connection = new SqlConnection(con.getConnection()))
                {
                    using(var command = new SqlCommand("Seguridad.spObtenerUsuarioPorId", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@idUsuario", id);
                        connection.Open();
                        using(var reader = await command.ExecuteReaderAsync())
                        {
                            var user = new Usuario();
                            while (reader.Read())
                            {
                                user.idUsuario = reader.GetInt32("idUsuario");
                                user.usuario = reader.GetString("usuario");
                                user.password = reader.GetString("password");
                                user.nombre = reader.GetString("nombre");
                                user.apellido = reader.GetString("apellido");
                                user.direccion = reader.GetString("direccion");
                                user.idZona = reader.GetInt32("idZona");
                                user.idRol = reader.GetInt32("idRol");
                                var fecha = reader.GetDateTime("fecha");
                                user.fecha = fecha.ToString("MM/dd/yyyy");
                                user.activo = reader.GetBoolean("activo");
                                user.idTipoUsuario = reader.GetInt32("idTipoUsuario");
                                user.idEmpresa = reader.GetInt32("idEmpresa");
                                user.correo = reader.GetString("email");
                            }

                            response.success = true;
                            response.message = "Datos Obtenidos Correctamente";
                            response.id = id;
                            response.Data = user;
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

        public async Task<Response<Usuario>> ObtenerUsuarioPorTelefono(string tel)
        {
            var response = new Response<Usuario>();
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using (var command = new SqlCommand("Seguridad.spBuscarUsuarioPorTelefono", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@telefono", tel);
                        connection.Open();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            var user = new Usuario();
                            while (reader.Read())
                            {
                                user.idUsuario = reader.GetInt32("idUsuario");
                                user.nombre = reader.GetString("nombre");
                                user.apellido = reader.GetString("apellido");
                                user.direccion = reader.GetString("direccion");
                                user.nombreZona = reader.GetString("nombreZona");
                            }

                            response.success = true;
                            response.message = "Datos Obtenidos Correctamente";
                            response.id = user.idUsuario;
                            response.Data = user;
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

        public async Task<Response<List<Usuario>>> ObtenerUsuarios()
        {
            var response = new Response<List<Usuario>>();
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using (var command = new SqlCommand("Seguridad.spObtenerUsuarios", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            var list = new List<Usuario>();
                            while (reader.Read())
                            {
                                list.Add(new Usuario
                                {
                                    idUsuario = reader.GetInt32("idUsuario"),
                                    usuario = reader.GetString("usuario"),
                                    password = reader.GetString("password"),
                                    nombre = reader.GetString("nombre"),
                                    apellido = reader.GetString("apellido"),
                                    direccion = reader.GetString("direccion"),
                                    idZona = reader.GetInt32("idZona"),
                                    idRol = reader.GetInt32("idRol"),
                                    fecha = reader.GetDateTime("fecha").ToString("MM/dd/yyyy"),
                                    activo = reader.GetBoolean("activo"),
                                    idTipoUsuario= reader.GetInt32("idTipoUsuario"),
                                    verificado = reader.GetBoolean("verificado"),
                                    telefono = reader.GetString("telefono")

                                });
                            }

                            response.success = true;
                            response.message = "Datos Obtenidos Correctamente";
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

        public async Task<ResponseBase> ActivarUsuario(int id, Boolean activo)
        {
            var response = new ResponseBase();
            try
            {
                using(var connection = new SqlConnection(con.getConnection()))
                {
                    using(var command= new SqlCommand("Seguridad.spActivarUsuario",connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@idUsuario", id);
                        command.Parameters.AddWithValue("@activo", activo);
                        connection.Open();
                        var result = await command.ExecuteNonQueryAsync();
                        if (result > 0)
                        {
                            response.success = true;
                            response.message = "Datos Actualizados Correctamente";
                            response.id = id;
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

        public async Task<ResponseBase> RegistrarUsuarioEmpresa(Usuario user, Empresa empresa)
        {
            var response = new ResponseBase();
            try
            {
                using(var connection = new SqlConnection(con.getConnection()))
                {
                    using(var command = new SqlCommand("Seguridad.spRegistrarUsuario_Empresa", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idEmpresaOut", 0);
                        command.Parameters.AddWithValue("@nombre",empresa.nombre);
                        command.Parameters.AddWithValue("@direccion",empresa.direccion);
                        command.Parameters.AddWithValue("@rfc",empresa.rfc);
                        command.Parameters.AddWithValue("@encargado",empresa.encargado);
                        command.Parameters.AddWithValue("@vision",empresa.vision);
                        command.Parameters.AddWithValue("@mision",empresa.mision);
                        command.Parameters.AddWithValue("@telefono",empresa.telefono);
                        command.Parameters.AddWithValue("@urlLogo",empresa.urlLogo);
                        command.Parameters.AddWithValue("@usuario",user.usuario);
                        command.Parameters.AddWithValue("@password",user.password);
                        command.Parameters.AddWithValue("@idZona",user.idZona);
                        command.Parameters.AddWithValue("@idRol",user.idRol);
                        command.Parameters.AddWithValue("@idTipoUsuario",user.idTipoUsuario);
                        command.Parameters.AddWithValue("@activo",user.activo);
                        command.Parameters.AddWithValue("@idRolEmpresa",empresa.idRolEmpresa);
                        command.Parameters["@idEmpresaOut"].Direction = ParameterDirection.Output;
                        connection.Open();
                        var result = await command.ExecuteNonQueryAsync();
                        if (result > 2)
                        {
                            response.success = true;
                            response.id = Convert.ToInt32(command.Parameters["@idEmpresaOut"].Value);
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

        public async Task<ResponseBase> actualizarLogo(int id,string logo, string img)
        {
            var response = new ResponseBase();
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using (var command = new SqlCommand("Seguridad.spActualizarLogoEmpresa", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@idEmpresa", id);
                        command.Parameters.AddWithValue("@imagen",img);
                        command.Parameters.AddWithValue("@url", logo);
                        connection.Open();
                        var result = await command.ExecuteNonQueryAsync();
                        if (result > 1)
                        {
                            response.success = true;
                            response.message = "Datos Actualizados Correctamente";
                            response.id = id;
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

        public async Task<Response<List<HistorialUsuario>>> ObtenerHistorialCompraUsuario(int idUsuario)
        {
            var response = new Response<List<HistorialUsuario>>();
            var linkback = _configuration.GetValue<string>("backlink");
            try
            {
                using(var connection = new SqlConnection(con.getConnection()))
                {
                    using(var command = new SqlCommand("Reporte.spObtenerHistorialByUsuario", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idUsuario", idUsuario);
                        connection.Open();
                        using(var reader = await command.ExecuteReaderAsync())
                        {
                            var list = new List<HistorialUsuario>();
                            while (reader.Read())
                            {
                                var histo = new HistorialUsuario();
                                histo.idTicket = reader.GetInt32("idTicket");
                                histo.fecha = reader.GetString("fecha");
                                histo.nombreEmpresa = reader.GetString("nombreEmpresa");
                                histo.idEmpresa = reader.GetInt32("idEmpresa");
                                histo.imagen = reader.GetString("imagen");
                                histo.logo = $"http://{linkback}/api/empresa/logo/" + histo.idEmpresa + "/" + histo.imagen;
                                list.Add(histo);
                            }

                            response.Data = list;
                            response.success = true;
                            response.message = "datos obtenidos correctamente";
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
