using AcrylicWindow.Client.Core.Models;
using System;
using System.Threading.Tasks;

namespace AcrylicWindow.Client.Core.IContract.IServices
{
    public interface IEmployeeService : ICrudService<Employee, Guid>
    {
        Task<long> CountAsync();
    }
}
