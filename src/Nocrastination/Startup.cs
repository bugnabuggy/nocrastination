using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
using Nocrastination.Helpers;
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

	        var identityServerSection = Configuration.GetSection("IdentityServer");
	        IdentityServerSettings.Client = identityServerSection["Client"];
			IdentityServerSettings.Api = identityServerSection["Api"];
	        IdentityServerSettings.Secret = identityServerSection["Secret"];
	        IdentityServerSettings.AccessTokenLifetime = int.Parse(identityServerSection["AccessTokenLifetime"]);
			IdentityServerSettings.ServerHost = identityServerSection["ServerHost"];

			//services.AddSingleton(Configuration.GetSection("IdentityServer").Get<IdentityServerSettings>());

            //services.AddSingleton(Configuration.GetSection("AppSettings:Store").Get<StoreSettings>());
            services.AddSingleton(Configuration.GetSection("AppSettings").Get<AppSettings>());

            services.AddScoped<IRepository<AppUser>, DbRepository<AppUser>>();
            services.AddScoped<IRepository<Tasks>, DbRepository<Tasks>>();
            services.AddScoped<IRepository<Store>, DbRepository<Store>>();
            services.AddScoped<IRepository<Purchase>, DbRepository<Purchase>>();

	        services.AddScoped<IClaimsHelper, ClaimsHelper>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITasksService, TasksService>();
            services.AddScoped<IStoreService, StoreService>();
            services.AddScoped<IPurchaseService, PurchaseService>();

			services.AddDbContext<ApplicationDbContext>(opt =>
                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<AppUser, IdentityRole>(opt =>
            {
	            opt.Password.RequiredLength = 3;
	            opt.Password.RequireLowercase = false;
	            opt.Password.RequireUppercase = false;
	            opt.Password.RequireNonAlphanumeric = false;
	            opt.Password.RequireDigit = false;
			})
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

			services.AddIdentityServer()
                .AddDeveloperSigningCredential()
				.AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
				.AddInMemoryApiResources(IdentityServerConfig.GetApiResources())
                .AddInMemoryClients(IdentityServerConfig.GetClients())
                .AddAspNetIdentity<AppUser>();

			services.AddAuthentication(opt =>
				{
					opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
					opt.DefaultAuthenticateScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
					opt.DefaultChallengeScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
				})
				.AddIdentityServerAuthentication(opt =>
				{
					opt.Authority = IdentityServerSettings.ServerHost;
					opt.RequireHttpsMetadata = false;
					opt.ApiName = IdentityServerSettings.Api;
					opt.ApiSecret = IdentityServerSettings.Secret;
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
