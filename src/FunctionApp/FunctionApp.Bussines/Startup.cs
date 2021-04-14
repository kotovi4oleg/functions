using FunctionApp.Bussines.Services;
using FunctionApp.DataAccess;
using FunctionApp.DataAccess.Abstractions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FunctionApp.Bussines {
    public static class Startup {
        public static void ConfigureServices(this IServiceCollection collection, IConnectionSettings settings) {
            collection
                .AddCosmos(settings)
                .AddScoped<IQueryService, QueryService>()
                .AddMediatR(new[] { typeof(Startup).Assembly }, configuration => configuration.AsScoped());
        }
    }
}