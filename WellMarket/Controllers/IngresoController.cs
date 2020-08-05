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
    public class IngresoController : ControllerBase
    {
        private readonly IIngreso ingreso;

        public IngresoController(IIngreso ingreso)
        {
            this.ingreso = ingreso;
        }

        [HttpGet]
        public async Task<ActionResult> ObtenerIngresosPorIdEmpresa([FromQuery] int idEmpresa, [FromQuery] string fecha)
        {
            var response = new Response<List<Ingreso>>();
            try
            {
                response = await this.ingreso.ObtenerIngresosPorIdEmpresa(idEmpresa, fecha);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult> IngresarIngreso([FromBody] Ingreso ingreso)
        {
            var response = new ResponseBase();
            try
            {
                response = await this.ingreso.InsertarIngreso(ingreso);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);
        }

        [HttpDelete("{idIngreso}")]
        public async Task<ActionResult> EliminarGasto(int idIngreso)
        {
            var response = new ResponseBase();
            try
            {
                response = await this.ingreso.EliminarIngreso(idIngreso);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);
        }
    }
}
