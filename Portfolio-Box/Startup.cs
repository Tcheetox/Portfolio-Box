using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Portfolio_Box.Attributes;
using Portfolio_Box.Extensions;
using Portfolio_Box.Models;
using Portfolio_Box.Models.Shared;
using System;
using System.Globalization;

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
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped(provider => provider.GetService<IUserRepository>().GetUserByAccessToken());
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
            app.UseCsp(options => options
                .BaseUris(s => s.Self())
                .ObjectSources(s => s.None())
                .ScriptSources(s => s.Self())
                .ReportUris(s => s.Uris(Configuration.GetValue<string>("Hosting:CspReport"))));

            string basePath = new PathString(Configuration.GetValue<string>("Hosting:BasePath"));
            SharedFileExtension.BasePath = basePath;
            app.UsePathBase(basePath);
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            // app.UseHttpsRedirection(); // -> Nginx handles it let's not use https through proxy_pass directive
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    // Cache static files for 30 days
                    ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=2592000");
                    ctx.Context.Response.Headers.Append("Expires", DateTime.UtcNow.AddDays(30).ToString("R", CultureInfo.InvariantCulture));
                }
            });
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
