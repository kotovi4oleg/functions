using FunctionApp;
using FunctionApp.DataAccess;
using FunctionApp.DataAccess.Abstractions;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

[assembly: FunctionsStartup(typeof(Startup))]
namespace FunctionApp {
    public class Startup : FunctionsStartup {
        public override void Configure(IFunctionsHostBuilder builder) {
            var settings = new ConnectionSettings();
            builder.GetContext().Configuration.GetSection("Cosmos").Bind(settings);
            builder.Services.AddCosmos(settings);
        }
    }
}