using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nocrastination.Core.Entities;
using Nocrastination.Data;
using Nocrastination.Interfaces;
using Nocrastination.Services;
using Nocrastination.Settings;

namespace Nocrastination
{
    public class Startup
    {
        public IHostingEnvironment Enviroment;
        public IConfiguration Configuration { get; }

        public Startup(IHostingEnvironment enviroment)
        {
            Enviroment = enviroment;

            var builder = new ConfigurationBuilder()
                .SetBasePath(enviroment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{enviroment.EnvironmentName}.json", optional: true, reloadOnChange: true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(Configuration);
            //services.AddSingleton(Configuration.GetSection("IdentityServer:Api").Get<ApiSettings>());
            //services.AddSingleton(Configuration.GetSection("IdentityServer:Client").Get<ClientSettings>());
            services.AddSingleton(Configuration.GetSection("IdentityServer").Get<IdentityServerSettings>());
            //services.AddSingleton(Configuration.GetSection("AppSettings:Store").Get<StoreSettings>());
            services.AddSingleton(Configuration.GetSection("AppSettings").Get<AppSettings>());

            services.AddScoped<IRepository<AppUser>, DbRepository<AppUser>>();
            services.AddScoped<IRepository<Tasks>, DbRepository<Tasks>>();
            services.AddScoped<IRepository<Store>, DbRepository<Store>>();
            services.AddScoped<IRepository<Purchase>, DbRepository<Purchase>>();

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITasksService, TasksService>();
            services.AddScoped<IStoreService, StoreService>();
            services.AddScoped<IPurchaseService, PurchaseService>();

            services.AddDbContext<ApplicationDbContext>(opt =>
                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<AppUser, IdentityRole>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddIdentityServer(opt =>
                    opt.PublicOrigin = Configuration.GetSection("ServerHost").Value)
                .AddDeveloperSigningCredential()
                .AddInMemoryApiResources(IdentityServerConfig.GetApiResources())
                .AddInMemoryClients(IdentityServerConfig.GetClients())
                .AddAspNetIdentity<AppUser>();

            services.AddAuthentication(opt =>
            {
                opt.DefaultScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                opt.DefaultAuthenticateScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
            })
            .AddIdentityServerAuthentication(opt =>
            {
                opt.Authority = Configuration.GetSection("ServerHost").Value;
                opt.RequireHttpsMetadata = false;
                opt.ApiName = ClientSettings.Id;
                opt.ApiSecret = ClientSettings.Secret;
            });

            services.ConfigureApplicationCookie(opt =>
            {
                opt.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
            });

            services.AddMvcCore()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddCors()
                .AddFormatterMappings()
                .AddCacheTagHelper()
                .AddJsonFormatters()
                .AddAuthorization();

            services.AddMvc();

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseCors(builder =>
            {
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
                builder.AllowAnyOrigin();
            });

            app.UseIdentityServer();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
