using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using WellMarket.Entities;
using WellMarket.Helpers;
using WellMarket.Responses;

namespace WellMarket.Repository
{
    public interface ICategoriaP
    {
        Task<Response<List<Categoria_Producto>>> ObtenerCategoriaP();
    }
    public class CategoriaPRepository : ICategoriaP
    {
        private readonly IConnection con;
        public CategoriaPRepository(IConnection con)
        {
            this.con = con;
        }
        public async Task<Response<List<Categoria_Producto>>> ObtenerCategoriaP()
        {
            var response = new Response<List<Categoria_Producto>>();
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using (var command = new SqlCommand("Catalogos.spObtenerCategoriaProducto",connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        connection.Open();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            var list = new List<Categoria_Producto>();
                            while (reader.Read())
                            {
                                list.Add(new Categoria_Producto
                                {
                                    idCategoria = reader.GetInt32("idCategoria"),
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
            catch(Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return response;
        }
    }
}
