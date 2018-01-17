using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using TransportApp.Models;
using TransportApp.DbCore;
using Microsoft.EntityFrameworkCore;

namespace TransportApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .Build();


            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<RepoContext>();
                    DbInitializer.Initialize(context);
                }
                catch (Exception ex)
                {
                    //  var logger = services.GetRequiredService<ILogger<Program>>();
                    //  logger.LogError(ex, "An error occurred while seeding the database.");
                    Console.Write(ex.Message);
                }
            }

            host.Run();
        }
    }
}
