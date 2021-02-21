using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AcrylicWindow.Client.Data
{
    public interface IRepository<out TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        Task DeleteAsync(TKey id);

        Task<IEnumerable<TModel>> GetAllAsync<TModel>(int page, int pageSize);

        Task<TModel> GetByIdAsync<TModel>(TKey id);

        Task InsertAsync<TModel>(TModel model);

        Task UpdateAsync<TModel>(TKey id, TModel model);

        Task<IEnumerable<TModel>> FindAsync<TModel, TValue>(Expression<Func<TModel, TValue>> expression, TValue value);
    }
}
