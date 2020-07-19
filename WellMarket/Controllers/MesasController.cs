﻿using System;
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
    }
}
