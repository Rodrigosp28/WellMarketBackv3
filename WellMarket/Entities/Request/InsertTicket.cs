using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WellMarket.Entities.Request
{
    public class InsertTicket
    {
        public int idTicket { get; set; }
        public string fecha { get; set; }
        public string horaEntrada { get; set; }
        public int idUsuario { get; set; }
        public int idEmpresa { get; set; }
        public int idMesa { get; set; }
    }
}
