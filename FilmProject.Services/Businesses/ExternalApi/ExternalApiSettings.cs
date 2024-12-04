using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmProject.Services.Businesses.ExternalApi
{
    public class ExternalApiSettings
    {
        public KernelServer Kernel { get; set; }
        public IISServer IIS { get; set; }
    }
    public class KernelServer
    {
        public string URLHttps { get; set; }
        public string URLHttp { get; set; }
    }
    public class IISServer
    {
        public string applicationUrl { get; set; }
        public string sslUrl { get; set; }
    }
}
