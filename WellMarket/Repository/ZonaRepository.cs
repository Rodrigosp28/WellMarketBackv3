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
    public interface IZona
    {
        Task<Response<List<Zona>>> ObtenerZonasPorMunicipio(int idMunicipio);
    }
    public class ZonaRepository:IZona
    {
        private readonly IConnection con;
        public ZonaRepository(IConnection con)
        {
            this.con = con;
        }

        public async Task<Response<List<Zona>>> ObtenerZonasPorMunicipio(int idMunicipio)
        {
            var response = new Response<List<Zona>>();
            try
            {
                using(var connection = new SqlConnection(con.getConnection()))
                {
                    using(var command = new SqlCommand("Catalogos.spObtenerZonasPorMunicipio", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idMunicipio", idMunicipio);
                        connection.Open();
                        using(var reader = await command.ExecuteReaderAsync())
                        {
                            var list = new List<Zona>();
                            while (reader.Read())
                            {
                                list.Add(new Zona
                                {
                                    idZona = reader.GetInt32("idZona"),
                                    descripcionZona = reader.GetString("nombre"),
                                    idMunicipio = reader.GetInt32("idMunicipio")
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
