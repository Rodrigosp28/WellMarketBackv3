using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WellMarket.Entities
{
    public class Zona: Municipio
    {
        public int idZona { get; set; }
        public string descripcionZona { get; set; }
    }
}
