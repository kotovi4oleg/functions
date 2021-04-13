using Microsoft.Azure.Cosmos;
using System.Threading.Tasks;

namespace FunctionApp.DataAccess.Connections {
    internal sealed class Connection : IConnection {
        private readonly Database _database;
        public Connection(Database database) => _database = database;
        public async Task<Container> OpenAsync(string container) => await _database.CreateContainerIfNotExistsAsync(container, "/id");
    }
}