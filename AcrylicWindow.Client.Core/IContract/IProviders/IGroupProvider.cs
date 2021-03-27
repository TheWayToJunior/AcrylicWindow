using AcrylicWindow.Client.Core.Models;
using AcrylicWindow.Client.Entity;
using System;
using System.Threading.Tasks;

namespace AcrylicWindow.Client.Core.IContract
{
    public interface IGroupProvider
    {
        Task<PaginationResult<Group>> GetAllAsync(int page, int pageSize);

        Task<Group> GetByIdAsync(Guid id);

        Task InsertAsync(GroupCreate model);

        Task UpdateAsync(Guid id, GroupUpdate model);

        Task DeleteAsync(Guid id);

        internal Task Exclude(Action<IReferencesDeleteable> setReferencesAction, IGroupsReferense model);
    }
}
