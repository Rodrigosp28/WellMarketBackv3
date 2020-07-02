using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WellMarket.Entities;
using WellMarket.Helpers;
using WellMarket.Responses;

namespace WellMarket.Repository
{
    public interface IEmpresa
    {
        Task<ResponseBase> cambiarestadoEmpresa(Empresa empresa);
    }
    public class EmpresaRepository:IEmpresa
    {
        private readonly IConnection con;

        public EmpresaRepository(IConnection con)
        {
            this.con = con;
        }

        public async Task<ResponseBase> cambiarestadoEmpresa(Empresa empresa)
        {
            var response = new ResponseBase();
            try
            {
                using(var connection = new SqlConnection(con.getConnection()))
                {
                    using(var command = new SqlCommand("Seguridad.spAbrirEmpresa", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@estado", empresa.abierto);
                        command.Parameters.AddWithValue("@idEmpresa", empresa.idEmpresa);
                        connection.Open();
                        var result = await command.ExecuteNonQueryAsync();
                        if (result > 0)
                        {
                            response.id = empresa.idEmpresa;
                            response.success = true;
                            response.message = "Datos Actualizados Correctamente";
                        }
                    }
                }
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
