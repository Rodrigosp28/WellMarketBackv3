using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    public class CatalogosController : ControllerBase
    {
        private readonly IMunicipio municipio;
        private readonly IZona zona;
        private readonly IRolEmpresa rolEmpresa;
        private readonly IDisponibleP disponibleP;
        private readonly ICategoriaP categoriaP;
        private readonly IEstatusT estatusT;

        public CatalogosController(IMunicipio municipio, IZona zona, IRolEmpresa rolEmpresa, IDisponibleP disponibleP, ICategoriaP categoriaP, IEstatusT estatusT)
        {
            this.municipio = municipio;
            this.zona = zona;
            this.rolEmpresa = rolEmpresa;
            this.disponibleP = disponibleP;
            this.categoriaP = categoriaP;
            this.estatusT = estatusT;
        }

        #region Municipios

        [HttpGet("Municipios")]
        public async Task<ActionResult> ObtenerMunicipios()
        {
            var response = new Response<List<Municipio>>();
            try
            {
                response = await this.municipio.ObtenerMunicipios();
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
                return StatusCode(500, response);
            }
            return Ok(response);
        }

        #endregion

        #region Zonas

        [HttpGet("Zona/{id}/ByMunicipio")]
        public async Task<ActionResult>ObtenerZonaPorMunicipio(int id)
        {
            var response = new Response<List<Zona>>();
            try
            {
                response = await zona.ObtenerZonasPorMunicipio(id);
            }
            catch(Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
                return StatusCode(500, response);
            }
            return Ok(response);
        }

        #endregion

        #region RolEmpresa

        [HttpGet("RolEmpresa")]
        public async Task<ActionResult> ObtenerRolEmpresa()
        {
            var response = new Response<List<RolEmpresa>>();
            try
            {
                response = await rolEmpresa.ObtenerRolEmpresa();
            }
            catch(Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);
        }

        #endregion

        #region Disponible_Producto

        [HttpGet("disponible/producto")]
        public async Task<ActionResult> ObtenerdisponibleProductor()
        {
            var response = new Response<List<disponible_producto>>();
            try
            {
                response = await this.disponibleP.ObtenerDisponibleProducto();
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
                return StatusCode(500, response);
            }
            return Ok(response);
        }

        #endregion

        #region Categoria_Producto

        [HttpGet("categoria/producto")]
        public async Task<ActionResult> ObtenercategoriasProductos()
        {
            var response = new Response<List<Categoria_Producto>>();
            try
            {
                response = await this.categoriaP.ObtenerCategoriaP();
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
                return StatusCode(500, response);
            }
            return Ok(response);
        }

        #endregion

        #region Estatus_Ticket

        [HttpGet("estatust")]
        public async Task<ActionResult> ObtenerEstatusT()
        {
            var response = new Response<List<EstatusTicket>>();
            try
            {
                response = await this.estatusT.ObtenerEstatus();
            }
            catch(Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);
        }

        #endregion
    }
}
