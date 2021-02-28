using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AcrylicWindow.Client.Data
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

        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression);

        Task<IEnumerable<TEntity>> SearchAsync(string search);
    }
}
