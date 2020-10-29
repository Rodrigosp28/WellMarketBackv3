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
    public interface IPromocion
    {
        Task<ResponseBase> InsertarPromocion(Promocion promo);
        Task<ResponseBase> InsertarImagenPromocion(Imagen_promocion imgp);
        Task<ResponseBase> ActualizarPromocion(int id, Promocion promo);
        Task<ResponseBase> EliminarPromocion(int idPromocion);
        Task<ResponseBase> EliminarImagenPromocion(int idImagenPromocion);
        Task<Response<Promocion>> ObtenerPromocionPorId(int id);
        Task<Response<List<Imagen_promocion>>> ObtenerImagenesPorPromocion(int id);
        Task<Response<List<Promocion>>> ObtenerPromocionPorEmpresa(int idEmpresa);
        Task<Response<List<Promocion>>> ObtenerPromocionDisponiblePorMunicipio(int idMunicipio, int pag,int dia);



    }
    public class PromocionRepository:IPromocion
    {
        private readonly IConnection con;

        public PromocionRepository(IConnection con)
        {
            this.con = con;
        }

        public async Task<ResponseBase> InsertarPromocion(Promocion promo)
        {
            var response = new ResponseBase();
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using (var command = new SqlCommand("Catalogos.spInsertarPromocion", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idPromocion", 0);
                        command.Parameters.AddWithValue("@nombre", promo.nombre);
                        command.Parameters.AddWithValue("@descripcion", promo.descripcion);
                        command.Parameters.AddWithValue("@precio", promo.precio);
                        command.Parameters.AddWithValue("@disponible", promo.disponible);
                        command.Parameters.AddWithValue("@diaDisponible", promo.diaDisponible);
                        command.Parameters.AddWithValue("@idEmpresa", promo.idEmpresa);
                        command.Parameters["@idPromocion"].Direction = ParameterDirection.Output;
                        connection.Open();
                        var result = await command.ExecuteNonQueryAsync();
                        if (result > 0)
                        {
                            response.id = Convert.ToInt32(command.Parameters["@idPromocion"].Value);
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

        public async Task<ResponseBase> InsertarImagenPromocion(Imagen_promocion imgp)
        {
            var response = new ResponseBase();
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using (var command = new SqlCommand("Catalogos.spInsertaImagenPromocion", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idImagenPromocion", 0);
                        command.Parameters.AddWithValue("@idEmpresa", imgp.idEmpresa);
                        command.Parameters.AddWithValue("@imagen", imgp.imagen);
                        command.Parameters.AddWithValue("@url", imgp.url);
                        command.Parameters.AddWithValue("@idPromocion", imgp.idPromocion);
                        command.Parameters["@idImagenPromocion"].Direction = ParameterDirection.Output;
                        connection.Open();
                        var result = await command.ExecuteNonQueryAsync();
                        if (result > 0)
                        {
                            response.id = Convert.ToInt32(command.Parameters["@idImagenPromocion"].Value);
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

        public async Task<ResponseBase> ActualizarPromocion(int id, Promocion promo)
        {
            var response = new ResponseBase();
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using (var command = new SqlCommand("Catalogos.spActualizaPromocion", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idPromocion", id);
                        command.Parameters.AddWithValue("@descripcion", promo.descripcion);
                        command.Parameters.AddWithValue("@precio", promo.precio);
                        command.Parameters.AddWithValue("@disponible", promo.disponible);
                        command.Parameters.AddWithValue("@diaDisponible", promo.diaDisponible);
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

        public async Task<ResponseBase> EliminarPromocion(int idPromocion)
        {
            var response = new ResponseBase();
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using (var command = new SqlCommand("Catalogos.spEliminarPromocion", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idPromocion", idPromocion);
                        connection.Open();
                        var result = await command.ExecuteNonQueryAsync();
                        if (result > 0)
                        {
                            response.id = idPromocion;
                            response.success = true;
                            response.message = "Dato eliminado Correctamente";
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

        public async Task<ResponseBase> EliminarImagenPromocion(int idImagenPromocion)
        {
            var response = new ResponseBase();
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using (var command = new SqlCommand("Catalogos.spEliminaImagenPromocion", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idImagenPromocion", idImagenPromocion);
                        connection.Open();
                        var result = await command.ExecuteNonQueryAsync();
                        if (result > 0)
                        {
                            response.id = idImagenPromocion;
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

        //obtene una promocion por id
        public async Task<Response<Promocion>> ObtenerPromocionPorId(int id)
        {
            var response = new Response<Promocion>();
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using (var command = new SqlCommand("Catalogos.spObtenerPromocionPorId", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idPromocion", id);
                        connection.Open();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            Promocion promo = new Promocion();
                            while (reader.Read())
                            {
                                promo.idPromocion = reader.GetInt32("idPromocion");
                                promo.nombre = reader.GetString("nombre");
                                promo.descripcion = reader.GetString("descripcion");
                                promo.precio = reader.GetDouble("precio");
                                promo.disponible = reader.GetBoolean("disponible");
                                promo.diaDisponible = reader.GetInt32("diaDisponible");
                                promo.idEmpresa = reader.GetInt32("idEmpresa");
                            }
                            response.success = true;
                            response.Data = promo;
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
        //obtiene las imagenes de una promocion
        public async Task<Response<List<Imagen_promocion>>> ObtenerImagenesPorPromocion(int id)
        {
            var response = new Response<List<Imagen_promocion>>();
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using (var command = new SqlCommand("Catalogos.spObtenerImagenesPromocionPorId", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idPromocion", id);
                        connection.Open();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            var list = new List<Imagen_promocion>();
                            while (reader.Read())
                            {
                                list.Add(new Imagen_promocion
                                {
                                    idImagenPromocion = reader.GetInt32("idImagenPromocion"),
                                    idEmpresa = reader.GetInt32("idEmpresa"),
                                    imagen = reader.GetString("imagen"),
                                    url = reader.GetString("url"),
                                    idPromocion = reader.GetInt32("idPromocion")
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

        //obtienes todas las promociones de una empresa disponibles y no disponibles
        public async Task<Response<List<Promocion>>> ObtenerPromocionPorEmpresa(int idEmpresa)
        {
            var response = new Response<List<Promocion>>();
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using (var command = new SqlCommand("Catalogos.spObtenerPromocionPorEmpresa", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idEmpresa", idEmpresa);
                        connection.Open();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            var list = new List<Promocion>();
                            while (reader.Read())
                            {
                                list.Add(new Promocion
                                {
                                    idPromocion = reader.GetInt32("idPromocion"),
                                    nombre = reader.GetString("nombre"),
                                    descripcion = reader.GetString("descripcion"),
                                    precio = reader.GetDouble("precio"),
                                    disponible= reader.GetBoolean("disponible"),
                                    diaDisponible=reader.GetInt32("diaDisponible"),
                                    idEmpresa=reader.GetInt32("idEmpresa")
                                    
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

        //obtiene las promociones de la empresa por municipio
        public async Task<Response<List<Promocion>>> ObtenerPromocionDisponiblePorMunicipio(int idMunicipio, int pag, int dia)
        {
            var response = new Response<List<Promocion>>();
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using (var command = new SqlCommand("Catalogos.spObtenerPromocionPorMunicipioPag", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idMunicipio", idMunicipio);
                        command.Parameters.AddWithValue("@paginasTotal", 0);
                        command.Parameters.AddWithValue("@pagina", pag);
                        command.Parameters.AddWithValue("@dia", dia);
                        command.Parameters["@paginasTotal"].Direction = ParameterDirection.Output;
                        connection.Open();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            var list = new List<Promocion>();
                            while (reader.Read())
                            {
                                var responseT = new Response<List<Imagen_promocion>>();
                                var promo = new Promocion();
                                promo.idPromocion = reader.GetInt32("idPromocion");
                                promo.nombre = reader.GetString("nombre");
                                promo.descripcion = reader.GetString("descripcion");
                                promo.precio = reader.GetDouble("precio");
                                promo.disponible = reader.GetBoolean("disponible");
                                promo.diaDisponible = reader.GetInt32("diaDisponible");
                                promo.idEmpresa = reader.GetInt32("idEmpresa");
                                promo.nombreEmpresa = reader.GetString("nombreEmpresa");
                                promo.telefonoEmpresa = reader.GetString("telefonoEmpresa");
                                promo.direccionEmpresa = reader.GetString("direccionEmpresa");
                                responseT = await this.ObtenerImagenesPorPromocion(promo.idPromocion);
                                promo.imagenes = responseT.Data;
                                list.Add(promo);
                            }
                            response.success = true;
                            response.Data = list;
                            response.message = "Datos Obtenidos Correctamente";
                        }
                        response.paginas = Convert.ToInt32(command.Parameters["@paginasTotal"].Value);
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
