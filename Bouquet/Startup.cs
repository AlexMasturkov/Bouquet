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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Bouquet.DataAccess.Data;
using Bouquet.DataAccess.Repository.IRepository;
using Bouquet.DataAccess.Repository;
using Microsoft.AspNetCore.Identity.UI.Services;
using Bouquet.Utility;
using Stripe;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Bouquet.DataAccess.Initializer;

namespace Bouquet
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
            services.AddDbContext<ApplicationDbContext>(options =>options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));           
            services.AddIdentity<IdentityUser,IdentityRole>(opt=> {
                opt.SignIn.RequireConfirmedEmail = true;
                 opt.SignIn.RequireConfirmedAccount = true; }).AddDefaultTokenProviders().
                AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();
            services.Configure<EmailOptions>(Configuration);
            services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));
            services.Configure<TwilioSettings>(Configuration.GetSection("Twilio"));
            services.Configure<BrainTreeSettings>(Configuration.GetSection("BrainTree"));
            services.AddSingleton<IBrainTreeGate, BrainTreeGate>();
            services.AddScoped<IUnitOfWork,UnitOfWork>();
            services.AddScoped<IDbInitializer, DbInitializer>();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddRazorPages();
            services.AddServerSideBlazor();         

            services.ConfigureApplicationCookie(options =>
            {               
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });
            services.AddAuthentication().AddFacebook(options =>
            {
                options.AppId = "3455988557750675";
                options.AppSecret = "596dc72a7ea946857409d4b460218a29";
            });
            services.AddAuthentication().AddGoogle(options =>
            {
                options.ClientId = "719585898384-p909jf35t5m4r2t5j886t85vll3rkrin.apps.googleusercontent.com";
                options.ClientSecret = "sb9SuS6D9nLVtpg17EocqrEu";
            });
            services.AddSession(options =>{
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDbInitializer dbInitializer)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();            
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            StripeConfiguration.ApiKey = Configuration.GetSection("Stripe")["SecretKey"];
            app.UseSession();
            app.UseCookiePolicy();          
            app.UseAuthentication();
            app.UseAuthorization();
            dbInitializer.Inizialize();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
                endpoints.MapBlazorHub();
            });
        }
    }
}
