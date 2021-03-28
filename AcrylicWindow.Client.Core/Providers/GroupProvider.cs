using AcrylicWindow.Client.Core.Helpers;
using AcrylicWindow.Client.Core.IContract;
using AcrylicWindow.Client.Core.IContract.IData;
using AcrylicWindow.Client.Core.IContract.IServices;
using AcrylicWindow.Client.Core.Models;
using AcrylicWindow.Client.Entity;
using AcrylicWindow.Client.Entity.Entities;
using AutoMapper;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AcrylicWindow.Client.Core.Providers
{
    public class GroupProvider : IGroupProvider, IReferenceExcludable
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
            Has.NotNull(model);

            if (!id.Equals(model.Id))
            {
                throw new InvalidOperationException($"The IDs don't match: {id} and {model.Id}");
            }

            var entity = _mapper.Map<GroupEntity>(model);

            await _unitOfWork.SetAllReferences(entity, (@ref, id) => @ref.Groups.Add(id));
            await _unitOfWork.Groups.UpdateAsync(id, entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _unitOfWork.Groups.GetByIdAsync(id);

            if (entity == null)
            {
                throw new InvalidOperationException($"Missing id specified: {id}");
            }

            await _unitOfWork.SetAllReferences(entity, (@ref, id) => @ref.Groups.Remove(id));
            await _unitOfWork.Groups.DeleteAsync(id);
        }

        /// <summary>
        /// Deletes installed links in groups
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="referencesAction"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task Exclude(Action<IReferencesDeleteable> referencesAction, IGroupsReferense model)
        {
            Has.NotNull(model);

            foreach (var id in model.Groups)
            {
                var groupEntity = await _unitOfWork.Groups.GetByIdAsync(id);

                if (groupEntity == null)
                {
                    throw new InvalidOperationException($"Missing id specified: {id}");
                }

                referencesAction?.Invoke(groupEntity);
                await _unitOfWork.Groups.UpdateAsync(id, groupEntity);
            }
        }
    }
}
