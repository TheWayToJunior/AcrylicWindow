using AcrylicWindow.Client.Core.Helpers;
using AcrylicWindow.Client.Core.IContract;
using AcrylicWindow.Client.Core.IContract.IData;
using AcrylicWindow.Client.Core.IContract.IManagers;
using AcrylicWindow.Client.Core.Models;
using AutoMapper;
using System;
using System.Threading.Tasks;

namespace AcrylicWindow.Client.Core.Managers
{
    public class GroupManager : IGroupManager
    {
        private readonly IGroupProvider _groupProvider;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GroupManager(IGroupProvider groupProvider, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _groupProvider = Has.NotNull(groupProvider);
            _unitOfWork = Has.NotNull(unitOfWork);
            _mapper = Has.NotNull(mapper);
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _unitOfWork.Groups.GetByIdAsync(id);

            if (entity == null)
            {
                return;
            }

            await _unitOfWork.SetAllReferences(entity, (@ref, id) => @ref.Groups.Remove(id));
            await _unitOfWork.Groups.DeleteAsync(id);
        }

        public async Task UpdateAsync(Guid id, GroupUpdate model)
        {
            Has.NotNull(model);

            if (!id.Equals(model.Id))
            {
                throw new InvalidOperationException($"The IDs don't match: {id} and {model.Id}");
            }

            var entity = await _unitOfWork.Groups.GetByIdAsync(model.Id);

            if (entity == null)
            {
                throw new InvalidOperationException($"Missing id specified: {id}");
            }

            /// This is not a bug but a feature:
            /// When you change the reference to the group in the Student and Employee objects is not deleted so you have to delete it manually
            await _unitOfWork.SetAllReferences(DeletedGroupReference.GetInstance(entity, model));

            /// Adding new links received from the UI
            await _unitOfWork.SetAllReferences(InsertedGroupReference.GetInstance(entity, model));
            await _unitOfWork.Groups.UpdateAsync(id, _mapper.Map(model, entity));
        }

        public async Task<PaginationResult<Group>> GetAllAsync(int page, int pageSize)
        {
            return await _groupProvider.GetAllAsync(page, pageSize);
        }

        public async Task<Group> GetByIdAsync(Guid id)
        {
            return await _groupProvider.GetByIdAsync(id);
        }

        public async Task InsertAsync(GroupCreate model)
        {
            await _groupProvider.InsertAsync(model);
        }
    }
}
