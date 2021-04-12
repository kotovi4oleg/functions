using FunctionApp;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]
namespace FunctionApp {
    public class Startup : FunctionsStartup {
        public override void Configure(IFunctionsHostBuilder builder) {
            builder.Services.AddSingleton(service => {
                var client = new CosmosClientBuilder("https://cosmos-aka-data.documents.azure.com:443/",
                    "HwAbioeggYX1KSXad4zaFW2RPX7BnRAMzpoaHrNu8PD0Vtmldbh0ku7WNusMLOAbdqXADuzQJYw1we8SL11TWA==");
                return client
                    .WithSerializerOptions(new CosmosSerializationOptions {
                        PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                    })
                    .Build();
            });
        }
    }
}