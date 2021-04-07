using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using WellMarket.Entities;
using WellMarket.Helpers;
using WellMarket.Responses;

namespace WellMarket.Services
{
    public interface ICorreo
    {
        Task<ResponseBase>VerificacionCorreo(string email);
        Task<Usuario> ObtenerIdCorreo(string email);
    }
    public class CorreoService : ICorreo
    {
        private SmtpClient Cliente { get; }
        private OptionEmail Options { get; }
        private readonly IConnection con;
        private readonly IConfiguration _configuration;

        public CorreoService(IOptions<OptionEmail> options, IConnection con, IConfiguration configuration)
        {
            this.con = con;
            _configuration = configuration;
            Options = options.Value;
            Cliente = new SmtpClient()
            {
                Host = Options.Host,
                Port = Options.Port,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(Options.Email, Options.Password),
                EnableSsl = Options.EnableSsl,
            };
        }
        public async Task<ResponseBase>VerificacionCorreo(string email)
        {
            var response = new ResponseBase();
            Usuario Usuario= new Usuario();
            string asunto = "Verificacion de Email en WellMarket";
            try
            {
                Usuario = await this.ObtenerIdCorreo(email);
                if(Usuario.idUsuario==0)
                {
                    throw new Exception("Cuenta de correo electronico no registrada");
                }
                string linkFront = _configuration.GetValue<string>("frontLink");
                string link = $"http://{linkFront}/#/verificar/{Usuario.idUsuario}/verificado";
                StringBuilder mensaje = new StringBuilder();
                mensaje.Append("<h1>Bienvenido a WellMarket</h1>");
                mensaje.Append($"<h2>Usuario: {Usuario.usuario}   Contraseña: {Usuario.password}</h2>");
                mensaje.Append("<h3>Ingresa al link para verificar tu correo electronico</h3>");
                mensaje.Append($"<p><a href={link}>Link de verificacion</a></p>");
                var correo = new MailMessage(from: Options.Email, to: email, subject: asunto, body: mensaje.ToString());
                correo.IsBodyHtml = true;
                await Cliente.SendMailAsync(correo);
                response.success = true;
                response.message = "Correo Enviado Correctamente";
            }
            catch(Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return response;

        }

        public async Task<Usuario> ObtenerIdCorreo(string email)
        {
            int result=0;
            Usuario user = new Usuario();
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using(var command = new SqlCommand("Seguridad.spObtenerIdUsuario", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@email", email);
                        connection.Open();
                        using(var reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                user.idUsuario = reader.GetInt32("idUsuario");
                                user.usuario = reader.GetString("usuario");
                                user.password = reader.GetString("password");
                            }

                            if (user.idUsuario == 0)
                            {
                                result = 0;
                                return user;
                            }
                            return user;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                return user;
            }

        }
    }
}
