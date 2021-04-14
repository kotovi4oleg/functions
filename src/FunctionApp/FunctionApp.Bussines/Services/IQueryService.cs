using FunctionApp.Bussines.Queries;
using System.Threading.Tasks;

namespace FunctionApp.Bussines.Services {
    public interface IQueryService {
        Task<TEntry> QueryAsync<TEntry>(IQuery<TEntry> query);
    }
}