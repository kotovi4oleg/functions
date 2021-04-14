using FunctionApp.DataAccess.Connections;
using FunctionApp.DataAccess.Models;
using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionApp.DataAccess.Containers {
    internal sealed class ItemContainer : IItemContainer {
        private readonly IConnectionFactory _factory;
        public ItemContainer(IConnectionFactory factory) => _factory = factory;

        public async Task<IEnumerable<Item>> GetAsync(string id = null) {
            var connection = await _factory.GetAsync();
            var container = await connection.OpenAsync("Items");
            var collection = new List<Item>();
            using var iterator = Query(container, id);
            while (iterator.HasMoreResults) {
                var next = await iterator.ReadNextAsync();
                collection.AddRange(next);
            }

            return collection;
        }

        private static FeedIterator<Item> Query(Container container, string id) {
            var builder = new StringBuilder("SELECT * FROM c");

            return string.IsNullOrEmpty(id)
                ? container.GetItemQueryIterator<Item>(new QueryDefinition(builder.Append(" ORDER BY c._ts DESC").ToString()))
                : container.GetItemQueryIterator<Item>(new QueryDefinition(builder.Append(" WHERE c.id = @id").ToString()).WithParameter("@id", id), requestOptions: new QueryRequestOptions {
                    PartitionKey = new PartitionKey(id)
                });
        }

        public async Task<int> GetCountAsync() {
            var connection = await _factory.GetAsync();
            var container = await connection.OpenAsync("Items");
            using var iterator = container.GetItemQueryIterator<int>(new QueryDefinition("SELECT VALUE COUNT(1) FROM c"), requestOptions: new QueryRequestOptions {
                MaxItemCount = 1
            });
            return (await iterator.ReadNextAsync()).Resource.Single();
        }
    }
}