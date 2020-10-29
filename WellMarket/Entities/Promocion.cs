using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WellMarket.Entities
{
    public class Promocion
    {
        public int idPromocion { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public double precio { get; set; }
        public Boolean disponible { get; set; }
        public int diaDisponible { get; set; }
        public int idEmpresa { get; set; }
        public List<Imagen_promocion> imagenes { get; set; }
        public string nombreEmpresa { get; set; }
        public string telefonoEmpresa { get; set; }
        public string direccionEmpresa { get; set; }
        public Boolean habilitado { get; set; }

    }
}