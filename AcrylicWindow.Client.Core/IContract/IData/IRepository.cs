using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AcrylicWindow.Client.Data
{
    public interface IRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        Task DeleteAsync(TKey id);

        Task<IEnumerable<TEntity>> GetAllAsync(int page, int pageSize);

        Task<TEntity> GetByIdAsync(TKey id);

        Task InsertAsync(TEntity model);

        Task UpdateAsync(TKey id, TEntity model);

        Task<IEnumerable<TEntity>> FindAsync<TValue>(Expression<Func<TEntity, TValue>> expression, TValue value);
    }
}
