using MediatR;

namespace FunctionApp.Bussines.Queries {
    public interface IQuery<TResponse> : IRequest<TResponse> {

    }
}