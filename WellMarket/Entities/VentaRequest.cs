using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WellMarket.Entities
{
    public class VentaRequest
    {
        public Ticket ticket { get; set; }
        public List<Venta> productos { get; set; }
    }
}
