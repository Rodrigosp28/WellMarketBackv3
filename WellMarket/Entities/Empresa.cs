using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WellMarket.Entities
{
    public class Empresa
    {
        public int idEmpresa { get; set; }
        public string nombre { get; set; }
        public string direccion { get; set; }
        public string rfc { get; set; }
        public string encargado { get; set; }
        public string vision { get; set; }
        public string mision { get; set; }
        public string telefono { get; set; }
        public string urlLogo { get; set; }
        public int idRolEmpresa { get; set; }
        public string nombreRol { get; set; }
        public string fecha { get; set; }
        public Logo logo { get; set; }
        public Boolean abierto { get; set; }

    }
}
