using System.Linq;
using System.Net;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace FarmaciaSimanEntrenamientoProductos
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        static IHostingEnvironment hostingEnvironment;
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, configuration) =>
                {
                    hostingEnvironment = hostingContext.HostingEnvironment;
                })
          
            .UseStartup<Startup>()
                .Build();
    }
}
