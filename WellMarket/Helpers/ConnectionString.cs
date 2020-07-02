using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WellMarket.Helpers
{
    public interface IConnection
    {
        string getConnection();
    }
    public class ConnectionString: IConnection
    {
        private readonly IConfiguration configuracion;

        public ConnectionString(IConfiguration configuracion)
        {
            this.configuracion = configuracion;
        }
        public string getConnection()
        {
            string coneccion = configuracion["connectionStrings:defaultConnectionString"];
            return coneccion;

        }

        public string getPath()
        {
            return "/Content/Images";
        }
    }
}
