using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace WellMarket.Entities
{
    public class Producto
    {
        public int idProducto { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public double precio { get; set; }
        public int idDisponible { get; set; }
        public disponible_producto disponible { get; set; }
        public int idEmpresa { get; set; }
        public int idCategoria { get; set; }
        public Categoria_Producto categoria { get; set; }
        public List<Imagenes_Producto> imagenes { get; set; }
    }
}
