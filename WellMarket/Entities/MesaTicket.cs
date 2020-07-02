using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WellMarket.Entities
{
    public class MesaTicket
    {
        public int idMesaTicket { get; set; }
        public int idTicket { get; set; }
        public int idMesa { get; set; }
        public Boolean ocupado { get; set; }
    }
}
