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
    public class ProductoController : ControllerBase
    {
        private readonly IProducto producto;
        private readonly IHostingEnvironment environment;

        public ProductoController(IProducto producto, IHostingEnvironment environment)
        {
            this.producto = producto;
            this.environment = environment;
        }

        [HttpGet]
        public async Task<ActionResult> ObtenerProductos()
        {
            var response = new Response<List<Producto>>();
            try
            {
                response = await this.producto.ObtenerProductos();
            }
            catch(Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);
        }

        [HttpGet("{id}/ByEmpresa")]
        public async Task<ActionResult> ObtenerProductosPorEmpresa(int id)
        {
            var response = new Response<List<Producto>>();
            try
            {
                response = await this.producto.ObtenerProductoPorEmpresa(id);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);
        }

        [HttpGet("{id}/ByEmpresa/disponibles")]
        public async Task<ActionResult> ObtenerProductosDisponiblesPorEmpresa(int id)
        {
            var response = new Response<List<Producto>>();
            try
            {
                response = await this.producto.ObtenerProductoDisponiblePorEmpresa(id);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> ObtenerProductosPorId(int id)
        {
            var response = new Response<Producto>();
            try
            {
                response = await this.producto.ObtenerProductoPorId(id);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult> InsertarProductos([FromBody]Producto producto)
        {
            var response = new ResponseBase();
            try
            {
                response = await this.producto.InsertarProducto(producto);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> ActualizarProductos(int id,[FromBody] Producto producto)
        {
            var response = new ResponseBase();
            try
            {
                response = await this.producto.ActualizarProducto(id, producto);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);
        }

        [HttpPost("imagen")]
        public async Task<ActionResult> ImagenProducto(IFormCollection formdata)
        {
            var response = new ResponseBase();
            var imgp = new Imagenes_Producto();
            List<string> fileExtension = new List<string>() { ".png", ".jpg", "jpeg" };

            try
            {
                imgp.idProducto = int.Parse(formdata["idProducto"]);
                var idEmpresa = formdata["idEmpresa"];

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
                        var date = DateTime.Now;
                        var id = date.Millisecond;
                        var carpeta = filee + "\\" + "Content" + "\\" + idEmpresa +"\\" + imgp.idProducto;
                        var filePath = filee + "\\" + "Content" + "\\" + idEmpresa + "\\" + imgp.idProducto+"\\" + id + ext;
                        var ruta = "\\" + "Content" + "\\" + idEmpresa + "\\" + imgp.idProducto + "\\" + id + ext;
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
                            
                            response = await this.producto.InsertarImagenProducto(imgp);
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

        [HttpGet("imagen/{idEmpresa}/{idProducto}/{imagen}")]
        public async Task<IActionResult> ImagenProducto(int idEmpresa, int idProducto, string imagen)
        {
            Byte[] b;
            var filee = Path.Combine(environment.ContentRootPath);
            var filePath = filee + "\\" + "Content" + "\\" + idEmpresa + "\\" + idProducto + "\\" + imagen;
            try
            {
                b = await System.IO.File.ReadAllBytesAsync(filePath);
                
                return File(b, "image/jpg");
            }
            catch(Exception ex)
            {
                return StatusCode(500);
            }
            

        }

        [HttpDelete("imagen/{idEmpresa}/{idProducto}/{imagen}")]
        public async Task<IActionResult> EliminarImagenProducto(int idEmpresa, int idProducto, string imagen)
        {
            var response = new ResponseBase();
            var filee = Path.Combine(environment.ContentRootPath);
            var filePath = filee + "\\" + "Content" + "\\" + idEmpresa + "\\" + idProducto + "\\" + imagen;
            try
            {

                response = await this.producto.EliminarImagenProducto(idEmpresa, imagen);
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
                return StatusCode(500,response);
            }
            return Ok(response);


        }

        [HttpGet("imagen/{id}/ByProducto")]
        public async Task<ActionResult> ObtenerImagenesPorProducto(int id)
        {
            var response = new Response<List<Imagenes_Producto>>();
            try
            {
                response = await this.producto.ObtenerImagenesPorProducto(id);
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
