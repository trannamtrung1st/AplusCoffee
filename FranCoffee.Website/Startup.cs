using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FranCoffee.Website.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using TNT.Core.Helpers.Cryptography;
using TNT.Core.Helpers.Smtp;

namespace FranCoffee.Website
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Configuration.Bind("App", App.Instance);
            try
            {
                var mailInfo = App.Instance.FromMailInfo;
                var parts = mailInfo.Split(' ');
                var aes = CryptoFactory.GetDefaultAesCryptor(
                    "secret", "secret");
                parts = parts.Select(v => aes.DecryptFromBase64(v)).ToArray();
                App.Instance.GmailManager = new SmtpGmailManager(parts[0], parts[1]);
            }
            catch (Exception) { }
        }

        public static IConfiguration Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CoffeeContext>(options =>
            {
                options.UseSqlServer(
                    Configuration.GetConnectionString("FranCoffeeContext"));
                options.UseLazyLoadingProxies();
            });

            services.AddDbContext<IdentityContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("FranCoffeeContext")));

            services.AddIdentity<AppUsers, AppRoles>(config =>
            {
                config.SignIn.RequireConfirmedEmail = false;
            })
            .AddEntityFrameworkStores<IdentityContext>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });

            services.ConfigureApplicationCookie(o =>
            {
                o.AccessDeniedPath = "/accessdenied";
                o.ExpireTimeSpan = TimeSpan.FromHours(1);
                o.LoginPath = "/admin/login";
                o.LogoutPath = "/admin/logout";
                o.ReturnUrlParameter = "return_url";
            });

            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization()
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AuthorizeAreaFolder("admin", "/");
                    options.Conventions.AllowAnonymousToAreaPage("admin", "/login");
                    options.Conventions.AllowAnonymousToAreaPage("admin", "/error");
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseExceptionHandler("/error");
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            #region Glo and Loc
            var supportedCultures = App.Instance.SupportedCulturesInfo;

            app.UseRequestLocalization(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(supportedCultures[0]);
                // Formatting numbers, dates, etc.
                options.SupportedCultures = supportedCultures;
                // UI strings that we have localized.
                options.SupportedUICultures = supportedCultures;

                //Sử dụng cho Route culture convention
                //options.RequestCultureProviders.Insert(0, new AppRequestCultureProvider());
            });
            #endregion


            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                RequestPath = "/uploads/*",
                OnPrepareResponse = context =>
                {
                    context.Context.Response.Headers[HeaderNames.CacheControl] =
                        "no-cache, no-store";
                    context.Context.Response.Headers[HeaderNames.Expires] = "-1";
                    context.Context.Response.Headers[HeaderNames.Pragma] = "no-cache";
                }
            });

            app.UseMvc();
        }
    }
}
