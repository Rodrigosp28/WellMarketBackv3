using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WellMarket.Entities.Responses
{
    public class TicketVentaResponse
    {
        public List<Venta> venta { get; set; }
        public Ticket ticket { get; set; }
        public Mesa mesa { get; set; }
        public string hola {get;set;}
    }
}
