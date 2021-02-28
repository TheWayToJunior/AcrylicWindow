using AcrylicWindow.Client.Core.Model;
using AcrylicWindow.Client.Core.Models;
using System;
using System.Threading.Tasks;

namespace AcrylicWindow.Client.Core.IContract.IServices
{
    public interface IEmployeeService
    {
        Task<PaginationResult<Employee>> GetAll(int page, int pageSize, string filter = null);

        Task<Employee> GetByIdAsync(Guid id);

        Task InsertAsync(Employee model);

        Task UpdateAsync(Guid id, Employee model);

        Task DeleteAsync(Guid id);

        Task<long> CountAsync();
    }
}
