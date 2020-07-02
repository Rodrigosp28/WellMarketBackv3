using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WellMarket.Entities;
using WellMarket.Helpers;

namespace WellMarket.Services
{   
    public interface IUserService
    {
        Task<Usuario> GetUsuario(string usuario);
    }
    public class UserService:IUserService
    {
        private readonly IConnection con;

        public UserService(IConnection con)
        {
            this.con = con;
        }

        public async Task<Usuario> GetUsuario(string usuario)
        {
            Usuario user=null;
            try
            {
                using(var connection = new SqlConnection(con.getConnection()))
                {
                    using(var command = new SqlCommand("Seguridad.spObtenerUsuario", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@usuario", usuario);
                        connection.Open();
                        using(var reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                user = new Usuario();

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
                            }
                            return user;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                user = null;
            }
            return user;
        }


    }
}
