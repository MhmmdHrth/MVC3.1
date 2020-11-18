using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


namespace EmployeeManagement
{
    public class Startup
    {
        private readonly IConfiguration Configuration;

        public Startup(IConfiguration Configuration)
        {
            this.Configuration = Configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<ApplicationDbContext>
                (options => options.UseSqlServer(Configuration.GetConnectionString("EmployeeDB")));

            services.AddMvc(config => config.EnableEndpointRouting = false);
            services.AddScoped<IEmployeeRepository, SQLEmployeeRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePagesWithRedirects("/Error/{0}");
            }

            app.UseRouting();

            app.UseStaticFiles();

            app.UseMvc(route =>
            {
                route.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
