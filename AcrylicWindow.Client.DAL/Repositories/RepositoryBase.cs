using AcrylicWindow.Client.Core.Helpers;
using AcrylicWindow.Client.Data;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcrylicWindow.Client.DAL.Repositories
{
    public abstract class RepositoryBase<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        protected readonly IMongoDatabase Database;

        public RepositoryBase(IMongoDatabase database)
        {
            Database = Has.NotNull(database);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(string table)
        {
            var collection = Database.GetCollection<TEntity>(table);
            var result = await collection.FindAsync(new BsonDocument());

            return result.ToList();
        }

        public async Task<TEntity> GetByIdAsync(string table, TKey id)
        {
            var collection = Database.GetCollection<TEntity>(table);
            var filter = Builders<TEntity>.Filter.Eq("Id", id);
            var result = await collection.FindAsync(filter);

            return result.First();
        }

        public async Task InsertAsync(string table, TEntity entity)
        {
            var collection = Database.GetCollection<TEntity>(table);
            await collection.InsertOneAsync(entity);
        }

        public Task UpdateAsync(string table, TKey id, IEntity<TKey> entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string table, TKey id)
        {
            throw new NotImplementedException();
        }
    }
}
