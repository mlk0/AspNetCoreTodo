using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspNetCoreTodo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var webHost = CreateWebHostBuilder(args).Build();
            InitializeDatabase(webHost);
            webHost.Run();
        }

        private static void InitializeDatabase(IWebHost webHost)
        {
            using(var serviceScope = webHost.Services.CreateScope()){
                var serviceProvider = serviceScope.ServiceProvider;
                var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
                logger.LogDebug("in InitializeDatabase #################################");
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
