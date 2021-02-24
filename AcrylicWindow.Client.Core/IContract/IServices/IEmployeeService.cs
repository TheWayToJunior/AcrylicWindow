using AcrylicWindow.Client.Core.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcrylicWindow.Client.Core.IContract.IServices
{
    public interface IEmployeeService
    {
        Task<long> CountAsync();

        Task<IEnumerable<Employee>> GetAllAsync(int page, int pageSize);

        Task<Employee> GetByIdAsync(Guid id);

        Task InsertAsync(Employee model);

        Task UpdateAsync(Guid id, Employee model);

        Task DeleteAsync(Guid id);
    }
}
