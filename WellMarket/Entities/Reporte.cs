using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WellMarket.Entities
{
    public class Reporte
    {
        public double totalVendido { get; set; }
        public string articuloMasVendido { get; set; }
        public int totalArticulos { get; set; }
        public double totalGastos { get; set; }
        public double totalIngresos { get; set; }
    }
}
