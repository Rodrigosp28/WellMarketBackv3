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
    public class EmpresaController : ControllerBase
    {
        private readonly IEmpresa empresa;

        public EmpresaController(IEmpresa empresa)
        {
            this.empresa = empresa;
        }

        [HttpPut("abrir")]
        public async Task<ActionResult>abrirEmpresa([FromBody]Empresa empresa)
        {
            var response = new ResponseBase();
            try
            {
                response = await this.empresa.cambiarestadoEmpresa(empresa);
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
