using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WellMarket.Entities
{
    public class Ticket
    {
        public int idTicket { get; set; }
        public string fecha { get; set; }
        public string fechaSalida { get; set; }
        public string hora { get; set; }
        public string horaEntrada { get; set; }
        public string horaSalida { get; set; }
        public double total { get; set; }
        public string descripcion { get; set; }
        public string comentario { get; set; }
        public int idEstatus { get; set; }
        public string nombreEstatus { get; set; }
        public int idUsuario { get; set; }
        public string nombreUsuario { get; set; }
        public int idEmpresa { get; set; }
        public Boolean activo{get;set;}
        public double pago { get; set; }
        public double cambio { get; set; }
    }
}
