using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WellMarket.Entities;
using WellMarket.Entities.Request;
using WellMarket.Entities.Responses;
using WellMarket.Helpers;
using WellMarket.Responses;

namespace WellMarket.Repository
{
    public interface IVentas
    {
        Task<ResponseBase> InsertarVenta(VentaRequest vr);
        Task<ResponseBase> IniciarTicket(InsertTicket it);
        Task<ResponseBase> CerrarTicket(CerrarTicket ct);
        Task<ResponseBase> InsertarVentaTicket(Venta v);
        Task<Response<TicketVentaResponse>> ObtenerVentaTicket(int idTicket);
        Task<ResponseBase> CancelarTicket(CerrarTicket ct);
    }
    public class VentasRepository : IVentas
    {
        private readonly IConnection con;

        public VentasRepository(IConnection con)
        {
            this.con = con;
        }

        public async Task<ResponseBase> CancelarTicket(CerrarTicket ct)
        {
            var response = new ResponseBase();
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using (var command = new SqlCommand("Reporte.spCancelarTicket", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idTicket", ct.idTicket);
                        command.Parameters.AddWithValue("@horaSalida", ct.horaSalida);
                        command.Parameters.AddWithValue("@total", ct.total);
                        command.Parameters.AddWithValue("@descripcion", ct.descripcion);
                        command.Parameters.AddWithValue("@comentario", ct.comentario);
                        command.Parameters.AddWithValue("@idMesa", ct.idMesa);
                        connection.Open();
                        var result = await command.ExecuteNonQueryAsync();
                        if (result > 2)
                        {
                            response.success = true;
                            response.message = "Ticket Cerrado Correctamente";
                            response.id = ct.idTicket;
                        }
                        else
                        {
                            response.success = false;
                            response.message = "Error";
                            response.id = ct.idTicket;
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

        public async Task<ResponseBase> CerrarTicket(CerrarTicket ct)
        {
            var response = new ResponseBase();
            try
            {
                using(var connection = new SqlConnection(con.getConnection()))
                {
                    using(var command = new SqlCommand("Reporte.spCerrarTicket", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idTicket",ct.idTicket);
                        command.Parameters.AddWithValue("@horaSalida",ct.horaSalida);
                        command.Parameters.AddWithValue("@total",ct.total);
                        command.Parameters.AddWithValue("@descripcion",ct.descripcion);
                        command.Parameters.AddWithValue("@comentario",ct.comentario);
                        command.Parameters.AddWithValue("@pago",ct.pago);
                        command.Parameters.AddWithValue("@cambio",ct.cambio);
                        command.Parameters.AddWithValue("@idMesa",ct.idMesa);
                        connection.Open();
                        var result = await command.ExecuteNonQueryAsync();
                        if (result > 2)
                        {
                            response.success = true;
                            response.message = "Ticket Cerrado Correctamente";
                            response.id = ct.idTicket;
                        }
                        else
                        {
                            response.success = false;
                            response.message = "Error";
                            response.id = ct.idTicket;
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

        public async Task<ResponseBase> IniciarTicket(InsertTicket it)
        {
            var response = new ResponseBase();
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using (var command = new SqlCommand("Reporte.spInsertarTicket", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idTicket", 0);
                        command.Parameters.AddWithValue("@fecha",it.fecha);
                        command.Parameters.AddWithValue("@horaEntrada",it.horaEntrada);
                        command.Parameters.AddWithValue("@idUsuario",it.idUsuario);
                        command.Parameters.AddWithValue("@idEmpresa",it.idEmpresa);
                        command.Parameters.AddWithValue("@idMesa",it.idMesa);
                        command.Parameters["@idTicket"].Direction = ParameterDirection.Output;
                        connection.Open();
                        var result = await command.ExecuteNonQueryAsync();
                        if (result > 2)
                        {
                            response.success = true;
                            response.message = "Ticket Insertado Correctamente";
                            response.id = Convert.ToInt32(command.Parameters["@idTicket"].Value);
                        }
                        else
                        {
                            response.success = false;
                            response.message = "Error";
                            
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

        public async Task<ResponseBase> InsertarVenta(VentaRequest vr)
        {
            var response = new ResponseBase();
            List<string> insertVenta = new List<string>();
            StringBuilder insercmd = new StringBuilder();
            var encabezado = "DECLARE @idTicket INT; BEGIN TRANSACTION venta BEGIN TRY ";
            var variable = " SET @idTicket = SCOPE_IDENTITY(); ";
            var encabezadoVenta = " INSERT INTO Reporte.venta(idTicket, idProducto, cantidad, total) VALUES ";
            var pie = " COMMIT TRANSACTION END TRY BEGIN CATCH ROLLBACK TRANSACTION venta RAISERROR('ERROR', 16, 1) END CATCH";

            try
            {
                var insertTicket = " INSERT INTO Reporte.ticket(fecha, hora, total, descripcion, comentario,idEstatus, idUsuario, idEmpresa) VALUES"
                                    + string.Format("( convert(date,'{0}',105), convert(time,'{1}',105) , '{2}', '{3}', '{4}', '{5}', '{6}' , '{7}' ) ",
                                     vr.ticket.fecha
                                    , vr.ticket.hora
                                    , vr.ticket.total
                                    , vr.ticket.descripcion
                                    , vr.ticket.comentario
                                    , vr.ticket.idEstatus
                                    , vr.ticket.idUsuario
                                    , vr.ticket.idEmpresa);
                foreach (Venta venta in vr.productos)
                {
                    insertVenta.Add(string.Format(" ( {0} ,'{1}' ,'{2}' ,'{3}' )",
                                     "@idTicket"
                                    , venta.idProducto
                                    , venta.cantidad
                                    , venta.total));
                }

                insercmd.Append(encabezado);
                insercmd.Append(insertTicket);
                insercmd.Append(variable);
                insercmd.Append(encabezadoVenta);
                insercmd.Append(string.Join(",", insertVenta));
                insercmd.Append(pie);
                
                using(var connection = new SqlConnection(con.getConnection()))
                {
                    using(var command = new SqlCommand(insercmd.ToString(), connection))
                    {
                        command.CommandType = CommandType.Text;
                        connection.Open();
                        var result = await command.ExecuteNonQueryAsync();
                        if (result > 1)
                        {
                            response.success = true;
                            response.message = "Venta Ingresada Correctamente";
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

        public async Task<ResponseBase> InsertarVentaTicket(Venta v)
        {
            var response = new ResponseBase();
            try
            {
                using(var connection = new SqlConnection(con.getConnection()))
                {
                    using(var command = new SqlCommand("Reporte.spInsertarVentaTicket", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idVenta", 0);
                        command.Parameters.AddWithValue("@idTicket", v.idTicket);
                        command.Parameters.AddWithValue("@idProducto", v.idProducto);
                        command.Parameters.AddWithValue("@cantidad", v.cantidad);
                        command.Parameters.AddWithValue("@total", v.total);
                        command.Parameters["@idVenta"].Direction = ParameterDirection.Output;
                        connection.Open();
                        var result = await command.ExecuteNonQueryAsync();
                        if (result > 0)
                        {
                            response.success = true;
                            response.message = "Venta Insertada Correctamente";
                            response.id = Convert.ToInt32(command.Parameters["@idVenta"].Value);
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

        public async Task<Response<TicketVentaResponse>> ObtenerVentaTicket(int idTicket)
        {
            var response = new Response<TicketVentaResponse>();
            var ticket = new TicketVentaResponse();
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using (var command = new SqlCommand("Reporte.spObtenerProductosPorTicket", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idTicket", idTicket);
                        connection.Open();
                        using(var reader = await command.ExecuteReaderAsync())
                        {
                            var list = new List<Venta>();
                            var mesa = new Mesa();
                            var ticketD = new Ticket();
                            while (reader.Read())
                            {
                                var ventar = new Venta();
                                mesa.idMesa = reader.GetInt32("idMesa");
                                mesa.nombre = reader.GetString("nombreMesa");
                                mesa.descripcion = reader.GetString("descripcionMesa");
                                ticketD.fecha = reader.GetString("fecha");
                                ticketD.horaEntrada = reader.GetString("horaEntrada");
                                ticketD.activo = reader.GetBoolean("activoTicket");
                                ventar.idVenta = reader.GetInt32("idVenta");
                                ventar.idTicket = reader.GetInt32("idTicket");
                                ventar.idProducto = reader.GetInt32("idProducto");
                                ventar.producto = new Producto
                                {
                                    idProducto=reader.GetInt32("idProducto"),
                                    nombre = reader.GetString("nombre"),
                                    descripcion=reader.GetString("descripcion"),
                                    precio= reader.GetDouble("precio"),
                                    idEmpresa = reader.GetInt32("idEmpresa"),
                                    idCategoria=reader.GetInt32("idCategoria")
                                };
                                ventar.cantidad = reader.GetInt32("cantidad");
                                ventar.total = reader.GetDouble("total");
                                list.Add(ventar);
                            }
                            ticket.venta = list;
                            ticket.mesa = mesa;
                            ticket.ticket = ticketD;
                            response.success = true;
                            response.message = "Datos Obtenidos Correctamente";
                            response.Data = ticket;
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
