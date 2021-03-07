using AcrylicWindow.Client.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcrylicWindow.Client.Core.IContract.IData
{
    public interface IRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        Task<long> CountAsync();

        Task DeleteAsync(TKey id);

        IQueryable<TEntity> GetAll();

        Task<TEntity> GetByIdAsync(TKey id);

        Task InsertAsync(TEntity model);

        Task UpdateAsync(TKey id, TEntity model);

        Task<IEnumerable<TEntity>> SearchAsync(string search);
    }
}
