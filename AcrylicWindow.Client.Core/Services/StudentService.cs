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
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _repository;
        private readonly IMapper _mapper;

        public StudentService(IUnitOfWork dataProvider, IMapper mapper)
        {
            _repository = Has.NotNull(dataProvider).Students;
            _mapper = Has.NotNull(mapper);
        }

        public async Task<PaginationResult<Student>> GetAll(int page, int pageSize, string filter = null)
        {
            var entitys = string.IsNullOrEmpty(filter) ? _repository.GetAll() : await _repository.SearchAsync(filter);

            return new PaginationResult<Student>
            {
                TotalCount = entitys.Count(),
                Values = _mapper.Map<IEnumerable<Student>>(entitys
                    .Pagination(page, pageSize))
            };
        }

        public async Task<Student> GetByIdAsync(Guid id) =>
            _mapper.Map<Student>(
                await _repository.GetByIdAsync(id)
                ?? throw new InvalidOperationException($"Object with this id: {id}, was not found"));

        public async Task<Student> SingleOrDefaultAsync(Guid id) =>
            _mapper.Map<Student>(await _repository.GetByIdAsync(id));

        public async Task InsertAsync(Student model)
        {
            var entity = _mapper.Map<StudentEntity>(Has.NotNull(model));

            await _repository.InsertAsync(entity);
        }

        public async Task UpdateAsync(Guid id, Student model)
        {
            var entity = _mapper.Map<StudentEntity>(Has.NotNull(model));

            await _repository.UpdateAsync(id, entity);
        }

        public async Task DeleteAsync(Guid id) => await _repository.DeleteAsync(id);
    }
}
