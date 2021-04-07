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
        public async Task<ActionResult> InsertarVenta([FromBody] VentaRequest vr)
        {
            var response = new ResponseBase();
            try
            {
                response = await this.ventas.InsertarVenta(vr);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);
        }

        [HttpGet("ticket/{id}/byId")]
        public async Task<ActionResult> ObtenerTicketPorId(int id)
        {
            var response = new Response<Ticket>();
            try
            {
                response = await this.ventas.ObtenerTicketIdD(id);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);
        }

        //obtiene los 10 productos mas vendido para el usuario por zona
        [HttpGet("masVendidos/{zona}")]
        public async Task<ActionResult> ObtenerMasVendidosByMunicipio(int zona)
        {
            var response = new Response<List<MasVendidosUsuario>>();
            try
            {
                response = await ventas.ObtenerProductosMasVendidosByMunicipio(zona);
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

        [HttpPost("ticket/intervalo")]
        public async Task<ActionResult>ObtenerTicketsPorIntervalo([FromBody]IntervaloTicket i)
        {
            var response = new Response<List<Ticket>>();
            try
            {
                response = await this.ventas.ObtenerTicketsPorIntervalo(i);
            }
            catch(Exception ex)
            {
                response.success = false;
                response.message = ex.Message;

            }
            return Ok(response);
        }

        [HttpGet("ticket/month")]
        public async Task<ActionResult>ObtenerTicketsPorMesa([FromQuery]int idEmpresa,[FromQuery]int mes)
        {
            var response = new Response<List<Ticket>>();
            try
            {
                response = await this.ventas.ObtenerTicketsPorMes(idEmpresa, mes);
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

        [HttpGet("ticket/{idTicket}/domicilio")]
        public async Task<ActionResult> InsertarDomicilioTicket(int idTicket,[FromQuery] string domicilio)
        {
            var response = new ResponseBase();
            try
            {
                response = await this.ventas.InsertarDomicilioTicket(idTicket, domicilio);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);

        }

        //agregar usuario al ticket
        [HttpGet("ticket/{idTicket}/usuario")]
        public async Task<ActionResult> InsertarUsuarioTicket(int idTicket,[FromQuery] int idUsuario)
        {
            var response = new ResponseBase();
            try
            {
                response = await this.ventas.InsertarUsuarioTicket(idTicket, idUsuario);
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

        [HttpPut("ticket/venta/{idVenta}")]
        public async Task<ActionResult> ActualizarVenta([FromBody]Venta v)
        {
            var response = new ResponseBase();
            try
            {
                response = await this.ventas.ActualizarVenta(v);
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
