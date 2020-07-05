using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

using Cookies.AuthorizationRequirements;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Cookies {
    public class Startup {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services) {
            services.AddAuthentication("CookiesAuth").AddCookie("CookiesAuth", config => {
                config.Cookie.Name = "Grandmas.Cookie";
                config.LoginPath = "/Home/Authenticate";
            });

            services.AddAuthorization(config => {

                //     var defaultAuthBuilder = new AuthorizationPolicyBuilder();
                //     var defaultAuthPolicy = defaultAuthBuilder
                //         .RequireAuthenticatedUser()
                //         .RequireClaim(ClaimTypes.DateOfBirth)
                //         .Build();
                //     config.DefaultPolicy = defaultAuthPolicy;
                // });

                // config.AddPolicy("Claim.DoB", policyBuilder=>{
                //     policyBuilder.RequireClaim(ClaimTypes.DateOfBirth);
                // });                    
                // config.AddPolicy("Claim.DoB", policyBuilder => {
                //     policyBuilder.AddRequirements(new CustomRequireClaim(ClaimTypes.DateOfBirth));
                // });

                config.AddPolicy("Admin", policyBuilder =>
                    policyBuilder.RequireClaim(ClaimTypes.Role, "Admin")
                );

                config.AddPolicy("Claim.DoB", policyBuilder => {
                    policyBuilder.RequireCustomClaim(ClaimTypes.DateOfBirth);
                });
            });
            services.AddScoped<IAuthorizationHandler, CustomRequireClaimHandler>();

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            //who you are
            app.UseAuthentication();

            // are you allowed
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
