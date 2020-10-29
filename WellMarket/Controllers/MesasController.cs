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
    public class MesasController : ControllerBase
    {
        private readonly IMesa mesa;

        public MesasController(IMesa mesa)
        {
            this.mesa = mesa;
        }

        //obtiene mesas depende su tipo por idEmpresa y idtipo
        [HttpGet("{idEmpresa}/{idTipo}")]
        public async Task<ActionResult> ObtenerMesas(int idEmpresa, int idTipo)
        {
            var response = new Response<List<Mesa>>();
            try
            {
                response = await mesa.ObtenerMesasPorEmpresa(idEmpresa, idTipo);
            }
            catch(Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
                return StatusCode(500, response);
            }
            return Ok(response);
        }
            
        //obtiene el catalogo tipo de mesas
        [HttpGet("tipoMesa")]
        public async Task<ActionResult> ObtenerTipoMesa()
        {
            var response = new Response<List<TipoMesa>>();
            try
            {
                response = await this.mesa.ObtenerTiposMesa();
            }
            catch(Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);
        }

        //inserta una mesa a una empresa
        [HttpPost]
        public async Task<ActionResult> InsertarMesas([FromBody]Mesa mesa)
        {
            var response = new ResponseBase();
            try
            {
                response = await this.mesa.InsertarMesa(mesa);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
                return StatusCode(500, response);
            }
            return Ok(response);
        }

        //actualiza el nombre de la mesa
        [HttpPut("{idMesa}")]
        public async Task<ResponseBase>ActualizarMesa(int idMesa,[FromQuery]string nombre,[FromQuery]string descripcion)
        {
            ResponseBase response = new ResponseBase();
            try
            {
                response = await this.mesa.ActualizarNombreMesa(idMesa, nombre, descripcion);
            }
            catch(Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return response;
        }

        //elimina de forma logica la mesa
        [HttpDelete("{idMesa}")]
        public async Task<ResponseBase>EliminarMesa(int idMesa)
        {
            ResponseBase response = new ResponseBase();
            try
            {
                response = await this.mesa.EliminarMesa(idMesa);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return response;
        }

        //obtiene todas las mesas de una empresa
        [HttpGet("{idEmpresa}/all")]
        public async Task<Response<List<Mesa>>>ObtenerMesasAll(int idEmpresa)
        {
            Response<List<Mesa>> response = new Response<List<Mesa>>();
            try
            {
                response = await this.mesa.ObtenerMesasPorEmpresaAll(idEmpresa);
            }
            catch(Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return response;
        }
    }
}
