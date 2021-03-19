using AcrylicWindow.Client.Core.Models;
using System;
using System.Threading.Tasks;

namespace AcrylicWindow.Client.Core.IContract.IProviders
{
    public interface IGroupProvider
    {
        Task<PaginationResult<Group>> GetAllAsync(int page, int pageSize, string filter = null);

        Task<Group> GetByIdAsync(Guid id);

        Task InsertAsync(Group model);

        Task UpdateAsync(Guid id, Group model);

        Task DeleteAsync(Guid id);
    }
}
