using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WellMarket.Entities;
using WellMarket.Entities.Request;
using WellMarket.Repository;
using WellMarket.Responses;

namespace WellMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentaController : ControllerBase
    {
        private readonly IVentas ventas;

        public VentaController(IVentas ventas)
        {
            this.ventas = ventas;
        }

        [HttpPost]
        public async Task<ActionResult> InsertarVenta([FromBody]VentaRequest vr)
        {
            var response = new ResponseBase();
            try
            {
               response = await this.ventas.InsertarVenta(vr);
            }
            catch(Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);
        }

        [HttpPost("ticket/today")]
        public async Task<ActionResult>ObtenerTicketsdelDia([FromBody]EmpresaFecha ef)
        {
            var response = new Response<List<Ticket>>();
            try
            {
                response = await this.ventas.ObtenerTicketsPorIdEmpresa(ef.idEmpresa, ef.fecha);
            }
            catch(Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);
        }

        [HttpPost("ticket/iniciar")]
        public async Task<ActionResult>AbrirTicket([FromBody]InsertTicket it)
        {
            var response = new ResponseBase();
            try
            {
                response = await this.ventas.IniciarTicket(it);
            }
            catch(Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);
            
        }

        [HttpPost("ticket/cerrar")]
        public async Task<ActionResult> CerrarTicket([FromBody] CerrarTicket ct)
        {
            var response = new ResponseBase();
            try
            {
                response = await this.ventas.CerrarTicket(ct);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);

        }

        [HttpPost("ticket/cancelar")]
        public async Task<ActionResult> CancelarTicket([FromBody] CerrarTicket ct)
        {
            var response = new ResponseBase();
            try
            {
                response = await this.ventas.CancelarTicket(ct);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);

        }

        [HttpPost("ticket/venta")]
        public async Task<ActionResult> VentaTicket([FromBody] Venta v)
        {
            var response = new ResponseBase();
            try
            {
                response = await this.ventas.InsertarVentaTicket(v);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);

        }

        [HttpGet("ticket/venta/{idTicket}")]
        public async Task<ActionResult> ObtenerVentaTicket(int idTicket)
        {
            var response = new ResponseBase();
            try
            {
                response = await this.ventas.ObtenerVentaTicket(idTicket);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);

        }

        [HttpDelete("ticket/venta/{idVenta}")]
        public async Task<ActionResult> EliminarVentaTicket(int idVenta)
        {
            var response = new ResponseBase();
            try
            {
                response = await this.ventas.EliminarVentaTicket(idVenta);
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
