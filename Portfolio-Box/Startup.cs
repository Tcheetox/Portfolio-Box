using System;
using System.Globalization;
using System.Linq;
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
using Portfolio_Box.Models.Files;
using Portfolio_Box.Models.Links;
using Portfolio_Box.Models.Users;

namespace Portfolio_Box
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDBContext>(options =>
            {
                var connectionString = Configuration.GetConnectionString("DefaultConnection")!;
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });

            services.AddHttpContextAccessor();
            services.AddHttpClient();

            services.AddSingleton(Configuration);
            services.AddScoped<CookieHandler>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped(p => p.GetService<IUserRepository>()!.GetUserByAccessToken());
            services.AddScoped<IFileFactory, FileFactory>();
            services.AddScoped<IFileRepository, FileRepository>();
            services.AddScoped<ILinkRepository, LinkRepository>();
            services.AddControllers();

            services.Configure<KestrelServerOptions>(o => o.Limits.MaxRequestBodySize = Configuration.GetValue<long>("File:MaxBytes"));

            services.AddRazorPages(o =>
                o.Conventions.AddPageApplicationModelConvention("/Index", model =>
                    model.Filters.Add(new AntiforgeryTokenCookieAttribute()))
                );
            services.Configure<RazorViewEngineOptions>(options => options.ViewLocationFormats.Add("/Pages/{0}.cshtml"));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCsp(options => options
                .BaseUris(s => s.Self())
                .ObjectSources(s => s.None())
                .ScriptSources(s => s.Self())
                .ReportUris(s => s.Uris(Configuration.GetValue<string>("Hosting:CspReport"))));

            app.UsePathBase(Configuration.GetBasePath());


            // Do the migration stuff
            using var serviceScope = app
                .ApplicationServices
                .GetService<IServiceScopeFactory>()!
                .CreateScope();
            var database = serviceScope
                .ServiceProvider
                .GetRequiredService<AppDBContext>()
                .Database;
            database.EnsureCreated();
            database.Migrate();


            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseExceptionHandler("/Error");

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
                endpoints.MapGet("/healthcheck", async context =>
                {
                    string? forward = context.Request.Headers["X-Forwarded-For"];
                    var ipAddress = forward?.Split(',', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault()?.Trim();
                    var isAllowed = env.IsDevelopment() || Configuration.GetValue("Hosting:Whitelisted", string.Empty)!.Equals(ipAddress, StringComparison.OrdinalIgnoreCase);

                    if (!isAllowed)
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        return;
                    }

                    await context.Response.WriteAsync("Service is healthy");
                });
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
