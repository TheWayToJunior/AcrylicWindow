using AcrylicWindow.Client.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcrylicWindow.Client.Core.IContract
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetAllAsync();

        Task DeleteAsync(int id);
    }
}
