using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using WebApplicationWinService.Extentions;

namespace WebApplicationWinService
{
    public class Program
    {
        // https://stackoverflow.com/questions/37346383/hosting-asp-net-core-as-windows-service
        // https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/windows-service?view=aspnetcore-2.2
        public static void Main(string[] args)
        {
            var isService = !(Debugger.IsAttached || args.Contains("--console"));

            if (isService)
            {
                var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
                var pathToContentRoot = Path.GetDirectoryName(pathToExe);
                Directory.SetCurrentDirectory(pathToContentRoot);
            }

            var builder = CreateWebHostBuilder(
                args.Where(arg => arg != "--console").ToArray());

            var host = builder.Build();

            if (isService)
            {
                // To run the app without the CustomWebHostService change the
                // next line to host.RunAsService();

                host.RunAsCustomService();
                //host.RunAsService();
            }
            else
            {
                host.Run();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging((hostingContext, logging) =>
                {

                    //logging.AddEventLog();
                })
                .ConfigureAppConfiguration((context, config) =>
                {
                    // Configure the app here.
                })
                .UseStartup<Startup>();
    }

    internal class CustomWebHostService : WebHostService
    {
        public CustomWebHostService(IWebHost host) : base(host)
        {
        }

        protected override void OnStarting(string[] args)
        {
            // Log
            base.OnStarting(args);
        }

        protected override void OnStarted()
        {
            // More log
            base.OnStarted();
        }

        protected override void OnStopping()
        {
            // Even more log
            base.OnStopping();
        }
    }

}
