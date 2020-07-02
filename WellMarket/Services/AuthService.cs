using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WellMarket.Entities;
using WellMarket.Helpers;
using WellMarket.Responses;

namespace WellMarket.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> Authenticate(string user, string password, int tipo);
    }
    public class AuthService: IAuthService
    {
        private readonly AppSettings appSettings;
        private readonly IUserService usuariorepo;

        public AuthService(IOptions<AppSettings> appSettings, IUserService usuariorepo)
        {
            this.appSettings = appSettings.Value;
            this.usuariorepo = usuariorepo;
        }

        public async Task<AuthResponse> Authenticate(string user, string password, int tipo)
        {
            var AuthResponse = new AuthResponse();
            var _user = await usuariorepo.GetUsuario(user);
            if (_user == null)
            {
                AuthResponse.success = false;
                AuthResponse.messages = "Usuario no Registrado";
                return AuthResponse;
            }

            if (!_user.password.Equals(password))
            {
                AuthResponse.success = false;
                AuthResponse.messages = "Contraseña del usuario no valida";
                return AuthResponse;
            }
            if(_user.activo == false)
            {
                AuthResponse.success = false;
                AuthResponse.messages = "Usuario Inactivo";
                return AuthResponse;
            }
            if (_user.idTipoUsuario != tipo)
            {
                AuthResponse.success = false;
                AuthResponse.messages = "Usuario no implicito en esta area";
                return AuthResponse;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(this.appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,_user.idUsuario.ToString()),
                    new Claim(ClaimTypes.Role,_user.idRol.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            AuthResponse.token = tokenHandler.WriteToken(token);
            AuthResponse.success = true;
            AuthResponse.user = _user;
            AuthResponse.messages = "credenciales correctas";
            return AuthResponse;

        }
    }
}
