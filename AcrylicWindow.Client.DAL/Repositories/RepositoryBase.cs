using AcrylicWindow.Client.Core.Helpers;
using AcrylicWindow.Client.Data;
using AcrylicWindow.Client.Data.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AcrylicWindow.Client.DAL.Repositories
{
    public abstract class RepositoryBase<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : EntityBase<TKey>, new()
    {
        private readonly string _tableName;

        protected readonly IMongoDatabase Database;

        public RepositoryBase(IMongoDatabase database, string tableName)
        {
            Database = Has.NotNull(database);

            _tableName = Has.NotNullOrEmpty(tableName);
        }

        public virtual async Task<long> CountAsync() => 
            await Database.GetCollection<TEntity>(_tableName).CountDocumentsAsync(Builders<TEntity>.Filter.Empty);

        /// <summary>
        /// Gets a record in the data database divided by pages
        /// </summary>
        /// <typeparam name="TModel">The model that the initial entity will be converted to</typeparam>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Number of entries per page</param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(int page, int pageSize)
        {
            var collection = Database.GetCollection<TEntity>(_tableName);

            using (var entiteis = await collection.FindAsync(Builders<TEntity>.Filter.Empty))
            {
                /// Divides the selected records by page
                return entiteis.ToList()
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize);
            }
        }

        public async Task<TEntity> GetByIdAsync(TKey id)
        {
            var collection = Database.GetCollection<TEntity>(_tableName);
            var filter = Builders<TEntity>.Filter.Eq("Id", id);

            using (var entiteis = await collection.FindAsync(filter))
            {
                return await entiteis.FirstOrDefaultAsync();
            }
        }

        public virtual async Task InsertAsync(TEntity entity)
        {
            var collection = Database.GetCollection<TEntity>(_tableName);
            entity.CreatedBy = entity.UpdatedBy = DateTime.Now;

            await collection.InsertOneAsync(entity);
        }

        /// <summary>
        /// Updates a record in the database,
        /// if the record cannot be found by the specified id, a new one will be added
        /// </summary>
        public virtual async Task UpdateAsync(TKey id, TEntity entity)
        {
            var collection = Database.GetCollection<TEntity>(_tableName);
            var filter = Builders<TEntity>.Filter.Eq("Id", id);

            /// We need to update all the files except CreatedBy, which is not possible at the database level.
            /// Let's do it manually
            var foundEntity = await GetByIdAsync(id);

            /// If the record was not found in the database, it will be created again
            entity.CreatedBy = foundEntity?.CreatedBy ?? DateTime.Now;
            entity.UpdatedBy = DateTime.Now;

            var result = await collection.ReplaceOneAsync(filter, entity,
                new ReplaceOptions { IsUpsert = true });
        }

        public virtual async Task DeleteAsync(TKey id)
        {
            var collection = Database.GetCollection<TEntity>(_tableName);
            var filter = Builders<TEntity>.Filter.Eq("Id", id);

            await collection.DeleteOneAsync(filter);
        }

        /// <summary>
        /// Searches for records by the specified property
        /// </summary>
        /// <param name="expression">An expression that specifies the property that will be searched for</param>
        /// <param name="value">The value to search for</param>
        public virtual async Task<IEnumerable<TEntity>> FindAsync<TValue>(Expression<Func<TEntity, TValue>> expression, TValue value)
        {
            var memberExpression = expression.Body as MemberExpression;

            if (memberExpression == null)
                throw new InvalidOperationException();

            var collection = Database.GetCollection<TEntity>(_tableName);
            var filter = Builders<TEntity>.Filter.Eq(memberExpression.Member.Name, value);

            using (var entiteis = await collection.FindAsync(filter))
            {
                return entiteis.ToList();
            }
        }
    }
}
