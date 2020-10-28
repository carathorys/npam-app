using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using FuryTechs.LinuxAdmin.Core.ProfileService;
using FuryTechs.LinuxAdmin.Core.Validators;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Npam;

namespace FuryTechs.LinuxAdmin.Core
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
            services.AddControllersWithViews();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration => { configuration.RootPath = "clientapp/build"; });
            services.AddHttpContextAccessor();

            services.AddIdentityServer(options =>
                {
                    options.InputLengthRestrictions.Password = int.MaxValue;
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                })
                .AddInMemoryCaching()
                .AddInMemoryApiScopes(new List<ApiScope>
                {
                })
                .AddInMemoryClients(new List<Client>
                {
                    // JavaScript Client
                    new Client
                    {
                        ClientId = "js",
                        ClientName = "JavaScript Client",
                        AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                        RequireClientSecret = false,

                        RedirectUris = {"https://localhost:5003/callback.html"},
                        PostLogoutRedirectUris = {"https://localhost:5003/index.html"},
                        AllowedCorsOrigins = {"https://localhost:5003"},

                        AllowedScopes =
                        {
                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.Profile,
                            "api1"
                        }
                    }
                })
                .AddResourceOwnerValidator<PamUserValidator>()
                .AddProfileService<PamProfileService>()
                .AddSigningCredential(
                    new X509Certificate2(Path.Combine(Directory.GetCurrentDirectory(), "Contents", "key.pfx")));
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "clientapp";

                if (env.IsDevelopment())
                {
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:3000");
                    // spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}