using AcrylicWindow.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcrylicWindow.IContract
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetAllAsync();

        Task DeleteAsync(int id);
    }
}
