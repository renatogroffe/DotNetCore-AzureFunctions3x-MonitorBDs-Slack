using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using MonitoramentoBasesSql.Clients;

[assembly: FunctionsStartup(typeof(MonitoramentoBasesSql.Startup))]
namespace MonitoramentoBasesSql
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient<CanalSlackClient>();
        }
    }
}