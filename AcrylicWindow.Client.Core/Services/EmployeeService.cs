using AcrylicWindow.Client.Core.Helpers;
using AcrylicWindow.Client.Core.IContract.IData;
using AcrylicWindow.Client.Core.IContract.IServices;
using AcrylicWindow.Client.Core.Models;
using AcrylicWindow.Client.Entity.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcrylicWindow.Client.Core.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;
        private readonly IMapper _mapper;

        public EmployeeService(IUnitOfWork dataProvider, IMapper mapper)
        {
            _repository = Has.NotNull(dataProvider).Employees;
            _mapper = Has.NotNull(mapper);
        }

        public async Task<Employee> GetByIdAsync(Guid id) =>
            _mapper.Map<Employee>(
                await _repository.GetByIdAsync(id)
                ?? throw new InvalidOperationException($"Object with this id: {id}, was not found"));

        /// <summary>
        /// Will give records, dividing them into pages and filtering them if necessary
        /// </summary>
        /// <param name="page">The number of the desired page</param>
        /// <param name="pageSize">Number of entries per page</param>
        /// <param name="search">Search filter</param>
        /// <returns></returns>
        public async Task<PaginationResult<Employee>> GetAll(int page, int pageSize, string search = null)
        {
            var entitys = string.IsNullOrEmpty(search) ? _repository.GetAll() : await _repository.SearchAsync(search);

            return new PaginationResult<Employee>
            {
                TotalCount = entitys.Count(),
                Values = _mapper.Map<IEnumerable<Employee>>(entitys
                    .Pagination(page, pageSize))
            };
        }

        public async Task InsertAsync(Employee model)
        {
            var entity = _mapper.Map<EmployeeEntity>(Has.NotNull(model));

            await _repository.InsertAsync(entity);
        }

        public async Task UpdateAsync(Guid id, Employee model)
        {
            var entity = _mapper.Map<EmployeeEntity>(Has.NotNull(model));

            await _repository.UpdateAsync(id, entity);
        }

        public async Task DeleteAsync(Guid id) => await _repository.DeleteAsync(id);

        public async Task<long> CountAsync() => await _repository.CountAsync();
    }
}
