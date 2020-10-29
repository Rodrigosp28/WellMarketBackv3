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
        private readonly ICategoriaP categoriaP; // esta es la categoria del producto basico como alimento bebida etc.
        private readonly IEstatusT estatusT;
        private readonly ICatProducto catProducto; //esta es la categoria del producto agregada por la empresa
        private readonly IDisponibilidadD _disponibilidadD; //disponibilidad de domicilio de la empresa

        public CatalogosController(IMunicipio municipio, IZona zona, IRolEmpresa rolEmpresa, IDisponibleP disponibleP, ICategoriaP categoriaP, IEstatusT estatusT, ICatProducto catProductoR,
                                    IDisponibilidadD disponibilidadD)
        {
            this.municipio = municipio;
            this.zona = zona;
            this.rolEmpresa = rolEmpresa;
            this.disponibleP = disponibleP;
            this.categoriaP = categoriaP;
            this.estatusT = estatusT;
            this.catProducto = catProductoR;
            this._disponibilidadD = disponibilidadD;
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
        public async Task<ActionResult> ObtenerZonaPorMunicipio(int id)
        {
            var response = new Response<List<Zona>>();
            try
            {
                response = await zona.ObtenerZonasPorMunicipio(id);
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

        #region RolEmpresa

        [HttpGet("RolEmpresa")]
        public async Task<ActionResult> ObtenerRolEmpresa()
        {
            var response = new Response<List<RolEmpresa>>();
            try
            {
                response = await rolEmpresa.ObtenerRolEmpresa();
            }
            catch (Exception ex)
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

        // esta es la categoria del producto basico como alimento bebida etc.
        #region Categoria_Producto
        // esta es la categoria del producto basico como alimento bebida etc.
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

        //esta es la categoria del producto agregada por la empresa
        #region Categoria_ProductoCat
        //esta es la categoria del producto agregada por la empresa
        [HttpGet("catproducto/{idEmpresa}")]
        public async Task<ActionResult> ObtenercategoriasProductosdeEmpresa(int idEmpresa)
        {
            var response = new Response<List<CatProducto>>();
            try
            {
                response = await this.catProducto.ObtenerCategoriaProductoPorEmpresa(idEmpresa);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
                return StatusCode(500, response);
            }
            return Ok(response);
        }

        //obtiene las categorias de la empresa que esten disponibles para menu
        [HttpGet("catproducto/{idEmpresa}/menu")]
        public async Task<ActionResult> ObtenercategoriasProductosdeEmpresaMenu(int idEmpresa)
        {
            var response = new Response<List<CatProducto>>();
            try
            {
                response = await this.catProducto.ObtenerCategoriaProductoPorEmpresaMenu(idEmpresa);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
                return StatusCode(500, response);
            }
            return Ok(response);
        }

        //activa o desactiva la categoria del producto interna de la empresa
        [HttpGet("catproducto/{idCat}/activar")]
        public async Task<ActionResult> ActivarcategoriasProductos(int idCat,[FromQuery]Boolean activar)
        {
            var response = new ResponseBase();
            try
            {
                response = await this.catProducto.ActivarCategoria(idCat, activar);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
                return StatusCode(500, response);
            }
            return Ok(response);
        }

        [HttpPost("catproducto")]
        public async Task<ActionResult> insertarcategoriasProductosdeEmpresa([FromBody] CatProducto cp)
        {
            var response = new ResponseBase();
            try
            {
                response = await this.catProducto.InsertarCategoriaProducto(cp);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
                return StatusCode(500, response);
            }
            return Ok(response);
        }

        [HttpDelete("catproducto/{idCatProducto}")]
        public async Task<ActionResult>EliminarCatProducto(int idCatProducto)
        {
            var response = new ResponseBase();
            try
            {
                response = await this.catProducto.EliminarCatProducto(idCatProducto);
            }
            catch(Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
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
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return Ok(response);
        }

        #endregion

        #region Disponibilida_Domicilio

        [HttpPost("disponibilidad/domicilio")]
        public async Task<ActionResult> InsertarDisponibilidadD([FromBody]DisponibilidadDomicilio dm)
        {
            var response = new ResponseBase();
            try
            {
                response = await this._disponibilidadD.InsertarDisponibilidadMunicipio(dm);
            }
            catch(Exception ex)
            {
                response.success = true;
                response.message = ex.Message;
            }
            return Ok(response);
        }

        [HttpGet("disponibilidad/domicilio/{idEmpresa}")]
        public async Task<ActionResult> ObtenerDisponibilidadDomicilio(int idEmpresa)
        {
            var response = new Response<List<DisponibilidadDomicilio>>();
            try
            {
                response = await this._disponibilidadD.ObtenerDisponibilidad(idEmpresa);
            }
            catch(Exception ex)
            {
                response.success = true;
                response.message = ex.Message;
            }
            return Ok(response);
        }

        #endregion
    }
}
