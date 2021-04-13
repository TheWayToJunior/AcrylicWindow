using AcrylicWindow.Client.Core.Helpers;
using AcrylicWindow.Client.Core.IContract;
using AcrylicWindow.Client.Core.IContract.IData;
using AcrylicWindow.Client.Core.IContract.IServices;
using AcrylicWindow.Client.Core.IManagers;
using AcrylicWindow.Client.Core.Models;
using System;
using System.Threading.Tasks;

namespace AcrylicWindow.Client.Core.Managers
{
    /// <summary>
    /// Decorator over IEmployeeService that performs tasks related to Group references
    /// </summary>
    public class EmployeeManager : IEmployeeManager
    {
        private readonly IReferenceExcludable _excludable;
        private readonly IEmployeeService _employeeService;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeManager(IUnitOfWork unitOfWork, IReferenceExcludable excludable, IEmployeeService employeeService)
        {
            _excludable = Has.NotNull(excludable);
            _employeeService = Has.NotNull(employeeService);
            _unitOfWork = Has.NotNull(unitOfWork);
        }

        /// <summary>
        /// Deletes as well as links to it in all groups
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(Guid id)
        {
            var entity = await _unitOfWork.Employees.GetByIdAsync(id);

            if (entity == null)
                throw new InvalidOperationException();

            await _excludable.Exclude(g => g.DeleteReferenseTeacher(), entity);
            await _employeeService.DeleteAsync(id);
        }

        public async Task<PaginationResult<Employee>> GetAll(int page, int pageSize, string filter = null)
        {
            return await _employeeService.GetAll(page, pageSize, filter);
        }

        public async Task<Employee> GetByIdAsync(Guid id)
        {
            return await _employeeService.GetByIdAsync(id);
        }

        public async Task InsertAsync(Employee model)
        {
            await _employeeService.InsertAsync(Has.NotNull(model));
        }

        public async Task UpdateAsync(Guid id, Employee model)
        {
            await _employeeService.UpdateAsync(id, Has.NotNull(model));
        }
    }
}
