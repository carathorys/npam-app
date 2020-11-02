using System;
using System.Collections.Generic;
using System.IO;
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

namespace FuryTechs.LinuxAdmin.Core {
    public class Startup {
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {
            services.AddControllersWithViews ();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles (configuration => { configuration.RootPath = "clientapp/build"; });
            services.AddHttpContextAccessor ();

            services.AddAuthentication (o => {
                    o.DefaultScheme = IdentityConstants.ApplicationScheme;
                    o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                })
                .AddIdentityCookies (o => { });

            services
                .AddLinuxIdentity ()
                .AddDefaultUI ()
                .AddDefaultTokenProviders ();

            services.AddAntiforgery ();

            services.AddIdentityServer (options => {
                    options.InputLengthRestrictions.Password = int.MaxValue;
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                })
                .AddAspNetIdentity<User> ()
                .AddInMemoryPersistedGrants ()
                .AddInMemoryIdentityResources (new IdentityResource[] {
                    new IdentityResources.OpenId (),
                        new IdentityResources.Profile (), 
                        
                })
                .AddInMemoryApiResources(new ApiResource[] {
                    new ApiResource {
                        Name = "api1",
                        Scopes = {
                            new Scope("api1")
                        }
                    }
                })
                .AddInMemoryClients (new [] {
                    new Client {
                        ClientId = "js",
                            ClientName = "JavaScript Client",
                            AllowedGrantTypes = {
                                GrantType.ResourceOwnerPassword,
                                GrantType.ClientCredentials,
                            },

                            AccessTokenLifetime = 3600,
                            AbsoluteRefreshTokenLifetime = 36000,
                            RequireClientSecret = false,
                            AllowAccessTokensViaBrowser = true,
                            AlwaysIncludeUserClaimsInIdToken = false,
                            IdentityTokenLifetime = 3600,
                            
                            ClientSecrets = {
                                 new Secret("secret".Sha256())
                            },
                            AllowedScopes = {
                                IdentityServerConstants.StandardScopes.OpenId,
                                IdentityServerConstants.StandardScopes.Profile,
                                "api1"
                            },
                            RefreshTokenUsage = TokenUsage.ReUse,
                            AccessTokenType = AccessTokenType.Jwt,
                            SlidingRefreshTokenLifetime = 3600,
                            UpdateAccessTokenClaimsOnRefresh = true,
                            RefreshTokenExpiration = TokenExpiration.Sliding,
                            AllowOfflineAccess = true
                    }
                })
                .AddDeveloperSigningCredential (true, "key.pfx");

            services.AddAuthentication ()
                .AddIdentityServerJwt ();

            services.AddControllersWithViews ();
            services.AddRazorPages ();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            } else {
                app.UseExceptionHandler ("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts ();
            }

            app.UseHttpsRedirection ();
            app.UseStaticFiles ();
            app.UseSpaStaticFiles ();

            app.UseRouting ();

            app.UseAuthentication ();
            app.UseIdentityServer ();
            app.UseAuthorization ();

            app.UseEndpoints (endpoints => {
                endpoints.MapControllerRoute (
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa (spa => {
                spa.Options.SourcePath = "clientapp";

                if (env.IsDevelopment ()) {
                    spa.UseProxyToSpaDevelopmentServer ("http://localhost:3000");
                    // spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
