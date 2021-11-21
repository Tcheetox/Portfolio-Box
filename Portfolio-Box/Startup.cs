using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Portfolio_Box.Attributes;
using Portfolio_Box.Models;
using Portfolio_Box.Models.Shared;
using Portfolio_Box.Models.User;

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
            // Register our own services 
            services.AddDbContext<AppDBContext>(options => options.UseMySQL(Configuration.GetConnectionString("DefaultConnection")));
            services.AddHttpContextAccessor();
            services.AddSingleton(Configuration);
            services.AddScoped<CookieHandler>();
            services.AddScoped(user => User.GetUser(user));
            services.AddScoped<ISharedFileRepository, SharedFileRepository>();
            // Register framework services
            services.AddControllersWithViews();

            // TODO: config
            services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = int.MaxValue; // if don't set default value is: 30 MB
            });

            services.AddRazorPages(options => {
                options.Conventions.AddPageApplicationModelConvention("/FileUpload",model => {
                   model.Filters.Add(new GenerateAntiforgeryTokenCookieAttribute());
                   model.Filters.Add(new DisableFormValueModelBindingAttribute());
               });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UsePathBase(new PathString(Configuration.GetValue<string>("Hosting:BasePath")));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                // else => ??? app.UseExceptionHandler("/Home/Error");
            }

            app.UseHttpsRedirection();
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
