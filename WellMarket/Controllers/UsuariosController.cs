using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WellMarket.Entities;
using WellMarket.Repository;
using WellMarket.Responses;

namespace WellMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioRepository usuario;

        public UsuariosController(IUsuarioRepository Usuario)
        {
            usuario = Usuario;
        }

        [HttpGet]
        public async Task<ActionResult> GetUsuarios()
        {
            var response = new Response<List<Usuario>>();
            try
            {
                response = await usuario.ObtenerUsuarios();
            }
            catch(Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
                return StatusCode(500, response);
            }
            return Ok(response);
        }

        //obtiene el historial de compra del usuario
        [HttpGet("historialCompra/{idUsuario}")]
        public async Task<ActionResult>ObtenerHistorialCompraUsuario(int idUsuario)
        {
            var response = new Response<List<HistorialUsuario>>();
            try
            {
                response = await usuario.ObtenerHistorialCompraUsuario(idUsuario);
            }
            catch(Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);

        }

        [HttpGet("verificacion/{idUsuario}")]
        public async Task<ActionResult> VerificacionUsuario(int idUsuario)
        {
            var response = new ResponseBase();
            try
            {
                response = await usuario.VerificacionUsuario(idUsuario);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
                return StatusCode(500, response);
            }
            return Ok(response);
        }

        [HttpGet("{id}/ById")]
        public async Task<ActionResult> GetUsuario(int id)
        {
            var response = new Response<Usuario>();
            try
            {
                response = await usuario.ObtenerUsuarioPorId(id);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
                return StatusCode(500, response);
            }
            return Ok(response);
        }

        //obtener usuario por su telefono
        [HttpGet("{tel}/ByTelefono")]
        public async Task<ActionResult> GetUsuarioPorTelefono(string tel)
        {
            var response = new Response<Usuario>();
            try
            {
                response = await usuario.ObtenerUsuarioPorTelefono(tel);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
                return StatusCode(500, response);
            }
            return Ok(response);
        }

        [HttpGet("active/{id}/{active}")]
        public async Task<ActionResult> ActivarUsuario(int id, Boolean active)
        {
            var response = new ResponseBase();
            try
            {
                response = await usuario.ActivarUsuario(id, active);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
                return StatusCode(500, response);
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult> PostUsuario([FromBody] Usuario user)
        {
            var response = new ResponseBase();
            try
            {
                response = await usuario.InsertarUsuario(user);
                if(response.success == false)
                {
                    return StatusCode(500, response);
                }
            }
            catch(Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
                return StatusCode(500, response);
            }
            return Ok(response);
        }
    }
}
