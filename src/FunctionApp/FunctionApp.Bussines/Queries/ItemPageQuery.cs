using FunctionApp.Bussines.Models;
using FunctionApp.DataAccess.Models;

namespace FunctionApp.Bussines.Queries {
    public class ItemPageQuery : IQuery<PageResult<Item>> {
        public string Id { get; }
        public ItemPageQuery(string id) => Id = id;
    }
}