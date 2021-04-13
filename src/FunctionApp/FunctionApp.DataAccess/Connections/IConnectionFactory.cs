using System.Threading.Tasks;

namespace FunctionApp.DataAccess.Connections {
    public interface IConnectionFactory {
        Task<IConnection> GetAsync();
    }
}