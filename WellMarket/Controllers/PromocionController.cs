using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WellMarket.Entities;
using WellMarket.Repository;
using WellMarket.Responses;

namespace WellMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromocionController : ControllerBase
    {
        private readonly IPromocion promocion;
        private readonly IHostingEnvironment environment;
        public PromocionController(IPromocion promo, IHostingEnvironment environment)
        {
            this.promocion = promo;
            this.environment = environment;
        }

        //inserta una promocion en la base de datos
        [HttpPost]
        public async Task<ActionResult> InsertarPromocion([FromBody] Promocion promo)
        {
            var response = new ResponseBase();
            try
            {
                response = await this.promocion.InsertarPromocion(promo);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);
        }

        //sube imagen fisica de la promocion
        [HttpPost("imagen")]
        public async Task<ActionResult> ImagenProducto(IFormCollection formdata)
        {
            var response = new ResponseBase();
            var imgp = new Imagen_promocion();
            List<string> fileExtension = new List<string>() { ".png", ".jpg", ".jpeg" };

            try
            {
                imgp.idPromocion = int.Parse(formdata["idPromocion"]);
                var idEmpresa = formdata["idEmpresa"];

                var files = HttpContext.Request.Form.Files;

                foreach (var file in files)
                {
                    var filename = file.FileName;
                    var ext = Path.GetExtension(filename);
                    var buffer = file.Length;
                    double mb = (buffer / 1024f) / 1024f;
                    if (mb > 10)
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
                        var date = DateTime.Now;
                        var id = date.Millisecond;
                        var carpeta = filee + "\\" + "Content" + "\\" + "empresa"  +"\\" + idEmpresa + "\\" + "promociones" + "\\" + imgp.idPromocion;
                        var filePath = filee + "\\" + "Content" + "\\" + "empresa" + "\\" + idEmpresa + "\\" + "promociones" + "\\" + imgp.idPromocion + "\\" + id + ext;
                        var ruta = "\\" + "Content" + "\\" + "empresa" + "\\" + idEmpresa  +"\\" + "promociones" + "\\" + imgp.idPromocion + "\\" + id + ext;
                        imgp.url = ruta;
                        imgp.idEmpresa = int.Parse(idEmpresa);
                        imgp.imagen = id.ToString() + ext;


                        if (!Directory.Exists(carpeta))
                        {
                            Directory.CreateDirectory(carpeta);
                        }

                        using (var stream = System.IO.File.Create(filePath))
                        {

                            await file.CopyToAsync(stream);

                            response = await this.promocion.InsertarImagenPromocion(imgp);
                            if (response.success == false)
                            {
                                return StatusCode(403, response);
                            }
                        }
                    }
                }


            }
            catch (Exception ex)
            {

                response.success = false;
                response.message = ex.Message;
                return StatusCode(500, response);
            }
            return Ok(response);
        }

        // elimina imagen fisica y de base de datos
        [HttpDelete("imagen/{idEmpresa}/{idPromocion}/{imagen}")]
        public async Task<IActionResult> EliminarImagenProducto(int idEmpresa, int idPromocion, string imagen,[FromQuery]int idImg)
        {
            var response = new ResponseBase();
            var filee = Path.Combine(environment.ContentRootPath);
            var filePath = filee + "\\" + "Content" + "\\" + "empresa" + "\\" + idEmpresa + "\\"+ "promociones" +"\\" + idPromocion + "\\" + imagen;
            try
            {

                response = await this.promocion.EliminarImagenPromocion(idImg);
                if (response.success == false)
                {
                    throw new Exception(response.message);
                }
                System.IO.File.Delete(filePath);
            }
            catch (DirectoryNotFoundException dirNotFound)
            {
                response.success = false;
                response.message = dirNotFound.Message;
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
                return StatusCode(500, response);
            }
            return Ok(response);


        }

        //actualiza los datos de una promocion
        [HttpPut("{id}")]
        public async Task<ActionResult> ActualizarPromocion(int id, [FromBody] Promocion promo)
        {
            var response = new ResponseBase();
            try
            {
                response = await this.promocion.ActualizarPromocion(id, promo);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);
        }

        //elimina de forma logica una promocion
        [HttpDelete("{id}")]
        public async Task<ActionResult> eliminarPromocion(int id)
        {
            var response = new ResponseBase();
            try
            {
                response = await this.promocion.EliminarPromocion(id);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);
        }

        //obtiene una promocion por id
        [HttpGet("{id}")]
        public async Task<ActionResult> ObtenerPromocionPorId(int id)
        {
            var response = new Response<Promocion>();
            try
            {
                response = await this.promocion.ObtenerPromocionPorId(id);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);
        }

        //obtiene un arreglo con una tabla de imagenes de una promocion
        [HttpGet("imagen/{id}/ByPromocion")]
        public async Task<ActionResult> ObtenerImagenesPorPromocion(int id)
        {
            var response = new Response<List<Imagen_promocion>>();
            try
            {
                response = await this.promocion.ObtenerImagenesPorPromocion(id);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);
        }

        //obtienes las promociones por empresa
        [HttpGet("{id}/ByEmpresa")]
        public async Task<ActionResult> ObtenerPromocionPorEmpresa(int id)
        {
            var response = new Response<List<Promocion>>();
            try
            {
                response = await this.promocion.ObtenerPromocionPorEmpresa(id);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);
        }

        //obtiene las promociones por municipio con paginacion
        [HttpGet("{id}/ByMunicipio/disponibles/pag")]
        public async Task<ActionResult> ObtenerPromocionesDisponiblesPorMunicipioPag(int id, [FromQuery] int pagina,[FromQuery]int dia)
        {
            var response = new Response<List<Promocion>>();
            try
            {
                response = await this.promocion.ObtenerPromocionDisponiblePorMunicipio(id, pagina, dia);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);
        }

        // obtiene imagen especifica y fisica
        [HttpGet("imagen/{idEmpresa}/{idPromocion}/{imagen}")]
        public async Task<IActionResult> ImagenProducto(int idEmpresa, int idPromocion, string imagen)
        {
            Byte[] b;
            var filee = Path.Combine(environment.ContentRootPath);
            var filePath = filee + "\\" + "Content" + "\\" + "empresa" + "\\" + idEmpresa + "\\" + "promociones" + "\\" + idPromocion + "\\" + imagen;
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
    }
}
