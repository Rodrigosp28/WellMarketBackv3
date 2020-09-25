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
    public interface ICatProducto
    {
        Task<ResponseBase> InsertarCategoriaProducto(CatProducto cp);
        Task<Response<List<CatProducto>>> ObtenerCategoriaProductoPorEmpresa(int idEmpresa);
    }
    public class CategoriaProductoRespository : ICatProducto
    {
        private readonly IConnection con;

        public CategoriaProductoRespository(IConnection con)
        {
            this.con = con;
        }
        public async Task<ResponseBase> InsertarCategoriaProducto(CatProducto cp)
        {
            var response = new ResponseBase();
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using(var command = new SqlCommand("Catalogos.spInsertarCategoriaProducto",connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idCatProducto", 0);
                        command.Parameters.AddWithValue("@nombre",cp.nombre);
                        command.Parameters.AddWithValue("@descripcion",cp.descripcion);
                        command.Parameters.AddWithValue("@idEmpresa",cp.idEmpresa);
                        command.Parameters["@idCatProducto"].Direction = ParameterDirection.Output;
                        connection.Open();
                        var result = await command.ExecuteNonQueryAsync();
                        if(result > 0)
                        {
                            response.success = true;
                            response.message = "Datos Insertados Correctamente";
                            response.id = Convert.ToInt32(command.Parameters["@idCatProducto"].Value);
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

        public async Task<Response<List<CatProducto>>> ObtenerCategoriaProductoPorEmpresa(int idEmpresa)
        {
            var response = new Response<List<CatProducto>>();
            try
            {
                using(var connection = new SqlConnection(con.getConnection()))
                {
                    using(var command = new SqlCommand("Catalogos.spObtenerCatalogoProductosPorEmpresa",connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idEmpresa", idEmpresa);
                        connection.Open();
                        using(var reader = await command.ExecuteReaderAsync())
                        {
                            var list = new List<CatProducto>();
                            while(reader.Read())
                            {
                                list.Add(new CatProducto
                                {
                                    idCatProducto = reader.GetInt32("idCatProducto"),
                                    nombre = reader.GetString("nombre"),
                                    descripcion = reader.GetString("descripcion"),
                                    idEmpresa = reader.GetInt32("idEmpresa"),
                                    habilitado = reader.GetBoolean("habilitado")
                                });
                            }
                            response.success = true;
                            response.message = "datos obtenidos correctamente";
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
