using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PMS_Horus.Data;
using PMS_Horus.Interfaces;
using PMS_Horus.Services;
using PMS_Horus.UI;
using PMS_Horus.UI.GetUI;
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
                    services.AddScoped<IPrisonerExtensionServices, PrisonerExtensionServices>();
                    services.AddScoped<IBehaviorRecordService, BehaviorRecordService>();
                    services.AddTransient<StartingPage>();
                    services.AddTransient<PrisonerActions>();
                    services.AddTransient<PrisonerExtensionActions>();
                    services.AddTransient<GetPrisonerByUI>();
                    services.AddTransient<BehaviorRecordActions>();
                    
                })
                .Build();

            // Стартирай UI
            var ui = host.Services.GetRequiredService<StartingPage>();
            await ui.RunAsync();  // Главен метод
        }
    }
}