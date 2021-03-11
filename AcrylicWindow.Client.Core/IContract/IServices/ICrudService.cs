using AcrylicWindow.Client.Core.Models;
using System.Threading.Tasks;

namespace AcrylicWindow.Client.Core.IContract.IServices
{
    public interface ICrudService<TModel, TKey>
        where TModel : IModel
    {
        Task<PaginationResult<TModel>> GetAll(int page, int pageSize, string filter = null);

        Task<TModel> GetByIdAsync(TKey id);

        Task InsertAsync(TModel model);

        Task UpdateAsync(TKey id, TModel model);

        Task DeleteAsync(TKey id);
    }
}
