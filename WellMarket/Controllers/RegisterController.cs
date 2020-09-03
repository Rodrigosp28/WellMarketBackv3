using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using WellMarket.Entities;
using WellMarket.Helpers;
using WellMarket.Repository;
using WellMarket.Responses;

namespace WellMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IConnection con;
        private readonly IHostingEnvironment environment;
        private readonly IUsuarioRepository usuariorepo;

        public RegisterController(IConnection con, IHostingEnvironment environment, IUsuarioRepository usuariorepo)
        {
            this.con = con;
            this.environment = environment;
            this.usuariorepo = usuariorepo;
        }

        [HttpPost]
        public async Task<ActionResult> RegistrarEmpresa(IFormCollection formdata)
        {
            var response = new ResponseBase();
            var empresa = new Empresa();
            var usuario = new Usuario();
            List<string> fileExtension = new List<string>() { ".png",".jpg","jpeg" };

            try
            {
                empresa.nombre = formdata["nombre"];
                empresa.direccion = formdata["direccion"];
                empresa.rfc = formdata["rfc"];
                empresa.encargado = formdata["encargado"];
                empresa.vision = formdata["vision"];
                empresa.mision = formdata["mision"];
                empresa.telefono = formdata["telefono"];
                usuario.usuario = formdata["usuario"];
                usuario.password = formdata["password"];
                usuario.idZona = int.Parse(formdata["idZona"]);
                usuario.idRol = int.Parse(formdata["idRol"]);
                usuario.idTipoUsuario = int.Parse(formdata["idTipoUsuario"]);
                usuario.activo = bool.Parse(formdata["activo"]);
                empresa.idRolEmpresa = int.Parse(formdata["idRolEmpresa"]);
                empresa.urlLogo = "ruta";

                var files = HttpContext.Request.Form.Files;
                
                foreach (var file in files)
                {
                    var filename = file.FileName;
                    var ext = Path.GetExtension(filename);
                    var buffer = file.Length;
                    double mb = (buffer / 1024f) / 1024f;
                    if (mb > 30)
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
                        response = await usuariorepo.RegistrarUsuarioEmpresa(usuario, empresa);
                        if (response.success == false)
                        {
                            return StatusCode(403, response);
                        }
                        var idEmpresa = response.id;
                        var carpeta = filee + "\\" + "Content" + "\\" + "empresa" + "\\" + response.id;
                        var filePath = filee + "\\" + "Content" + "\\" + "empresa" + "\\" + response.id + "\\" + "logo" +ext;
                        var ruta = "\\" + "Content" + "\\" + "empresa" + "\\" + response.id + "\\" + "logo" + ext;
                        var img = "logo" + ext;


                        if (!Directory.Exists(carpeta))
                        {
                            Directory.CreateDirectory(carpeta);
                        }

                        using (var stream = System.IO.File.Create(filePath))
                        {
                            await file.CopyToAsync(stream);
                            response = await usuariorepo.actualizarLogo(idEmpresa, ruta,img);
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

        [HttpPost("user")]
        public async Task<ActionResult>RegistrarUsuario(Usuario user)
        {
            var response = new ResponseBase();
            try
            {
                response = await this.usuariorepo.RegistrarUsuario(user);
            }
            catch(Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult> prueba()
        {
            var filePath = "hello";
            
            return Ok(filePath);
        }
    }
    public class InMemoryMultipartFormDataStreamProvider : MultipartStreamProvider
    {
        private NameValueCollection _formData = new NameValueCollection();
        private List<HttpContent> _fileContents = new List<HttpContent>();

        // Set of indexes of which HttpContents we designate as form data
        private Collection<bool> _isFormData = new Collection<bool>();

        /// <summary>
        /// Gets a <see cref="NameValueCollection"/> of form data passed as part of the multipart form data.
        /// </summary>
        public NameValueCollection FormData
        {
            get { return _formData; }
        }

        /// <summary>
        /// Gets list of <see cref="HttpContent"/>s which contain uploaded files as in-memory representation.
        /// </summary>
        public List<HttpContent> Files
        {
            get { return _fileContents; }
        }

        public override Stream GetStream(HttpContent parent, HttpContentHeaders headers)
        {
            // For form data, Content-Disposition header is a requirement
            ContentDispositionHeaderValue contentDisposition = headers.ContentDisposition;
            if (contentDisposition != null)
            {
                // We will post process this as form data
                _isFormData.Add(String.IsNullOrEmpty(contentDisposition.FileName));

                return new MemoryStream();
            }

            // If no Content-Disposition header was present.
            throw new InvalidOperationException(string.Format("Did not find required '{0}' header field in MIME multipart body part..", "Content-Disposition"));
        }

        /// <summary>
        /// Read the non-file contents as form data.
        /// </summary>
        /// <returns></returns>
        public override async Task ExecutePostProcessingAsync()
        {
            // Find instances of non-file HttpContents and read them asynchronously
            // to get the string content and then add that as form data
            for (int index = 0; index < Contents.Count; index++)
            {
                if (_isFormData[index])
                {
                    HttpContent formContent = Contents[index];
                    // Extract name from Content-Disposition header. We know from earlier that the header is present.
                    ContentDispositionHeaderValue contentDisposition = formContent.Headers.ContentDisposition;
                    string formFieldName = UnquoteToken(contentDisposition.Name) ?? String.Empty;

                    // Read the contents as string data and add to form data
                    string formFieldValue = await formContent.ReadAsStringAsync();
                    FormData.Add(formFieldName, formFieldValue);
                }
                else
                {
                    _fileContents.Add(Contents[index]);
                }
            }
        }

        /// <summary>
        /// Remove bounding quotes on a token if present
        /// </summary>
        /// <param name="token">Token to unquote.</param>
        /// <returns>Unquoted token.</returns>
        private static string UnquoteToken(string token)
        {
            if (String.IsNullOrWhiteSpace(token))
            {
                return token;
            }

            if (token.StartsWith("\"", StringComparison.Ordinal) && token.EndsWith("\"", StringComparison.Ordinal) && token.Length > 1)
            {
                return token.Substring(1, token.Length - 2);
            }

            return token;
        }
    }
}
