using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WellMarket.Entities
{
    public class Venta
    {
        public int idVenta { get; set; }
        public int idTicket { get; set; }
        public int idProducto { get; set; }
        public Producto producto { get; set; }
        public int cantidad { get; set; }
        public double total { get; set; }
    }
}
