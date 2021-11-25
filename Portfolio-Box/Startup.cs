using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Portfolio_Box.Attributes;
using Portfolio_Box.Models;
using Portfolio_Box.Models.Shared;
using Portfolio_Box.Models.User;
using System.Diagnostics;

namespace Portfolio_Box
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDBContext>(options => options.UseMySQL(Configuration.GetConnectionString("DefaultConnection")));
            services.AddHttpContextAccessor();
            services.AddSingleton(Configuration);
            services.AddScoped<CookieHandler>();
            services.AddScoped(user => User.GetUser(user));
            services.AddScoped<ISharedFileFactory, SharedFileFactory>();
            services.AddScoped<ISharedFileRepository, SharedFileRepository>();
            services.AddScoped<ISharedLinkRepository, SharedLinkRepository>();
            services.AddControllers();

            services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = Configuration.GetValue<long>("File:MaxBytes");
            });


            services.AddRazorPages(options =>
                options.Conventions.AddPageApplicationModelConvention("/Index", model =>
                    model.Filters.Add(new AntiforgeryTokenCookieAttribute())));
            services.Configure<RazorViewEngineOptions>(options => options.ViewLocationFormats.Add("/Pages/{0}.cshtml"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UsePathBase(new PathString(Configuration.GetValue<string>("Hosting:BasePath")));

            if (env.IsDevelopment())
            {
                Debug.WriteLine("> Portfolio-Box running in development mode");
                app.UseDeveloperExceptionPage();
            }
            else
            {
                Debug.WriteLine("> Portfolio-Box running in production mode");
                app.UseExceptionHandler("/Error");
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
