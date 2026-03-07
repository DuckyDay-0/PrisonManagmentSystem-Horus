using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PMS_Horus.Data;
using PMS_Horus.Services;
using PMS_Horus.UI;
using System.Threading.Tasks;

namespace PMSHorus
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            // Dependency Injection
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddDbContext<PrisonDBContext>();
                    services.AddScoped<IPrisonerServices, PrisonerService>();
                    services.AddTransient<StartingPage>();
                    services.AddTransient<PrisonerActions>();
                    
                })
                .Build();

            // Стартирай UI
            var ui = host.Services.GetRequiredService<StartingPage>();
            await ui.RunAsync();  // Главен метод
        }
    }
}