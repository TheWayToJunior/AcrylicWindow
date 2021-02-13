using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcrylicWindow.Client.Data
{
    public interface IRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        Task<IEnumerable<TEntity>> GetAllAsync(string table);

        Task<TEntity> GetByIdAsync(string table, TKey id);

        Task InsertAsync(string table, TEntity entity);

        Task UpdateAsync(string table, TKey id, IEntity<TKey> entity);

        Task DeleteAsync(string table, TKey id);
    }
}
