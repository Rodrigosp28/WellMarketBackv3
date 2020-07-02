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
    public interface IDisponibleP
    {
        Task<Response<List<disponible_producto>>> ObtenerDisponibleProducto();
    }
    public class DisponiblePRepository : IDisponibleP
    {
        private readonly IConnection con;
        public DisponiblePRepository(IConnection con)
        {
            this.con = con;
        }
        public async Task<Response<List<disponible_producto>>> ObtenerDisponibleProducto()
        {
            var response = new Response<List<disponible_producto>>();
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using (var command = new SqlCommand("Catalogos.spObtenerDisponibleProducto",connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        connection.Open();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            var list = new List<disponible_producto>();
                            while (reader.Read())
                            {
                                list.Add(new disponible_producto
                                {
                                    idDisponible = reader.GetInt32("idDisponible"),
                                    descripcion = reader.GetString("descripcion")
                                });
                            }

                            response.success = true;
                            response.message = "Datos Obtenidos Correctamente";
                            response.Data = list;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return response;
        }
    }
}
