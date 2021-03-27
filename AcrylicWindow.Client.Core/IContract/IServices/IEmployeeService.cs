using AcrylicWindow.Client.Core.Models;
using System;
using System.Threading.Tasks;

namespace AcrylicWindow.Client.Core.IContract.IServices
{
    public interface IEmployeeService : IOperationBase<Employee, Guid>
    {
        Task<Employee> SingleOrDefaultAsync(Guid id);

        Task<long> CountAsync();
    }
}
