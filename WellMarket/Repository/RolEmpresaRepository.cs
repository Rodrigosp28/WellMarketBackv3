using Microsoft.Extensions.DependencyInjection;
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
    public interface IRolEmpresa
    {
        Task<Response<List<RolEmpresa>>> ObtenerRolEmpresa();
    }
    public class RolEmpresaRepository : IRolEmpresa
    {
        private readonly IConnection con;

        public RolEmpresaRepository(IConnection con)
        {
            this.con = con;
        }
        public async Task<Response<List<RolEmpresa>>> ObtenerRolEmpresa()
        {
            var response = new Response<List<RolEmpresa>>();
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using (var command = new SqlCommand("Catalogos.spObtenerRolEmpresa", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        connection.Open();
                        using(var reader = await command.ExecuteReaderAsync())
                        {
                            var list = new List<RolEmpresa>();
                            while (reader.Read())
                            {
                                list.Add(new RolEmpresa
                                {
                                    idRolEmpresa = reader.GetInt32("idRolEmpresa"),
                                    descripcion = reader.GetString("nombre")
                                });
                            }
                            response.success = true;
                            response.Data = list;
                            response.message = "Datos Obtenidos Correctamente";
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
