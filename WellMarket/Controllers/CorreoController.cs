using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WellMarket.Responses;
using WellMarket.Services;

namespace WellMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CorreoController : ControllerBase
    {
        private readonly ICorreo correo;
        public CorreoController(ICorreo _correo)
        {
            this.correo = _correo;
        }

        [HttpGet("enviarConfirmacion")]
        public async Task<ActionResult>EnviarCorreoConfirmacion([FromQuery]string correo)
        {
            var response = new ResponseBase();
            try
            {
                response = await this.correo.VerificacionCorreo(correo);
            }
            catch(Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);
        }

    }
}
