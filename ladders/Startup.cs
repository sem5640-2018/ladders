using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.MySql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using ladders.Models;
using ladders.Repositories;
using ladders.Repositories.Interfaces;
using ladders.Services;
using ladders.Shared;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.HttpOverrides;

namespace ladders
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly IHostingEnvironment _environment;
        private readonly IConfigurationSection _appConfig;
        
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            _environment = env;
            _appConfig = configuration.GetSection("ladders");
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IChallengesRepository, ChallengesRepository>();
            services.AddScoped<ILaddersRepository, LadderRepository>();
            services.AddScoped<IProfileRepository, ProfileRepository>();
            
            services.AddHttpClient();
            services.AddHttpClient("LadderClient", client => {
            });
            services.AddSingleton<IApiClient, ApiClient>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            
            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = "Cookies";
                    options.DefaultChallengeScheme = "oidc";
                })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                {
                    options.SignInScheme = "Cookies";
                    options.Authority = _appConfig.GetValue<string>("GatekeeperUrl");
                    options.ClientId = _appConfig.GetValue<string>("ClientId");
                    options.ClientSecret = _appConfig.GetValue<string>("ClientSecret");
                    options.ResponseType = "code id_token";
                    options.SaveTokens = true;
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.Scope.Add("profile");
                    options.Scope.Add("offline_access");
                    options.ClaimActions.MapJsonKey("locale", "locale");
                    options.ClaimActions.MapJsonKey("user_type", "user_type");
                })
                .AddIdentityServerAuthentication("Bearer", options =>
                {
                    options.Authority = _appConfig.GetValue<string>("GatekeeperUrl");
                    options.ApiName = _appConfig.GetValue<string>("ApiResourceName");
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Administrator", pb => pb.RequireClaim("user_type", "administrator"));

                // Coordinator policy allows both Coordinators and Administrators
                options.AddPolicy("Coordinator", pb => pb.RequireClaim("user_type", "administrator", "coordinator"));
            });
            
            services.AddMvc().AddRazorPagesOptions(options =>
            {
                options.Conventions.AddPageRoute("/Ladders/Index", "");
            });

            var connectionString = Configuration.GetConnectionString("LaddersContext");

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDbContext<LaddersContext>(options =>
                    options.UseMySql(connectionString));
            
            services.AddHangfire(x => x.UseStorage(new MySqlStorage(connectionString, new MySqlStorageOptions())));
            
            if (!_environment.IsDevelopment())
            {
                services.Configure<ForwardedHeadersOptions>(options =>
                {
                    var proxyHost = _appConfig.GetValue("ReverseProxyHostname", "http://nginx");
                    var proxyAddresses = Dns.GetHostAddresses(proxyHost);
                    foreach (var ip in proxyAddresses)
                    {
                        options.KnownProxies.Add(ip);
                    }
                });
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseHangfireDashboard();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                var pathBase = _appConfig.GetValue<string>("PathBase");
                RunMigrations(app);
                app.UsePathBase(pathBase);
                app.Use((context, next) =>
                {
                    context.Request.PathBase = new PathString(pathBase);
                    return next();
                });
                app.UseForwardedHeaders(new ForwardedHeadersOptions
                {
                    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
                });
                app.UseExceptionHandler("/Shared/Error");
                app.UseHsts();
            }

            app.UseHangfireServer();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
          
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Ladders}/{action=Index}/{id?}");
            });

            RecurringJob.AddOrUpdate<EmailManager>(es => es.SendScheduledEmails(), Cron.Minutely);
        }
        
        private void RunMigrations(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var serviceProvider = serviceScope.ServiceProvider;
                var dbContext = serviceProvider.GetService<LaddersContext>();
                dbContext.Database.Migrate();
            }
        }
    }
}
