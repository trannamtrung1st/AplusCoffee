using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FranCoffee.Website
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //dotnet ef dbcontext scaffold "Server=localhost;Database=FranCoffee;Trusted_Connection=False;User Id=sa;Password=123456" Microsoft.EntityFrameworkCore.SqlServer -o Models -c CoffeeContext --project FranCoffee.Website --force
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
