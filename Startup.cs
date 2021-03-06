using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using  DataAccesLib.DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;

namespace VAII
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(options =>
                {
                    options.UseSqlServer(Configuration.GetConnectionString("Default"));
                }
                );
            services.AddControllersWithViews();
            
            services.AddSession();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.Use(async (context, next) =>
            {
                var paths = new[]
                {
                    "/Admin/Cms",
                    "/CMSSeries",
                    "/CMSBrands",
                    "/CMSServisDevices",
                };
                bool lockedPath = false;
                string currentUserIdSession = context.Session.GetString("user");
                foreach (var path in paths)
                {
                    if (context.Request.Path.Value.Contains(path))
                    {
                        lockedPath = true;
                        break;
                    }
                }
                if (lockedPath)
                {
                    
                    if (string.IsNullOrEmpty(currentUserIdSession))
                    {
                        var path = $"/Admin";
                        context.Response.Redirect(path);
                        return;
                    }

                }
                await next();
            });



            app.UseRouting();

           
            app.UseAuthorization();

           

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });


           
        }
    }
}
