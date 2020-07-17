using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WellMarket.Entities
{
    public class Logo
    {
        public int idLogo { get; set; }
        public int idEmpresa { get; set; }
        public string imagen { get; set; }
        public string url { get; set; }
    }
}
