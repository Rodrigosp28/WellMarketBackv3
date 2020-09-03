using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WellMarket.Entities
{
    public class Cocina
    {
        public int idCocina { get; set; }
        public int idVenta { get; set; }
        public string fecha { get; set; }
        public string horaEntrada { get; set; }
        public string horaSalida { get; set; }
        public Boolean activo { get; set; }
        public string nota { get; set; }
        public string producto { get; set; }
        public int cantidad { get; set; }

        public int idTicket { get; set; }
    }
}
