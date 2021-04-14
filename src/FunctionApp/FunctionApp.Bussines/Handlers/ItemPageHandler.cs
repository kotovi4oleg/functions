using FunctionApp.Bussines.Models;
using FunctionApp.Bussines.Queries;
using FunctionApp.DataAccess.Containers;
using FunctionApp.DataAccess.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FunctionApp.Bussines.Handlers {
    public sealed class ItemPageHandler : IRequestHandler<ItemPageQuery, PageResult<Item>> {
        private readonly IItemContainer _container;
        public ItemPageHandler(IItemContainer container) => _container = container;
        public async Task<PageResult<Item>> Handle(ItemPageQuery request, CancellationToken cancellationToken) {
            var resources = _container.GetAsync(request.Id);
            var counts = _container.GetCountAsync();
            await Task.WhenAll(resources, counts);
            return new PageResult<Item> {
                Count = await counts,
                Data = await resources
            };
        }
    }
}