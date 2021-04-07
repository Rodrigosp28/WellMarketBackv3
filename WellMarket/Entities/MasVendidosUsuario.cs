//entidad para mas vendidos para la interfaz de usuario
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WellMarket.Entities
{
    public class MasVendidosUsuario
    {
        public string producto { get; set; }
        public int idEmpresa {get;set;}
        public string nombreEmpresa { get; set; }
        public double precio { get; set; }
        public string imagen { get; set; }
        public string logo { get; set; }
    }
}
