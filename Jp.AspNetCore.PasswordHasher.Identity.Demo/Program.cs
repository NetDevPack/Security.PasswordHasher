using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using Jp.AspNetCore.PasswordHasher.Identity.Demo.Config;

namespace Jp.AspNetCore.PasswordHasher.Identity.Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var host = CreateWebHostBuilder(args).Build();

            Task.WaitAll(DbMigrationHelpers.EnsureSeedData(host));

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
