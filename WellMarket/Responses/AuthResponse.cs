using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WellMarket.Entities;

namespace WellMarket.Responses
{
    public class AuthResponse
    {
        public Boolean success { get; set; }
        public string token { get; set; }
        public Usuario user { get; set; }
        public string messages { get; set; }
    }
}
