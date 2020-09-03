using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WellMarket.Entities;
using WellMarket.Repository;
using WellMarket.Responses;

namespace WellMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CocinaController : ControllerBase
    {
        private readonly ICocina cocina;

        public CocinaController(ICocina cocina)
        {
            this.cocina = cocina;
        }

        [HttpPost("entrada")]
        public async Task<ActionResult> CocinaEntrada([FromBody]Cocina c)
        {
            var response = new ResponseBase();
            try
            {
                response = await this.cocina.CocinaEntrada(c);
            }
            catch(Exception ex)
            {
                response.success = true;
                response.message = ex.Message;
            }
            return Ok(response);

        }

        [HttpPost("salida")]
        public async Task<ActionResult> CocinaSalida([FromBody] Cocina c)
        {
            var response = new ResponseBase();
            try
            {
                response = await this.cocina.CocinaSalida(c);

            }
            catch (Exception ex)
            {
                response.success = true;
                response.message = ex.Message;
            }
            return Ok(response);

        }

        [HttpGet("productos")]
        public async Task<ActionResult> ObtenerProductosCocina([FromQuery]int idEmpresa,[FromQuery]string fecha)
        {
            var response = new Response<List<Cocina>>();
            try
            {
                response = await this.cocina.ObtenerProductoCocina(idEmpresa, fecha);

            }
            catch (Exception ex)
            {
                response.success = true;
                response.message = ex.Message;
            }
            return Ok(response);

        }

        [HttpGet("productos/dia")]
        public async Task<ActionResult> ObtenerProductosCocinaDelDia([FromQuery] int idEmpresa, [FromQuery] string fecha)
        {
            var response = new Response<List<Cocina>>();
            try
            {
                response = await this.cocina.ObtenerProductoCocinaDelDia(idEmpresa, fecha);

            }
            catch (Exception ex)
            {
                response.success = true;
                response.message = ex.Message;
            }
            return Ok(response);

        }

        [HttpGet("productos/cantidad")]
        public async Task<ActionResult> ObtenerCantidadProductosCocina([FromQuery] int idEmpresa, [FromQuery] string fecha)
        {
            var response = new ResponseBase();
            try
            {
                response = await this.cocina.ObtenerCantidadProductosCocina(idEmpresa, fecha);

            }
            catch (Exception ex)
            {
                response.success = true;
                response.message = ex.Message;
            }
            return Ok(response);
        }
    }
}
