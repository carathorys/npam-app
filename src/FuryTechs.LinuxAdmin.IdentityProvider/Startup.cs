using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using FuryTechs.LinuxAdmin.Identity;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FuryTechs.LinuxAdmin.IdentityProvider
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddAntiforgery();

            services.AddSession(c => { c.IdleTimeout = TimeSpan.FromHours(1); });

            services
                .AddLinuxIdentity()
                .AddDefaultTokenProviders();

            services.AddScoped<RSA>((s) => RSA.Create(4096));

            services.AddIdentityServer(options =>
                {
                    options.InputLengthRestrictions.Password = int.MaxValue;
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                })
                .AddAspNetIdentity<User>()
                .AddInMemoryPersistedGrants()
                .AddInMemoryIdentityResources(new IdentityResource[]
                {
                    new IdentityResources.OpenId(),
                    new IdentityResources.Profile(),
                    new IdentityResources.Email()
                })
                .AddInMemoryApiResources(new ApiResource[]
                {
                    new ApiResource
                    {
                        Name = "api1",
                        Scopes =
                        {
                            // new Scope ("api1")
                        }
                    }
                })
                .AddInMemoryClients(new[]
                {
                    new Client
                    {
                        ClientId = "js",
                        ClientName = "JavaScript Client",
                        AllowedGrantTypes = new []
                        {
                            GrantType.AuthorizationCode,
                            GrantType.ResourceOwnerPassword,
                            GrantType.ClientCredentials
                        },
                        AccessTokenLifetime = 3600,
                        AbsoluteRefreshTokenLifetime = 36000,
                        RequireClientSecret = false,
                        AllowAccessTokensViaBrowser = true,
                        AlwaysIncludeUserClaimsInIdToken = false,
                        IdentityTokenLifetime = 3600,
                        ClientSecrets =
                        {
                            new Secret("secret".Sha512())
                        },
                        AllowedScopes =
                        {
                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.Profile,
                            "api1"
                        },
                        RefreshTokenUsage = TokenUsage.ReUse,
                        AccessTokenType = AccessTokenType.Jwt,
                        SlidingRefreshTokenLifetime = 3600,
                        UpdateAccessTokenClaimsOnRefresh = true,
                        RefreshTokenExpiration = TokenExpiration.Sliding,
                        AllowOfflineAccess = true,
                        RedirectUris = new[] { "https://10.29.23.1:5001/" },
                        EnableLocalLogin = true,
                        ClientUri = "https://10.29.23.1:5001/",
                    }
                })
                .AddDeveloperSigningCredential(true, "key.pfx");
    // https://10.29.23.1:5001/connect/authorize?client_id=js&response_type=code&redirect_uri=https%3A%2F%2Flocalhost%3A5001%2F&scope=openid%20profile&code_verifier=6ZAUQuvaq_CFEK9vq-zlaapkgoJ3DuUslSFakKhhUMU&code_challenge_method=S256
            services.AddAuthentication(x =>
                {
                    x.RequireAuthenticatedSignIn = true;
                })
                .AddIdentityServerJwt();

            services.AddMvc().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition =
                    System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
            });
            services.AddHttpContextAccessor();
            services.AddSpaStaticFiles(configuration => configuration.RootPath = "clientapp/build");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}"
                );
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "clientapp";
                if (env.IsDevelopment())
                {
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:3000/");
                }
            });
        }
    }
}