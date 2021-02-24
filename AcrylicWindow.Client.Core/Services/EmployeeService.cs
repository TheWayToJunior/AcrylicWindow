using AcrylicWindow.Client.Core.Helpers;
using AcrylicWindow.Client.Core.IContract.IServices;
using AcrylicWindow.Client.Core.Model;
using AcrylicWindow.Client.Data;
using AcrylicWindow.Client.Data.Entities;
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

        public async Task<long> CountAsync() => await _repository.CountAsync();

        public async Task<IEnumerable<Employee>> GetAllAsync(int page, int pageSize) =>
            (await _repository
                .GetAllAsync(page, pageSize))
                .Select(e => _mapper.Map<Employee>(e));


        public async Task<Employee> GetByIdAsync(Guid id) =>
            _mapper.Map<Employee>(
                await _repository.GetByIdAsync(id)
                ?? throw new InvalidOperationException($"Object with this id: {id}, was not found"));

        public async Task InsertAsync(Employee model)
        {
            var entity = _mapper.Map<EmployeeEntity>(Has.NotNull(model));
            await _repository.InsertAsync(entity);
        }

        public async Task UpdateAsync(Guid id, Employee model)
        {
            var entity = _mapper.Map<EmployeeEntity>(Has.NotNull(model));
            await _repository.InsertAsync(entity);
        }

        public async Task DeleteAsync(Guid id) => await _repository.DeleteAsync(id);
    }
}
