using FunctionApp.DataAccess.Connections;
using FunctionApp.DataAccess.Models;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace FunctionApp.DataAccess.Containers {
    internal sealed class ItemContainer : IItemContainer {
        private readonly IConnectionFactory _factory;
        public ItemContainer(IConnectionFactory factory) => _factory = factory;

        public async Task<IEnumerable<Item>> GetAsync(string id = null) {
            var connection = await _factory.GetAsync();
            var container = await connection.OpenAsync("Items");
            var iterator = container.GetItemQueryIterator<Item>(Query(id));
            var collection = new List<Item>();
            while (iterator.HasMoreResults)
                collection.AddRange(await iterator.ReadNextAsync());

            return collection;
        }

        private static QueryDefinition Query(string id) {
            var builder = new StringBuilder("SELECT * FROM c");

            if (string.IsNullOrEmpty(id))
                return new QueryDefinition(builder.Append(" ORDER BY c._ts DESC").ToString());

            return new QueryDefinition(builder.Append(" WHERE c.id = @id").ToString()).WithParameter("@id", id);
        }
    }
}