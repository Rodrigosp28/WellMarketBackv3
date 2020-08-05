using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WellMarket.Entities.Request
{
    public class IntervaloTicket
    {
        public int idEmpresa { get; set; }
        public string fechaInicio { get; set; }
        public string fechaFinal { get; set; }
        public string horaInicio { get; set; }
        public string horaFinal { get; set; }
    }
}
