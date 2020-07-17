using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WellMarket.Entities;
using WellMarket.Entities.Request;
using WellMarket.Entities.Responses;
using WellMarket.Repository;
using WellMarket.Responses;

namespace WellMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpresaController : ControllerBase
    {
        private readonly IEmpresa empresa;
        private readonly IHostingEnvironment environment;

        public EmpresaController(IEmpresa empresa, IHostingEnvironment environment)
        {
            this.empresa = empresa;
            this.environment = environment;
        }

        [HttpPut("abrir")]
        public async Task<ActionResult> abrirEmpresa([FromBody] Empresa empresa)
        {
            var response = new ResponseBase();
            try
            {
                response = await this.empresa.cambiarestadoEmpresa(empresa);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);

        }

        [HttpGet("{idEmpresa}/byId")]
        public async Task<ActionResult> obtenerEmpresaById(int idEmpresa)
        {
            var response = new Response<Empresa>();
            try
            {
                response = await this.empresa.obtenerEmpresaPorId(idEmpresa);
            }
            catch(Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);
        }

        // obtiene imagen especifica y fisica
        [HttpGet("logo/{idEmpresa}/{imagen}")]
        public async Task<IActionResult> ImagenProducto(int idEmpresa, int idProducto, string imagen)
        {
            Byte[] b;
            var filee = Path.Combine(environment.ContentRootPath);
            var filePath = filee + "\\" + "Content" + "\\" + "empresa" + "\\" + idEmpresa + "\\" + imagen;
            try
            {
                b = await System.IO.File.ReadAllBytesAsync(filePath);

                return File(b, "image/jpg");
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }

        }

        [HttpPost("dashboard")]
        public async Task<ActionResult>obtenerDashboard([FromBody]EmpresaFecha fe)
        {
            var response = new Response<Dashboard>();
            try
            {
                response = await this.empresa.obtenerDashboard(fe.idEmpresa, fe.fecha);
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
