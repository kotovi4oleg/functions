using FunctionApp.Bussines.Queries;
using MediatR;
using System.Threading.Tasks;

namespace FunctionApp.Bussines.Services {
    public class QueryService : IQueryService {
        private readonly IMediator _mediator;
        public QueryService(IMediator mediator) => _mediator = mediator;
        public async Task<TEntry> QueryAsync<TEntry>(IQuery<TEntry> query) => await _mediator.Send(query);
    }
}