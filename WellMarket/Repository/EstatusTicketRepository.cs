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
    public interface IEstatusT
    {
        Task<Response<List<EstatusTicket>>>ObtenerEstatus();
    }
    public class EstatusTicketRepository:IEstatusT
    {
        private readonly IConnection con;

        public EstatusTicketRepository(IConnection con)
        {
            this.con = con;
        }

        public async Task<Response<List<EstatusTicket>>> ObtenerEstatus()
        {
            var response = new Response<List<EstatusTicket>>();
            try
            {
                using(var connection = new SqlConnection(con.getConnection()))
                {
                    using(var command = new SqlCommand("Catalogos.spObtenerEstatusTicket", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        connection.Open();
                        using(var reader = await command.ExecuteReaderAsync())
                        {
                            var list = new List<EstatusTicket>();
                            while (reader.Read())
                            {
                                list.Add(new EstatusTicket
                                {
                                    idEstatus = reader.GetInt32("idEstatus"),
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
