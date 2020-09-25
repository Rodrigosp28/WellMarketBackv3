using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WellMarket.Entities
{
    public class Usuario
    {
        public int idUsuario { get; set; }
        public string usuario { get; set; }
        public string password { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string direccion { get; set; }
        public string telefono { get; set; }
        public string correo { get; set; }
        public int idZona { get; set; }
        public int idRol { get; set; }
        public string fecha { get; set; }
        public Boolean activo { get; set; }
        public int idTipoUsuario { get; set; }
        public int idEmpresa { get; set; }
        public Boolean verificado { get; set; }
    }
}
