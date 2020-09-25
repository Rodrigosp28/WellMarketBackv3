using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WellMarket.Entities
{
    public class CatProducto
    {
        public int idCatProducto { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public int idEmpresa { get; set; }
        public Boolean habilitado { get; set; }
    }
}
