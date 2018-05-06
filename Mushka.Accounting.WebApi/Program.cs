using System.IO;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using NLog.Web;

namespace Mushka.Accounting.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
            //IWebHost host = new WebHostBuilder()
            //    .UseKestrel()
            //    .ConfigureServices(services => services.AddAutofac())
            //    .UseContentRoot(Directory.GetCurrentDirectory())
            //    .UseIISIntegration()
            //    .UseNLog()
            //    .UseStartup<Startup>()
            //    .Build();

            //host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(services => services.AddAutofac())
                .UseNLog()
                .UseStartup<Startup>()
                .Build();
    }
}