using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WellMarket.Responses
{
    public class Response<T>:ResponseBase
    {
        public T Data { get; set; }
    }
}
