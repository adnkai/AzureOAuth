using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using MultiWebApp.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication.Twitter;
using Microsoft.AspNetCore.Authentication.Google;
namespace MultiWebApp
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
                    
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            
            services.AddAuthentication(AzureADDefaults.AuthenticationScheme)
                .AddMicrosoftAccount(microsoftOptions => {Configuration.Bind("Authentication:Microsoft", microsoftOptions);})
                .AddAzureAD(options => {
                        Configuration.Bind("Authentication:AzureAd", options);
                        options.CookieSchemeName = IdentityConstants.ExternalScheme;
                    })
                .AddTwitter(twitterOptions => {
                        Configuration.Bind("Authentication:Twitter", twitterOptions);
                    })
                .AddGoogle(googleOptions => {
                        Configuration.Bind("Authentication:Google", googleOptions);
                });
            
            // .AddCookie(options => {
            //             options.LoginPath = "/Index/";
            //             options.AccessDeniedPath = "/Account/Forbidden/";
            //     })
            // .AddJwtBearer(options => {
            //     options.Audience = "http://localhost:5001/Index";
            //     options.Authority = "https://login.microsoftonline.com/";
            //     options.RequireHttpsMetadata = false;
            // });
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
