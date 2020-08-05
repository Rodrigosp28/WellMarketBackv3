using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WellMarket.Entities
{
    public class Gasto
    {
        public int idGasto { get; set; }
        public string concepto { get; set; }
        public string descripcion { get; set; }
        public double cantidad { get; set; }
        public string fecha { get; set; }
        public string hora { get; set; }
        public int idEmpresa { get; set; }
    }
}
