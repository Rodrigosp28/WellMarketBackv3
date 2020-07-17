using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WellMarket.Entities.Responses
{
    public class Dashboard
    {
        public double totalVendido { get; set; }
        public string articuloMasVendido { get; set; }
        public int totalArticulos { get; set; }
    }
}
