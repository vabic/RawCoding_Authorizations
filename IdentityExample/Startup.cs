using IdentityExample.Data;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityExample {
    public class Startup {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices (IServiceCollection services) {
            services.AddDbContext<AppDbContext> (config => {
                config.UseInMemoryDatabase ("Memory");
            });

            //AddIdrentity registers the services
            services.AddIdentity<IdentityUser, IdentityRole> (config => {
                    config.Password.RequiredLength = 4;
                    config.Password.RequireDigit = false;
                    config.Password.RequireNonAlphanumeric = false;
                    config.Password.RequireUppercase = false;

                })
                .AddEntityFrameworkStores<AppDbContext> ()
                .AddDefaultTokenProviders ();

            services.ConfigureApplicationCookie (config => {
                config.Cookie.Name = "Identity.Cookie";
                config.LoginPath = "/Home/Login";
            });

            // services.AddAuthentication ("CookiesAuth").AddCookie ("CookiesAuth", config => {
            //     config.Cookie.Name = "Grandmas.Cookie";
            //     config.LoginPath = "/Home/Authenticate";
            // });
            services.AddControllersWithViews ();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            }

            app.UseRouting ();

            //who you are
            app.UseAuthentication ();

            // are you allowed
            app.UseAuthorization ();

            app.UseEndpoints (endpoints => {
                endpoints.MapDefaultControllerRoute ();
            });
        }
    }
}