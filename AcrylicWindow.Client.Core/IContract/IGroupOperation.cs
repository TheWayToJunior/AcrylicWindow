using AcrylicWindow.Client.Core.Models;
using System;
using System.Threading.Tasks;

namespace AcrylicWindow.Client.Core.IContract
{
    public interface IGroupOperation
    {
        Task<PaginationResult<Group>> GetAllAsync(int page, int pageSize);

        Task<Group> GetByIdAsync(Guid id);

        Task InsertAsync(GroupCreate model);

        Task UpdateAsync(Guid id, GroupUpdate model);

        Task DeleteAsync(Guid id);
    }
}
