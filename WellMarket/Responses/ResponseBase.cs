using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WellMarket.Responses
{
    public class ResponseBase
    {
        public int id { get; set; }
        public Boolean success { get; set; }
        public string message { get; set; }
    }
}
