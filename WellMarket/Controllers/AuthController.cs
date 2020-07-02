using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WellMarket.Entities;
using WellMarket.Responses;
using WellMarket.Services;

namespace WellMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService auth;

        public AuthController(IAuthService authservice)
        {
            this.auth = authservice;
        }

        [HttpPost("{tipo}")]
        public async Task<ActionResult<AuthResponse>> Login(int tipo, [FromBody]ApplicationUser user)
        {
            var response = new AuthResponse();
            try
            {
                response = await this.auth.Authenticate(user.userName, user.Password, tipo);
                if (response.success == false)
                {
                    return StatusCode(403, response);
                }
            }
            catch(Exception ex)
            {
                response.success = false;
                response.messages = ex.Message;
                return StatusCode(500, response);
            }
            return Ok(response);
        }
    }
}
