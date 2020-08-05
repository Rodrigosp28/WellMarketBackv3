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
    public class ReporteController : ControllerBase
    {
        private readonly IReporte reporte;

        public ReporteController(IReporte reporte)
        {
            this.reporte = reporte;
        }

        [HttpGet("dia")]
        public async Task<ActionResult>ObtenerReportePorDia([FromQuery]int idEmpresa,[FromQuery] string fecha)
        {
            var response = new Response<Reporte>();
            try
            {
                response = await this.reporte.ObtenerReportePorDia(idEmpresa, fecha);
            }
            catch(Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);
        }

        [HttpGet("mes")]
        public async Task<ActionResult> ObtenerReportePorMes([FromQuery] int idEmpresa, [FromQuery]int mes)
        {
            var response = new Response<Reporte>();
            try
            {
                response = await this.reporte.ObtenerReportePorMes(idEmpresa, mes);

            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);
        }

        [HttpPost("intervalo")]
        public async Task<ActionResult> ObtenerReportePorIntervalo([FromBody]IntervaloTicket i)
        {
            var response = new Response<Reporte>();
            try
            {
                response = await this.reporte.ObtenerReportePorIntervalo(i);

            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);
        }

        [HttpGet("producto/dia")]
        public async Task<ActionResult>ObtenerReporteProductoPorDia([FromQuery]int idEmpresa,[FromQuery]string fecha)
        {
            var response = new Response<List<ReporteProducto>>();
            try
            {
                response = await this.reporte.ObtenerReporteProductoPorDia(idEmpresa, fecha);

            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);
        }

        [HttpGet("producto/mes")]
        public async Task<ActionResult> ObtenerReporteProductoPormes([FromQuery] int idEmpresa, [FromQuery]int mes)
        {
            var response = new Response<List<ReporteProducto>>();
            try
            {
                response = await this.reporte.ObtenerReporteProductoPorMes(idEmpresa, mes);

            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);
        }

        [HttpPost("producto/intervalo")]
        public async Task<ActionResult> ObtenerReporteProductoPorIntervalo([FromBody]IntervaloTicket i)
        {
            var response = new Response<List<ReporteProducto>>();
            try
            {
                response = await this.reporte.ObtenerReporteProductoPorIntervalo(i);

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
