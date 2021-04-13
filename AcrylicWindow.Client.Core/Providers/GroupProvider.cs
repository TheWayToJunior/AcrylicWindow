using AcrylicWindow.Client.Core.Helpers;
using AcrylicWindow.Client.Core.IContract;
using AcrylicWindow.Client.Core.IContract.IData;
using AcrylicWindow.Client.Core.IContract.IServices;
using AcrylicWindow.Client.Core.Models;
using AcrylicWindow.Client.Entity.Entities;
using AutoMapper;
using System;
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

        public async Task<PaginationResult<Group>> GetAllAsync(int page, int pageSize)
        {
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
            var model = _mapper.Map<Group>(Has.NotNull(entity));

            if (entity.TeacherId != default)
            {
                var teacher = await _employeeService.GetByIdAsync(entity.TeacherId);
                model.Teacher = teacher;
            }

            entity.StudentsIds?.SelectAsync(async id =>
            {
                Has.NotNull(model.Students).Add(await _studentService.GetByIdAsync(id));
                return id;
            });

            return model;
        }

        public async Task InsertAsync(GroupCreate model)
        {
            var entity = _mapper.Map<GroupEntity>(Has.NotNull(model));

            await _unitOfWork.Groups.InsertAsync(entity);
        }

        public async Task UpdateAsync(Guid id, GroupUpdate model)
        {
            await _unitOfWork.Groups.UpdateAsync(id, _mapper.Map<GroupEntity>(model));
        }

        public async Task DeleteAsync(Guid id)
        {
            await _unitOfWork.Groups.DeleteAsync(id);
        }
    }
}
