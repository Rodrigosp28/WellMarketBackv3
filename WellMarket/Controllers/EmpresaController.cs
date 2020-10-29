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

        [HttpGet("all")]
        public async Task<ActionResult>ObtenerEmpresasAll()
        {
            var response = new Response<List<Empresa>>();
            try
            {
                response = await empresa.ObtenerEmpresasaAll();
            }
            catch(Exception ex)
            {
                response.success = true;
                response.message = ex.Message;
            }
            return Ok(response);
        }

        [HttpGet("activardesactivar/{idEmpresa}")]
        public async Task<ActionResult>ActivarDesactivarEmpresa(int idEmpresa,[FromQuery]Boolean opt)
        {
            var response = new ResponseBase();
            try
            {
                response = await empresa.ActivarDesactivarEmpresa(idEmpresa, opt);
            }
            catch(Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);
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

        [HttpPut("{idEmpresa}")]
        public async Task<ActionResult>ActualizarDatosEmpresa(int idEmpresa,[FromBody]Empresa empresa)
        {
            var response = new ResponseBase();
            try
            {
                response = await this.empresa.ActualizarDatosEmpresa(idEmpresa,empresa);
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

        [HttpPost("logo/actualizar")]
        public async Task<ActionResult>ActualizarLogo(IFormCollection formdata)
        {
            var response = new ResponseBase();
            var logo = new Logo();
            List<string> fileExtension = new List<string>() { ".png", ".jpg", "jpeg" };
            try
            {
                logo.idLogo = int.Parse(formdata["idLogo"]);
                logo.idEmpresa = int.Parse(formdata["idEmpresa"]);

                var files = HttpContext.Request.Form.Files;
                foreach (var file in files)
                {
                    var filename = file.FileName;
                    var ext = Path.GetExtension(filename);
                    var buffer = file.Length;
                    double mb = (buffer / 1024f) / 1024f;
                    if (mb > 20)
                    {
                        response.success = false;
                        response.message = "tamaño de archivo demasiado grande";
                        return StatusCode(500, response);
                    }
                    if (!fileExtension.Contains(ext))
                    {
                        response.success = false;
                        response.message = "extension de archivo no permitida";
                        return StatusCode(500, response);
                    }
                    if (file.Length > 0)
                    {
                        var filee = Path.Combine(environment.ContentRootPath);
                        var carpeta = filee + "\\" + "Content" + "\\" + "empresa" + "\\" + logo.idEmpresa;
                        var filePath = filee + "\\" + "Content" + "\\" + "empresa" + "\\" + logo.idEmpresa + "\\"  + "logo" + ext;
                        var ruta = "Content" + "\\" + "empresa" + "\\" + logo.idEmpresa + "\\" + "logo" + ext;
                        logo.imagen = "logo" + ext;
                        logo.url = ruta;
                        if (!Directory.Exists(carpeta))
                        {
                            Directory.CreateDirectory(carpeta);
                        }
                        using (var stream = System.IO.File.Create(filePath))
                        {

                            await file.CopyToAsync(stream);

                            response = await this.empresa.actualizarLogo(logo);
                            if (response.success == false)
                            {
                                return StatusCode(403, response);
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);
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

        [HttpGet("municipio/{idMunicipio}")]
        public async Task<ActionResult>ObtenerEmpresasaporMunicipio(int idMunicipio,[FromQuery]int pag=1)
        {
            var response = new Response<List<Empresa>>();
            try
            {
                response = await this.empresa.ObtenerEmpresaPorMunicipio(idMunicipio,pag);
            }
            catch(Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);
        }

        [HttpGet("zona/{idZona}")]
        public async Task<ActionResult> ObtenerEmpresaPorZona(int idZona,[FromQuery]int pag=1)
        {
            var response = new Response<List<Empresa>>();
            try
            {
                response = await this.empresa.ObtenerEmpresaPorZona(idZona,pag);
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
