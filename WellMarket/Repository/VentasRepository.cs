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
        Task<ResponseBase> EliminarVentaTicket(int idVenta);
        Task<Ticket> ObtenerTicketId(int idTicket);
        Task<Response<Ticket>> ObtenerTicketIdD(int idTicket);
        Task<Mesa> ObtenerMesaIdTicket(int idTicket);
        Task<Response<List<Ticket>>> ObtenerTicketsPorIdEmpresa(int idEmpresa, string fecha);
        Task<Response<List<Ticket>>> ObtenerTicketsPorMes(int idEmpresa, int mes);
        Task<Response<List<Ticket>>> ObtenerTicketsPorIntervalo(IntervaloTicket i);
        Task<ResponseBase> ActualizarVenta(Venta v);
    }
    public class VentasRepository : IVentas
    {
        private readonly IConnection con;

        public VentasRepository(IConnection con)
        {
            this.con = con;
        }

        public async Task<ResponseBase> ActualizarVenta(Venta v)
        {
            var response = new ResponseBase();
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using (var command = new SqlCommand("Reporte.spActualizarVenta", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idVenta", v.idVenta);
                        command.Parameters.AddWithValue("@idProducto", v.idProducto);
                        command.Parameters.AddWithValue("@cantidad", v.cantidad);
                        command.Parameters.AddWithValue("@total", v.total);
                        connection.Open();
                        var result = await command.ExecuteNonQueryAsync();
                        if (result > 0)
                        {
                            response.success = true;
                            response.message = "Venta actualizada Correctamente";
                            response.id = v.idVenta;
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
                        command.Parameters.AddWithValue("@fechaSalida", ct.fechaSalida);
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
                        command.Parameters.AddWithValue("@fechaSalida", ct.fechaSalida);

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

        public async Task<ResponseBase> EliminarVentaTicket(int idVenta)
        {
            var response = new ResponseBase();
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using (var command = new SqlCommand("Reporte.spEliminarProductoVenta", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idVenta", idVenta);
                        connection.Open();
                        var result = await command.ExecuteNonQueryAsync();
                        if (result > 0)
                        {
                            response.success = true;
                            response.message = "Venta Eliminada Correctamente";
                            response.id = idVenta;
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

        public async Task<Mesa> ObtenerMesaIdTicket(int idTicket)
        {
            var mesa = new Mesa();
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using (var command = new SqlCommand("Reporte.spObtenerMesaPorIdTicket", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idTicket", idTicket);
                        connection.Open();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                mesa.idMesa = reader.GetInt32("idMesa");
                                mesa.nombre = reader.GetString("nombre");
                                mesa.descripcion = reader.GetString("descripcion");
                                mesa.idEmpresa = reader.GetInt32("idEmpresa");
                                mesa.ocupado = reader.GetBoolean("ocupado");
                            }
                            return mesa;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return mesa;
            }
        }

        public async Task<Ticket> ObtenerTicketId(int idTicket)
        {
            var ticket = new Ticket();
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using (var command = new SqlCommand("Reporte.spObtenerTicketPorId", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idTicket", idTicket);
                        connection.Open();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                ticket.idTicket = reader.GetInt32("idTicket");
                                ticket.fecha = reader.GetString("fecha");
                                ticket.horaEntrada = reader.GetString("horaEntrada");
                                ticket.idEstatus = reader.GetInt32("idEstatus");
                                ticket.activo = reader.GetBoolean("activo");
                            }
                            return ticket;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return ticket;
            }
        }

        public async Task<Response<Ticket>> ObtenerTicketIdD(int idTicket)
        {
            var response = new Response<Ticket>();
            try
            {
                using(var connection = new SqlConnection(con.getConnection()))
                {
                    using(var command = new SqlCommand("Reporte.spObtenerTicketPorId", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idTicket", idTicket);
                        connection.Open();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            var ticket = new Ticket();
                            while (reader.Read())
                            {
                                ticket.idTicket = reader.GetInt32("idTicket");
                                ticket.fecha = reader.GetString("fecha");
                                ticket.fechaSalida = reader.GetString("fechaSalida");
                                ticket.hora = reader.GetString("hora");
                                ticket.horaEntrada = reader.GetString("horaEntrada");
                                ticket.horaSalida = reader.GetString("horaSalida");
                                ticket.idEstatus = reader.GetInt32("idEstatus");
                                ticket.total = reader.GetDouble("total");
                                ticket.descripcion = reader.GetString("descripcion");
                                ticket.comentario = reader.GetString("comentario");
                                ticket.idEstatus = reader.GetInt32("idEstatus");
                                ticket.nombreEstatus = reader.GetString("nombreEstatus");
                                ticket.idUsuario = reader.GetInt32("idUsuario");
                                ticket.nombreUsuario = reader.GetString("nombreUsuario");
                                ticket.idEmpresa = reader.GetInt32("idEmpresa");
                                ticket.activo = reader.GetBoolean("activo");
                                ticket.pago = reader.GetDouble("pago");
                                ticket.cambio = reader.GetDouble("cambio");
                            }
                            response.success = true;
                            response.message = "datos obtenidos correctamente";
                            response.Data = ticket;
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

        public async Task<Response<List<Ticket>>> ObtenerTicketsPorIdEmpresa(int idEmpresa, string fecha)
        {
            var response = new Response<List<Ticket>>();
            try
            {
                using(var connection = new SqlConnection(con.getConnection()))
                {
                    using(var command = new SqlCommand("Reporte.spObtenerTicketsDia", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idEmpresa", idEmpresa);
                        command.Parameters.AddWithValue("@fecha", fecha);
                        connection.Open();
                        using(var reader = await command.ExecuteReaderAsync())
                        {
                            var list = new List<Ticket>();
                            
                            while (reader.Read())
                            {
                                var ticket = new Ticket();
                                ticket.idTicket = reader.GetInt32("idTicket");
                                ticket.fecha = reader.GetString("fecha");
                                ticket.hora = reader.GetString("hora");
                                ticket.total = reader.GetDouble("total");
                                ticket.descripcion = reader.GetString("descripcion");
                                ticket.comentario = reader.GetString("comentario");
                                ticket.idEstatus = reader.GetInt32("idEstatus");
                                ticket.nombreEstatus = reader.GetString("nombreEstatus");
                                ticket.idUsuario = reader.GetInt32("idUsuario");
                                ticket.activo = reader.GetBoolean("activo");
                                ticket.pago = reader.GetDouble("pago");
                                ticket.cambio = reader.GetDouble("cambio");
                                list.Add(ticket);
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

        public async Task<Response<List<Ticket>>> ObtenerTicketsPorIntervalo(IntervaloTicket i)
        {
            var response = new Response<List<Ticket>>();
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using (var command = new SqlCommand("Reporte.spObtenerTicketsIntervalo", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idEmpresa", i.idEmpresa);
                        command.Parameters.AddWithValue("@fechaInicio", i.fechaInicio);
                        command.Parameters.AddWithValue("@fechaFinal", i.fechaFinal);
                        command.Parameters.AddWithValue("@horaInicio", i.horaInicio);
                        command.Parameters.AddWithValue("@horaFinal", i.horaFinal);
                        connection.Open();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            var list = new List<Ticket>();

                            while (reader.Read())
                            {
                                var ticket = new Ticket();
                                ticket.idTicket = reader.GetInt32("idTicket");
                                ticket.fecha = reader.GetString("fecha");
                                ticket.hora = reader.GetString("hora");
                                ticket.total = reader.GetDouble("total");
                                ticket.descripcion = reader.GetString("descripcion");
                                ticket.comentario = reader.GetString("comentario");
                                ticket.idEstatus = reader.GetInt32("idEstatus");
                                ticket.nombreEstatus = reader.GetString("nombreEstatus");
                                ticket.idUsuario = reader.GetInt32("idUsuario");
                                ticket.activo = reader.GetBoolean("activo");
                                ticket.pago = reader.GetDouble("pago");
                                ticket.cambio = reader.GetDouble("cambio");
                                list.Add(ticket);
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

        public async Task<Response<List<Ticket>>> ObtenerTicketsPorMes(int idEmpresa, int mes)
        {
            var response = new Response<List<Ticket>>();
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using (var command = new SqlCommand("Reporte.spObtenerTicketsMes", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idEmpresa", idEmpresa);
                        command.Parameters.AddWithValue("@mes", mes);
                        connection.Open();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            var list = new List<Ticket>();

                            while (reader.Read())
                            {
                                var ticket = new Ticket();
                                ticket.idTicket = reader.GetInt32("idTicket");
                                ticket.fecha = reader.GetString("fecha");
                                ticket.hora = reader.GetString("hora");
                                ticket.total = reader.GetDouble("total");
                                ticket.descripcion = reader.GetString("descripcion");
                                ticket.comentario = reader.GetString("comentario");
                                ticket.idEstatus = reader.GetInt32("idEstatus");
                                ticket.nombreEstatus = reader.GetString("nombreEstatus");
                                ticket.idUsuario = reader.GetInt32("idUsuario");
                                ticket.activo = reader.GetBoolean("activo");
                                ticket.pago = reader.GetDouble("pago");
                                ticket.cambio = reader.GetDouble("cambio");
                                list.Add(ticket);
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
                            while (reader.Read())
                            {
                                var ventar = new Venta();
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
                                    idCategoria=reader.GetInt32("idCategoria"),
                                    paraCocina=reader.GetBoolean("paraCocina")
                                };
                                ventar.cantidad = reader.GetInt32("cantidad");
                                ventar.enCocina = reader.GetBoolean("enCocina");
                                ventar.total = reader.GetDouble("total");
                                list.Add(ventar);
                            }
                            ticket.venta = list;
                            ticket.mesa = await this.ObtenerMesaIdTicket(idTicket);
                            ticket.ticket = await this.ObtenerTicketId(idTicket);
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
