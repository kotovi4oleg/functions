using FunctionApp.DataAccess.Abstractions;
using FunctionApp.DataAccess.Connections;
using FunctionApp.DataAccess.Containers;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.DependencyInjection;

namespace FunctionApp.DataAccess {
    public static class Bootstrapper {
        public static IServiceCollection AddCosmos(this IServiceCollection collection, IConnectionSettings settings) {
            collection.AddSingleton(service => new CosmosClientBuilder(settings.Uri, settings.PrimaryKey)
                .WithSerializerOptions(new CosmosSerializationOptions {
                    IgnoreNullValues = true,
                    PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                }).Build());
            collection.AddSingleton(settings);
            collection.AddScoped<IConnectionFactory, ConnectionFactory>();
            collection.AddScoped<IItemContainer, ItemContainer>();
            return collection;
        }
    }
}