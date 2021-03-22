using AcrylicWindow.Client.Core.Helpers;
using AcrylicWindow.Client.Core.IContract.IData;
using AcrylicWindow.Client.Core.IContract;
using AcrylicWindow.Client.Core.IContract.IServices;
using AcrylicWindow.Client.Core.Models;
using AcrylicWindow.Client.Entity.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcrylicWindow.Client.Core.Providers
{
    public class GroupProvider : IGroupProvider
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStudentService _studentService;
        private readonly IEmployeeService _employeeService;

        private readonly IMapper _mapper;

        public GroupProvider(IUnitOfWork unitOfWork, IStudentService studentService, IEmployeeService employeeService, 
            IMapper mapper)
        {
            _unitOfWork = Has.NotNull(unitOfWork);
            _studentService = Has.NotNull(studentService);
            _employeeService = Has.NotNull(employeeService);

            _mapper = Has.NotNull(mapper);
        }

        public async Task<PaginationResult<Group>> GetAllAsync(int page, int pageSize, string filter = null)
        {
            /// ToDo : Figure out what to do with the search

            var groups = await _unitOfWork.Groups
                .GetAll()
                .Pagination(page, pageSize)
                .SelectAsync(async (group) => await MapToModel(group));

            return new PaginationResult<Group>
            {
                TotalCount = await _unitOfWork.Groups.CountAsync(),
                Values = groups.ToList()
            };
        }

        public async Task<Group> GetByIdAsync(Guid id) => await MapToModel(await _unitOfWork.Groups.GetByIdAsync(id));

        private async Task<Group> MapToModel(GroupEntity entity)
        {
            Has.NotNull(entity);

            var teacher = await _employeeService.GetByIdAsync(entity.TeacherId);
            var students = new List<Student>();

            foreach (var studentId in entity.StudentsIds)
            {
                students.Add(await _studentService.GetByIdAsync(studentId));
            }

            var model = _mapper.Map<Group>(entity);
            model.Teacher = teacher;
            model.Students = students;

            return model;
        }

        public async Task InsertAsync(GroupCreate model)
        {
            var entity = _mapper.Map<GroupEntity>(Has.NotNull(model));

            await _unitOfWork.Groups.InsertAsync(entity);
        }

        public async Task UpdateAsync(Guid id, GroupUpdate model)
        {
            Has.NotNull(model);

            if (!id.Equals(model.Id))
            {
                throw new InvalidOperationException();
            }

            var entity = _mapper.Map<GroupEntity>(model);
            await _unitOfWork.Groups.UpdateAsync(id, entity);
        }

        public async Task DeleteAsync(Guid id) => await _unitOfWork.Groups.DeleteAsync(id);
    }
}
