using FunctionApp.DataAccess.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunctionApp.DataAccess.Containers {
    public interface IItemContainer {
        Task<IEnumerable<Item>> GetAsync();
    }
}