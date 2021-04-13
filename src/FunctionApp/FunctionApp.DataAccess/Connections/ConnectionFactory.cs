using FunctionApp.DataAccess.Abstractions;
using Microsoft.Azure.Cosmos;
using System.Threading.Tasks;

namespace FunctionApp.DataAccess.Connections {
    internal sealed class ConnectionFactory : IConnectionFactory {
        private readonly CosmosClient _cosmos;
        private readonly IConnectionSettings _settings;
        public ConnectionFactory(CosmosClient cosmos,
            IConnectionSettings settings) {
            _cosmos = cosmos;
            _settings = settings;
        }

        public async Task<IConnection> GetAsync() {
            var database = await _cosmos.CreateDatabaseIfNotExistsAsync(_settings.Database);
            return new Connection(database.Database);
        }
    }
}