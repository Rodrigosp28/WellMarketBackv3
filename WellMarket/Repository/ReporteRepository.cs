using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using WellMarket.Entities;
using WellMarket.Entities.Request;
using WellMarket.Helpers;
using WellMarket.Responses;

namespace WellMarket.Repository
{
    public interface IReporte
    {
        Task<Response<Reporte>> ObtenerReportePorDia(int idEmpresa, string fecha);
        Task<Response<Reporte>> ObtenerReportePorMes(int idEmpresa, int mes);
        Task<Response<Reporte>> ObtenerReportePorIntervalo(IntervaloTicket i);
        Task<Response<List<ReporteProducto>>> ObtenerReporteProductoPorDia(int idEmpresa, string fecha);
        Task<Response<List<ReporteProducto>>> ObtenerReporteProductoPorMes(int idEmpresa, int mes);
        Task<Response<List<ReporteProducto>>> ObtenerReporteProductoPorIntervalo(IntervaloTicket i);

    }
    public class ReporteRepository: IReporte
    {
        private readonly IConnection con;
        public ReporteRepository(IConnection con)
        {
            this.con = con;
        }

        public async Task<Response<Reporte>> ObtenerReportePorDia(int idEmpresa, string fecha)
        {
            var response = new Response<Reporte>();
            try
            {
                using(var connection = new SqlConnection(con.getConnection()))
                {
                    using(var command = new SqlCommand("Reporte.spReportePorDia", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idEmpresa", idEmpresa);
                        command.Parameters.AddWithValue("@fecha", fecha);
                        connection.Open();
                        using(var reader = await command.ExecuteReaderAsync())
                        {
                            var reporte = new Reporte();
                            while (reader.Read())
                            {
                                reporte.totalVendido = reader.GetDouble("totalVendido");
                                reporte.articuloMasVendido = reader.GetString("articuloMasVendido");
                                reporte.totalArticulos = reader.GetInt32("totalArticulos");
                                reporte.totalGastos = reader.GetDouble("totalGastos");
                                reporte.totalIngresos = reader.GetDouble("totalIngresos");
                            }
                            response.success = true;
                            response.message = "Datos Obtenidos Correctamente";
                            response.Data = reporte;
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

        public async Task<Response<Reporte>> ObtenerReportePorIntervalo(IntervaloTicket i)
        {
            var response = new Response<Reporte>();
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using (var command = new SqlCommand("Reporte.spReportePorIntervalo ", connection))
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
                            var reporte = new Reporte();
                            while (reader.Read())
                            {
                                reporte.totalVendido = reader.GetDouble("totalVendido");
                                reporte.articuloMasVendido = reader.GetString("articuloMasVendido");
                                reporte.totalArticulos = reader.GetInt32("totalArticulos");
                                reporte.totalGastos = reader.GetDouble("totalGastos");
                                reporte.totalIngresos = reader.GetDouble("totalIngresos");
                            }
                            response.success = true;
                            response.message = "Datos Obtenidos Correctamente";
                            response.Data = reporte;
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

        public async Task<Response<Reporte>> ObtenerReportePorMes(int idEmpresa, int mes)
        {
            var response = new Response<Reporte>();
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using (var command = new SqlCommand("Reporte.spReportePorMes", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idEmpresa", idEmpresa);
                        command.Parameters.AddWithValue("@mes", mes);
                        connection.Open();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            var reporte = new Reporte();
                            while (reader.Read())
                            {
                                reporte.totalVendido = reader.GetDouble("totalVendido");
                                reporte.articuloMasVendido = reader.GetString("articuloMasVendido");
                                reporte.totalArticulos = reader.GetInt32("totalArticulos");
                                reporte.totalGastos = reader.GetDouble("totalGastos");
                                reporte.totalIngresos = reader.GetDouble("totalIngresos");
                            }
                            response.success = true;
                            response.message = "Datos Obtenidos Correctamente";
                            response.Data = reporte;
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

        public async Task<Response<List<ReporteProducto>>> ObtenerReporteProductoPorDia(int idEmpresa, string fecha)
        {
            var response = new Response<List<ReporteProducto>>();
            try
            {
                using(var connection = new SqlConnection(con.getConnection()))
                {
                    using(var command = new SqlCommand("Reporte.spReporteProductosPorDia", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idEmpresa", idEmpresa);
                        command.Parameters.AddWithValue("@fecha", fecha);
                        connection.Open();
                        using(var reader = await command.ExecuteReaderAsync())
                        {
                            var list = new List<ReporteProducto>();
                            while (reader.Read())
                            {
                                var reporte = new ReporteProducto();
                                reporte.idProducto = reader.GetInt32("idProducto");
                                reporte.nombre = reader.GetString("nombre");
                                reporte.precio = reader.GetDouble("precio");
                                reporte.cantidad = reader.GetInt32("cantidad");
                                reporte.total = reader.GetDouble("total");
                                list.Add(reporte);
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

        public async Task<Response<List<ReporteProducto>>> ObtenerReporteProductoPorIntervalo(IntervaloTicket i)
        {
            var response = new Response<List<ReporteProducto>>();
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using (var command = new SqlCommand("Reporte.spReporteProductosPorIntervalo", connection))
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
                            var list = new List<ReporteProducto>();
                            while (reader.Read())
                            {
                                var reporte = new ReporteProducto();
                                reporte.idProducto = reader.GetInt32("idProducto");
                                reporte.nombre = reader.GetString("nombre");
                                reporte.precio = reader.GetDouble("precio");
                                reporte.cantidad = reader.GetInt32("cantidad");
                                reporte.total = reader.GetDouble("total");
                                list.Add(reporte);
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

        public async Task<Response<List<ReporteProducto>>> ObtenerReporteProductoPorMes(int idEmpresa, int mes)
        {
            var response = new Response<List<ReporteProducto>>();
            try
            {
                using (var connection = new SqlConnection(con.getConnection()))
                {
                    using (var command = new SqlCommand("Reporte.spReporteProductoPorMes", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idEmpresa", idEmpresa);
                        command.Parameters.AddWithValue("@mes", mes);
                        connection.Open();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            var list = new List<ReporteProducto>();
                            while (reader.Read())
                            {
                                var reporte = new ReporteProducto();
                                reporte.idProducto = reader.GetInt32("idProducto");
                                reporte.nombre = reader.GetString("nombre");
                                reporte.precio = reader.GetDouble("precio");
                                reporte.cantidad = reader.GetInt32("cantidad");
                                reporte.total = reader.GetDouble("total");
                                list.Add(reporte);
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
