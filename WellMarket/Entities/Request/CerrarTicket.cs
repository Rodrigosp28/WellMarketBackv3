using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WellMarket.Entities.Request
{
    public class CerrarTicket
    {
        public int idTicket { get; set; }
        public string fechaSalida { get; set; }
        public string horaSalida { get; set; }
        public double total { get; set; }
        public string descripcion { get; set; }
        public string comentario { get; set; }
        public double pago { get; set; }
        public double cambio { get; set; }
        public int idMesa { get; set; }

    }
}
