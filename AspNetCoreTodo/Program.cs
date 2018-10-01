using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspNetCoreTodo {
    public class Program {
        public static void Main (string[] args) {
            var webHost = CreateWebHostBuilder (args).Build ();
            InitializeDatabase (webHost);
            webHost.Run ();
        }

        private static void InitializeDatabase (IWebHost webHost) {
            using (var serviceScope = webHost.Services.CreateScope ()) {
                var serviceProvider = serviceScope.ServiceProvider;
                var logger = serviceProvider.GetRequiredService<ILogger<Program>> ();
                logger.LogDebug ("in InitializeDatabase #################################");

                SeedData.InitializeAsync (serviceProvider).Wait ();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder (string[] args) =>
            WebHost.CreateDefaultBuilder (args)
            .UseStartup<Startup> ();
    }

    internal static class SeedData {
        public async static Task InitializeAsync (IServiceProvider serviceProvider) {
            //var logger = serviceProvider.GetRequiredService<ILogger<SeedData>>();

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>> ();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>> ();

            await CreateAdminRole (roleManager);
            await CreateAdminUser (userManager);
        }

        private async static Task CreateAdminUser (UserManager<IdentityUser> userManager) {
            var adminUser = await userManager.Users.FirstOrDefaultAsync (c => c.Email == Constants.AdministratorEmail);
            if (adminUser == null) {
                adminUser = new IdentityUser () {
                    Email = Constants.AdministratorEmail,
                        //probably it;s some bug but if the UserName is matching the Email, once the Admin account is created it fails on login
                        //on the other hand, the UI expects an input in a form of a valid email and this prevents passing the actual UserName
                        UserName = Constants.AdministratorEmail
                };

                await userManager.CreateAsync (adminUser, "TempPass123!@#");

                await userManager.AddToRoleAsync (adminUser, Constants.Administrator);

            }

        }

        private async static Task CreateAdminRole (RoleManager<IdentityRole> roleManager) {
            var administratorRoleExists = await roleManager.RoleExistsAsync (Constants.Administrator);

            if (!administratorRoleExists) {
                await roleManager.CreateAsync (new IdentityRole (Constants.Administrator));
            }
        }
    }

    internal static class Constants {
        public const string Administrator = "Admin";
        public const string AdministratorEmail = "mirko.atanasov@gmail.com";

        const string RegisteredUser = "Registered";

        const string GuesUser = "Guest";

    }
}