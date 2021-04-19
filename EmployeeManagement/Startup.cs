using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.Models;
using EmployeeManagement.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
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

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 2;
            }).AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddMvc(config => {
                config.EnableEndpointRouting = false;

                var policy = new AuthorizationPolicyBuilder()
                                .RequireAuthenticatedUser()
                                .Build();

                config.Filters.Add(new AuthorizeFilter(policy));

            }).AddRazorRuntimeCompilation();

            //authentication
            services.AddAuthentication()
                    .AddGoogle(options =>
                    {
                        options.ClientId = "942482808548-1rduin0mb6ng9io55at1mvfhd0sckqhr.apps.googleusercontent.com";
                        options.ClientSecret = "OgJNh4ZuxW2fb2N8Gr0x8low";
                    })
                    .AddFacebook(options =>
                    {
                        options.AppId = "460967725155733";
                        options.AppSecret = "b65d5181ccb90df721e74c52eb5298f3";
                    });

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = new PathString("/Administration/AccessDenied");
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("DeleteRolePolicy", policy => policy.RequireClaim("Delete Role"));

                //options.AddPolicy("EditRolePolicy", policy => policy.RequireAssertion(context =>
                //    context.User.IsInRole("Admin") &&
                //    context.User.HasClaim(claim => claim.Type == "Edit Role" && claim.Value == "true") ||
                //    context.User.IsInRole("Super Admin")
                //));

                options.AddPolicy("EditRolePolicy", policy => policy.AddRequirements(new ManageAdminRolesAndClaimsRequirement()));

                options.AddPolicy("RolePolicy", policy => policy.RequireRole("Super Admin","Admin", "Employee"));

                //if do not want the rest of the handlers to be called
                //when a failure is returned
                //options.InvokeHandlersAfterFailure = false;
            });

            services.AddScoped<IEmployeeRepository, SQLEmployeeRepository>();

            //requirement implementation
            services.AddSingleton<IAuthorizationHandler, CanEditOnlyOtherAdminRolesAndClaimsHandler>();
            services.AddSingleton<IAuthorizationHandler, SuperAdminHandler>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseRouting();

            app.UseMvc(route =>
            {
                route.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
