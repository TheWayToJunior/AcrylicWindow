using AcrylicWindow.Client.Core.Helpers;
using AcrylicWindow.Client.Data;
using AcrylicWindow.Client.Data.Entities;
using AutoMapper;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AcrylicWindow.Client.DAL.Repositories
{
    public abstract class RepositoryBase<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : EntityBase<TKey>
    {
        private readonly string _tableName;

        protected readonly IMongoDatabase Database;
        private readonly IMapper _mapper;

        public RepositoryBase(IMongoDatabase database, IMapper mapper, string tableName)
        {
            _tableName = Has.NotNullOrEmpty(tableName);
            _mapper = Has.NotNull(mapper);

            Database = Has.NotNull(database);
        }

        /// <summary>
        /// Gets a record in the data database divided by pages
        /// </summary>
        /// <typeparam name="TModel">The model that the initial entity will be converted to</typeparam>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Number of entries per page</param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<TModel>> GetAllAsync<TModel>(int page, int pageSize)
        {
            var collection = Database.GetCollection<TEntity>(_tableName);

            using (var entiteis = await collection.FindAsync(Builders<TEntity>.Filter.Empty))
            {
                /// Divides the selected records by page
                return entiteis.ToList()
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(e => _mapper.Map<TEntity, TModel>(e));
            }
        }

        public virtual async Task<TModel> GetByIdAsync<TModel>(TKey id) => 
            _mapper.Map<TEntity, TModel>(
                await GetByIdAsync(id) 
                ?? throw new InvalidOperationException($"Object with this id: {id}, was not found"));
        

        private async Task<TEntity> GetByIdAsync(TKey id)
        {
            var collection = Database.GetCollection<TEntity>(_tableName);
            var filter = Builders<TEntity>.Filter.Eq("Id", id);

            using (var entiteis = await collection.FindAsync(filter))
            {
                return await entiteis.FirstOrDefaultAsync();
            }
        }

        public virtual async Task InsertAsync<TModel>(TModel model)
        {
            var collection = Database.GetCollection<TEntity>(_tableName);
            var entity = _mapper.Map<TModel, TEntity>(model);

            entity.CreatedBy = entity.UpdatedBy = DateTime.Now;

            await collection.InsertOneAsync(entity);
        }

        /// <summary>
        /// Updates a record in the database,
        /// if the record cannot be found by the specified id, a new one will be added
        /// </summary>
        public virtual async Task UpdateAsync<TModel>(TKey id, TModel model)
        {
            var collection = Database.GetCollection<TEntity>(_tableName);
            var filter = Builders<TEntity>.Filter.Eq("Id", id);

            /// We need to update all the files except CreatedBy, which is not possible at the database level.
            /// Let's do it manually
            var entity = await GetByIdAsync(id);
            var map = _mapper.Map(model, entity);

            /// If the record was not found in the database, it will be created again
            if (entity == null)
                map.CreatedBy = DateTime.Now;

            map.UpdatedBy = DateTime.Now;

            var result = await collection.ReplaceOneAsync(filter, map, 
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
        public virtual async Task<IEnumerable<TModel>> FindAsync<TModel, TValue>(Expression<Func<TModel, TValue>> expression, TValue value)
        {
            var memberExpression = expression.Body as MemberExpression;

            if (memberExpression == null)
                throw new InvalidOperationException();

            var collection = Database.GetCollection<TEntity>(_tableName);
            var filter = Builders<TEntity>.Filter.Eq(memberExpression.Member.Name, value);

            using (var entiteis = await collection.FindAsync(filter))
            {
                return entiteis.ToList()
                    .Select(e => _mapper.Map<TEntity, TModel>(e));
            }
        }
    }
}
