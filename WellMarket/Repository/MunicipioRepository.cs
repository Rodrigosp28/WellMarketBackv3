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
    public interface IMunicipio
    {
        Task<Response<List<Municipio>>> ObtenerMunicipios();
    }
    public class MunicipioRepository:IMunicipio
    {
        private readonly IConnection con;
        public MunicipioRepository(IConnection con)
        {
            this.con = con;
        }

        public async Task<Response<List<Municipio>>>ObtenerMunicipios()
        {
            var response = new Response<List<Municipio>>();
            try
            {
                using(var connection = new SqlConnection(con.getConnection()))
                {
                    using(var command = new SqlCommand("Catalogos.spObtenerMunicipio", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();
                        using(var reader = await command.ExecuteReaderAsync())
                        {
                            var list = new List<Municipio>();
                            while (reader.Read())
                            {
                                list.Add(new Municipio
                                {
                                    idMunicipio = reader.GetInt32("idMunicipio"),
                                    descripcionMunicipio = reader.GetString("nombre")
                                });
                            }

                            response.success = true;
                            response.message = "Datos Obtenidos Correctamente";
                            response.Data = list;
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
