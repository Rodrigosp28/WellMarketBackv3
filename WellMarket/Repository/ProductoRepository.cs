using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WellMarket.Entities;
using WellMarket.Entities.Responses;
using WellMarket.Helpers;
using WellMarket.Responses;

namespace WellMarket.Repository
{
    public interface IProducto
    {
        Task<ResponseBase> InsertarProducto(Producto producto);
        Task<Response<List<Producto>>> ObtenerProductos();
        Task<Response<List<Producto>>> ObtenerProductoPorEmpresa(int idEmpresa);
        Task<ResponseBase> InsertarImagenProducto(Imagenes_Producto ip);
        Task<ResponseBase> ActualizarProducto(int id, Producto producto);
        Task<Response<Producto>> ObtenerProductoPorId(int id);
        Task<Response<List<Imagenes_Producto>>> ObtenerImagenesPorProducto(int id);
        Task<ResponseBase> EliminarImagenProducto(int idEmpresa, string imagen);
        Task<Response<List<Producto>>> ObtenerProductoDisponiblePorEmpresa(int idEmpresa);
        Task<Response<List<PMasVendidos>>> ObtenerCincoProductosMasVendidos(int idEmpresa, string fecha);

    }
    public class ProductoRepository: IProducto
    {
        private readonly IConnection con;

        public ProductoRepository(IConnection con)
        {
            this.con = con;
        }

        public async Task<ResponseBase> ActualizarProducto(int id, Producto producto)
        {
            var response = new ResponseBase();
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using (var command = new SqlCommand("Reporte.spActualizarProducto ", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idProducto", id);
                        command.Parameters.AddWithValue("@descripcion", producto.descripcion);
                        command.Parameters.AddWithValue("@precio", producto.precio);
                        command.Parameters.AddWithValue("@idDisponible", producto.idDisponible);
                        command.Parameters.AddWithValue("@idCategoria", producto.idCategoria);
                        connection.Open();
                        var result = await command.ExecuteNonQueryAsync();
                        if (result > 0)
                        {
                            response.id = id;
                            response.success = true;
                            response.message = "Datos Actualizados Correctamente";

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

        public async Task<ResponseBase> EliminarImagenProducto(int idEmpresa, string imagen)
        {
            var response = new ResponseBase();
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using (var command = new SqlCommand("Reporte.spEliminarImagenProducto", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idEmpresa", idEmpresa);
                        command.Parameters.AddWithValue("@imagen", imagen);
                        connection.Open();
                        var result = await command.ExecuteNonQueryAsync();
                        if (result > 0)
                        {
                            response.id = idEmpresa;
                            response.success = true;
                            response.message = "Imagen Eliminada Correctamente";
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

        public async Task<ResponseBase> InsertarImagenProducto(Imagenes_Producto ip)
        {
            var response = new ResponseBase();
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using (var command = new SqlCommand("Reporte.spInsertarImagenProducto", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idImagen", 0);
                        command.Parameters.AddWithValue("@idEmpresa", ip.idEmpresa);
                        command.Parameters.AddWithValue("@imagen", ip.imagen);
                        command.Parameters.AddWithValue("@url", ip.url);
                        command.Parameters.AddWithValue("@idProducto", ip.idProducto);
                        command.Parameters["@idImagen"].Direction = ParameterDirection.Output;
                        connection.Open();
                        var result = await command.ExecuteNonQueryAsync();
                        if (result > 0)
                        {
                            response.id = Convert.ToInt32(command.Parameters["@idImagen"].Value);
                            response.success = true;
                            response.message = "Datos Insertados Correctamente";
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

        public async Task<ResponseBase> InsertarProducto(Producto producto)
        {
            var response = new ResponseBase();
            try
            {
                using(var connection = new SqlConnection(con.getConnection()))
                {
                    using(var command = new SqlCommand("Reporte.spInsertarProducto", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idProducto", 0);
                        command.Parameters.AddWithValue("@nombre", producto.nombre);
                        command.Parameters.AddWithValue("@descripcion", producto.descripcion);
                        command.Parameters.AddWithValue("@precio", producto.precio);
                        command.Parameters.AddWithValue("@idDisponible", producto.idDisponible);
                        command.Parameters.AddWithValue("@idEmpresa", producto.idEmpresa);
                        command.Parameters.AddWithValue("@idCategoria", producto.idCategoria);
                        command.Parameters["@idProducto"].Direction = ParameterDirection.Output;
                        connection.Open();
                        var result = await command.ExecuteNonQueryAsync();
                        if (result > 0)
                        {
                            response.id = Convert.ToInt32(command.Parameters["@idProducto"].Value);
                            response.success = true;
                            response.message = "Datos Insertados Correctamente";
                            
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

        public async Task<Response<List<PMasVendidos>>> ObtenerCincoProductosMasVendidos(int idEmpresa, string fecha)
        {
            var response = new Response<List<PMasVendidos>>();
            try
            {
                using(var connection = new SqlConnection(con.getConnection()))
                {
                    using(var command = new SqlCommand("Reporte.spArticulosMasVendidos", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idEmpresa", idEmpresa);
                        command.Parameters.AddWithValue("@fecha", fecha);
                        connection.Open();
                        using(var reader = await command.ExecuteReaderAsync())
                        {
                            var list = new List<PMasVendidos>();
                            while (reader.Read())
                            {
                                list.Add(new PMasVendidos
                                {
                                    nombre = reader.GetString("nombre"),
                                    cantidad = reader.GetInt32("cantidad"),
                                    precio = reader.GetDouble("precio")
                                });
                            }
                            response.success = true;
                            response.message = "Datos obtenidos Correctamente";
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

        public async Task<Response<List<Imagenes_Producto>>> ObtenerImagenesPorProducto(int id)
        {
            var response = new Response<List<Imagenes_Producto>>();
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using (var command = new SqlCommand("Reporte.spObtenerImagenesProductoPorIdProducto", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idProducto", id);
                        connection.Open();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            var list = new List<Imagenes_Producto>();
                            while (reader.Read())
                            {
                                list.Add(new Imagenes_Producto
                                {
                                    idImagen = reader.GetInt32("idImagen"),
                                    idEmpresa = reader.GetInt32("idEmpresa"),
                                    imagen = reader.GetString("imagen"),
                                    url = reader.GetString("url"),
                                    idProducto = reader.GetInt32("idProducto")
                                }); ;
                            }
                            response.success = true;
                            response.Data = list;
                            response.message = "Datos Obtenidos Correctamente";
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

        public async Task<Response<List<Producto>>> ObtenerProductoDisponiblePorEmpresa(int idEmpresa)
        {
            var response = new Response<List<Producto>>();
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using (var command = new SqlCommand("Reporte.spObtenerProductosDisponiblePorIdEmpresa", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idEmpresa", idEmpresa);
                        connection.Open();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            var list = new List<Producto>();
                            while (reader.Read())
                            {
                                var responseT = new Response<List<Imagenes_Producto>>();
                                var producto = new Producto();
                                producto.idProducto = reader.GetInt32("idProducto");
                                producto.nombre = reader.GetString("nombre");
                                producto.descripcion = reader.GetString("descripcion");
                                producto.precio = reader.GetDouble("precio");
                                producto.idDisponible = reader.GetInt32("idDisponible");
                                producto.idEmpresa = reader.GetInt32("idEmpresa");
                                producto.disponible = new disponible_producto
                                {
                                    idDisponible = reader.GetInt32("idDisponible"),
                                    descripcion = reader.GetString("disponible")
                                };
                                producto.idCategoria = reader.GetInt32("idCategoria");
                                producto.categoria = new Categoria_Producto
                                {
                                    idCategoria = reader.GetInt32("idCategoria"),
                                    descripcion = reader.GetString("categoria")
                                };
                                responseT = await this.ObtenerImagenesPorProducto(producto.idProducto);
                                producto.imagenes = responseT.Data;
                                list.Add(producto);
                            }
                            response.success = true;
                            response.Data = list;
                            response.message = "Datos Obtenidos Correctamente";
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

        public async Task<Response<List<Producto>>> ObtenerProductoPorEmpresa(int idEmpresa)
        {
            var response = new Response<List<Producto>>();
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using (var command = new SqlCommand("Reporte.spObtenerProductoPorEmpresa", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idEmpresa", idEmpresa);
                        connection.Open();
                        using(var reader = await command.ExecuteReaderAsync())
                        {
                            var list = new List<Producto>();
                            while (reader.Read())
                            {
                                list.Add(new Producto
                                {
                                    idProducto = reader.GetInt32("idProducto"),
                                    nombre = reader.GetString("nombre"),
                                    descripcion = reader.GetString("descripcion"),
                                    precio = reader.GetDouble("precio"),
                                    idDisponible = reader.GetInt32("idDisponible"),
                                    disponible = new disponible_producto
                                    {
                                        idDisponible=reader.GetInt32("idDisponible"),
                                        descripcion= reader.GetString("disponible")
                                    },
                                    idEmpresa = reader.GetInt32("idEmpresa"),
                                    idCategoria = reader.GetInt32("idCategoria"),
                                    categoria = new Categoria_Producto
                                    {
                                        idCategoria = reader.GetInt32("idCategoria"),
                                        descripcion = reader.GetString("categoria")
                                    }
                                }); 
                            }
                            response.success = true;
                            response.Data = list;
                            response.message = "Datos Obtenidos Correctamente";
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

        public async Task<Response<Producto>> ObtenerProductoPorId(int id)
        {
            var response = new Response<Producto>();
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using (var command = new SqlCommand("Reporte.spObtenerProductoPorId", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idProducto", id);
                        connection.Open();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            Producto producto = new Producto();
                            while (reader.Read())
                            {
                                producto = new Producto
                                {
                                    idProducto = reader.GetInt32("idProducto"),
                                    nombre = reader.GetString("nombre"),
                                    descripcion = reader.GetString("descripcion"),
                                    precio = reader.GetDouble("precio"),
                                    idDisponible = reader.GetInt32("idDisponible"),
                                    disponible = new disponible_producto
                                    {
                                        idDisponible = reader.GetInt32("idDisponible"),
                                        descripcion = reader.GetString("disponible")
                                    },
                                    idEmpresa = reader.GetInt32("idEmpresa"),
                                    idCategoria = reader.GetInt32("idCategoria"),
                                    categoria = new Categoria_Producto
                                    {
                                        idCategoria = reader.GetInt32("idCategoria"),
                                        descripcion = reader.GetString("categoria")
                                    }
                                };
                            }
                            response.success = true;
                            response.Data = producto;
                            response.message = "Datos Obtenidos Correctamente";
                            response.id = id;
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

        public async Task<Response<List<Producto>>> ObtenerProductos()
        {
            var response = new Response<List<Producto>>();
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using (var command = new SqlCommand("Reporte.spObtenerProductos", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        connection.Open();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            var list = new List<Producto>();
                            while (reader.Read())
                            {
                                list.Add(new Producto
                                {
                                    idProducto = reader.GetInt32("idProducto"),
                                    nombre = reader.GetString("nombre"),
                                    descripcion = reader.GetString("descripcion"),
                                    precio = reader.GetDouble("precio"),
                                    idDisponible = reader.GetInt32("idDisponible"),
                                    disponible = new disponible_producto
                                    {
                                        idDisponible = reader.GetInt32("idDisponible"),
                                        descripcion = reader.GetString("disponible")
                                    },
                                    idEmpresa = reader.GetInt32("idEmpresa"),
                                    idCategoria = reader.GetInt32("idCategoria"),
                                    categoria = new Categoria_Producto
                                    {
                                        idCategoria = reader.GetInt32("idCategoria"),
                                        descripcion = reader.GetString("categoria")
                                    }
                                });
                            }
                            response.success = true;
                            response.Data = list;
                            response.message = "Datos Obtenidos Correctamente";
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
