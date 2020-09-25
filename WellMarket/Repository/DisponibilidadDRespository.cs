using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using WellMarket.Entities;
using WellMarket.Helpers;
using WellMarket.Responses;

namespace WellMarket.Repository
{
    public interface IDisponibilidadD
    {
        Task<ResponseBase> InsertarDisponibilidadMunicipio(DisponibilidadDomicilio dm);
        Task<Response<List<DisponibilidadDomicilio>>> ObtenerDisponibilidad(int idEmpresa);
    }
    public class DisponibilidadDRespository: IDisponibilidadD
    {
        private readonly IConnection con;
        public DisponibilidadDRespository(IConnection con)
        {
            this.con = con;
        }

        public async Task<ResponseBase> InsertarDisponibilidadMunicipio(DisponibilidadDomicilio dm)
        {
            var response = new ResponseBase();
            try
            {
                using(var connection = new SqlConnection(con.getConnection()))
                {
                    using(var command = new SqlCommand("Catalogos.spInsertarDisponibilidadDomicilio", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idDisponibilidad", 0);
                        command.Parameters.AddWithValue("@idEmpresa", dm.idEmpresa);
                        command.Parameters.AddWithValue("@idZona", dm.idZona);
                        command.Parameters["@idDisponibilidad"].Direction = ParameterDirection.Output;
                        connection.Open();
                        var result = await command.ExecuteNonQueryAsync();
                        if (result > 0)
                        {
                            response.success = true;
                            response.id = Convert.ToInt32(command.Parameters["@idDisponibilidad"].Value);
                            response.message = "datos insertados correctamente";
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

        public async Task<Response<List<DisponibilidadDomicilio>>> ObtenerDisponibilidad(int idEmpresa)
        {
            var response = new Response<List<DisponibilidadDomicilio>>();
            try
            {
                using(var connection = new SqlConnection(con.getConnection()))
                {
                    using(var command = new SqlCommand("Catalogos.spObtenerDisponibilidadDomicilio", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idEmpresa", idEmpresa);
                        connection.Open();
                        using(var reader = await command.ExecuteReaderAsync())
                        {
                            var list = new List<DisponibilidadDomicilio>();
                            while (reader.Read())
                            {
                                list.Add(new DisponibilidadDomicilio
                                {
                                    idDisponibilidad = reader.GetInt32("idDisponibilidad"),
                                    idEmpresa = reader.GetInt32("idEmpresa"),
                                    idZona = reader.GetInt32("idzona"),
                                    nombreZona = reader.GetString("nombreZona"),
                                    nombreMunicipio = reader.GetString("nombreMunicipio")
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
                response.success = true;
                response.message = ex.Message;
            }
            return response;
        }
    }
}
