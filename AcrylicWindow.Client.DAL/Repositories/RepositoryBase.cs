using AcrylicWindow.Client.Core.Helpers;
using AcrylicWindow.Client.Core.IContract.IData;
using AcrylicWindow.Client.Entity;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcrylicWindow.Client.DAL.Repositories
{
    public abstract class RepositoryBase<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : EntityBase<TKey>, new()
    {
        protected readonly IMongoCollection<TEntity> _collection;

        public RepositoryBase(IMongoDatabase database, string tableName, bool searchable = true)
        {
            _collection = Has.NotNull(database).GetCollection<TEntity>(tableName);

            if(searchable)
            {
                CreateIndex();
            }
        }

        /// <summary>
        /// Gets a record in the data database divided by pages
        /// </summary>
        /// <typeparam name="TModel">The model that the initial entity will be converted to</typeparam>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Number of entries per page</param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> GetAll() => _collection.AsQueryable();

        public virtual async Task<IEnumerable<TEntity>> SearchAsync(string search)
        {
            var filter = Builders<TEntity>.Filter.Text(search);
            return (await _collection.FindAsync(filter))
                .ToList();
        }

        /// <summary>
        /// Returns a single element by the specified Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> GetByIdAsync(TKey id)
        {
            return await _collection.AsQueryable()
                .SingleAsync(entity => entity.Id.Equals(id));
        }

        public virtual async Task InsertAsync(TEntity entity)
        {
            entity.CreatedBy = entity.UpdatedBy = DateTime.Now;
            await _collection.InsertOneAsync(entity);
        }

        /// <summary>
        /// Updates a record in the database,
        /// if the record cannot be found by the specified id, a new one will be added
        /// </summary>
        public virtual async Task UpdateAsync(TKey id, TEntity entity)
        {
            var filter = Builders<TEntity>.Filter.Eq("Id", id);

            /// We need to update all the files except CreatedBy, which is not possible at the database level.
            /// Let's do it manually
            var foundEntity = await GetByIdAsync(id);

            entity.Id = id;
            /// If the record was not found in the database, it will be created again
            entity.CreatedBy = foundEntity?.CreatedBy ?? DateTime.Now;
            entity.UpdatedBy = DateTime.Now;

            await _collection.ReplaceOneAsync(filter, entity,
                new ReplaceOptions { IsUpsert = true });
        }

        public virtual async Task DeleteAsync(TKey id)
        {
            var filter = Builders<TEntity>.Filter.Eq("Id", id);
            await _collection.DeleteOneAsync(filter);
        }

        /// <summary>
        /// MongoDB provides text indexes to support text search queries on string content.
        /// Text indexes can include any field whose value is a string or an array of string elements.
        /// </summary>
        public virtual void CreateIndex()
        {
            var filter = Builders<TEntity>.IndexKeys.Text("$**");
            var indexModel = new CreateIndexModel<TEntity>(filter);

            _collection.Indexes.CreateOne(indexModel);
        }

        public virtual async Task<long> CountAsync()
        {
            var filter = Builders<TEntity>.Filter.Empty;
            return await _collection.CountDocumentsAsync(filter);
        }
    }
}
