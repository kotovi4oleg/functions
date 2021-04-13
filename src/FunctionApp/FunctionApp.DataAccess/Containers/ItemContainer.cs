using FunctionApp.DataAccess.Connections;
using FunctionApp.DataAccess.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunctionApp.DataAccess.Containers {
    internal sealed class ItemContainer : IItemContainer {
        private readonly IConnectionFactory _factory;
        public ItemContainer(IConnectionFactory factory) => _factory = factory;

        public async Task<IEnumerable<Item>> GetAsync() {
            var connection = await _factory.GetAsync();
            var container = await connection.OpenAsync("Items");
            var iterator = container.GetItemQueryIterator<Item>("SELECT * FROM c ORDER BY c._ts DESC");
            var collection = new List<Item>();
            while (iterator.HasMoreResults)
                collection.AddRange(await iterator.ReadNextAsync());

            return collection;
        }
    }
}