using Microsoft.Azure.Cosmos;
using System.Threading.Tasks;

namespace FunctionApp.DataAccess.Connections {
    public interface IConnection {
        Task<Container> OpenAsync(string container);
    }
}