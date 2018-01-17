using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using TransportApp.DbCore;
using TransportApp.Models;
using System;

namespace TransportApp
{
    public class Startup
    {
        public static IConfiguration Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            try
            {
                services.AddDbContext<RepoContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            }

            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            
            app.Use(async (context, next) => {
                await next();
                if (context.Response.StatusCode == 404 &&
                   !Path.HasExtension(context.Request.Path.Value) &&
                   !context.Request.Path.Value.StartsWith("/api/"))
                {
                    context.Request.Path = "/index.html";
                    await next();
                }
            });

            app.UseMvcWithDefaultRoute();
            app.UseDefaultFiles();
            app.UseStaticFiles();
        }
    }
}
