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
    public class GastosController : ControllerBase
    {
        private readonly IGasto gasto;

        public GastosController(IGasto gasto)
        {
            this.gasto = gasto;
        }

        [HttpGet]
        public async Task<ActionResult>ObtenerGastosPorIdEmpresa([FromQuery]int idEmpresa,[FromQuery]string fecha)
        {
            var response = new Response<List<Gasto>>();
            try
            {
                response = await this.gasto.ObtenerGastosPorIdEmpresa(idEmpresa, fecha);
            }
            catch(Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult>IngresarGasto([FromBody]Gasto gasto)
        {
            var response = new ResponseBase();
            try
            {
                response = await this.gasto.InsertarGasto(gasto);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);
        }

        [HttpDelete("{idGasto}")]
        public async Task<ActionResult>EliminarGasto(int idGasto)
        {
            var response = new ResponseBase();
            try
            {
                response = await this.gasto.EliminarGasto(idGasto);
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
