using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WellMarket.Entities
{
    public class DisponibilidadDomicilio
    {
        public int idDisponibilidad { get; set; }
        public int idEmpresa { get; set; }
        public int idZona { get; set; }
        public string nombreZona { get; set; }
        public string nombreMunicipio { get; set; }
    }
}
