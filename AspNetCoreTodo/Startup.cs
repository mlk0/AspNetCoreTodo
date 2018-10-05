using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreTodo.Data;
using AspNetCoreTodo.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Proxies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreTodo {
    public class Startup {
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {
            // services.AddSingleton<ITodoItemService, FakeTodoItemService>();

            //it is important for all the service classes that are working with the database, opening connections
            //like ApplicationDbContext through the EntityFramework, to be registered 
            //in the Dependency Injection system as Scoped services instead of Singletons
            //which will ensure that an unique service instance will be created per request insted of only once when the app pull starts
            services.AddScoped<ITodoItemService, TodoItemService> ();

            services.Configure<CookiePolicyOptions> (options => {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext> (options =>
                // options.UseLazyLoadingProxies().UseSqlite(Configuration.GetConnectionString ("DefaultConnection")));
                options.UseSqlite (Configuration.GetConnectionString ("DefaultConnection")));

            // services.AddDefaultIdentity<IdentityUser> ().AddRoles<IdentityRole> ()
            //     .AddEntityFrameworkStores<ApplicationDbContext> ()
            //     .AddDefaultTokenProviders ();


// services.AddDefaultIdentity<IdentityUser>()
//     .AddRoles<IdentityRole>()
//     .AddEntityFrameworkStores<ApplicationDbContext>();

services.AddIdentity<IdentityUser, IdentityRole>()
              .AddEntityFrameworkStores<ApplicationDbContext>()
              //TODO: UPDATE IN 2.2
              //AddDefaultIdentity<IdentityUser>().AddRoles<IdentityRole>() SIMPLY DOES NOT WORK FOR 2.1
              //BUT IS FALLING BACK TO 1.1 TEMPLATE 
              //    services.AddIdentity<IdentityUser, IdentityRole>() ..AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
              //WITHOUT SPECIFYING THAT THE DEFAULT UI NEEDS TO BE USED THE APP SIMPLY FAILS TO WORK, LIKE THERE IS NO LOGIN PAGE
              .AddDefaultUI() 
              .AddDefaultTokenProviders();

              
            // services.AddIdentity<IdentityUser, IdentityRole> ()
            //     .AddEntityFrameworkStores<ApplicationDbContext> ()
            //     .AddDefaultTokenProviders ();

            services.AddMvc().SetCompatibilityVersion (CompatibilityVersion.Version_2_1);
 

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
                app.UseDatabaseErrorPage ();
            } else {
                app.UseExceptionHandler ("/Home/Error");
                app.UseHsts ();
            }

            app.UseHttpsRedirection ();
            app.UseStaticFiles ();
            app.UseCookiePolicy ();

            app.UseAuthentication ();
            

            app.UseMvc (routes => {
                routes.MapRoute (
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

        }
    }
}