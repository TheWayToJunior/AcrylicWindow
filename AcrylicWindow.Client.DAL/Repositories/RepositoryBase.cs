using AcrylicWindow.Client.Core.Helpers;
using AcrylicWindow.Client.Data;
using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AcrylicWindow.Client.DAL.Repositories
{
    public abstract class RepositoryBase<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
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

        public virtual async Task<IEnumerable<TModel>> GetAllAsync<TModel>(int page, int pageSize)
        {
            var collection = Database.GetCollection<TEntity>(_tableName);

            using (var entiteis = await collection.FindAsync(new BsonDocument()))
            {
                return entiteis.ToList()
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(e => _mapper.Map<TEntity, TModel>(e));
            }
        }

        public virtual async Task<TModel> GetByIdAsync<TModel>(TKey id)
        {
            var collection = Database.GetCollection<TEntity>(_tableName);
            var filter = Builders<TEntity>.Filter.Eq("Id", id);

            using (var entiteis = await collection.FindAsync(filter))
            {
                return _mapper.Map<TEntity, TModel>(entiteis.First());
            }
        }

        public virtual async Task InsertAsync<TModel>(TModel model)
        {
            var collection = Database.GetCollection<TEntity>(_tableName);
            var entity = _mapper.Map<TModel, TEntity>(model);

            await collection.InsertOneAsync(entity);
        }

        public virtual Task UpdateAsync<TModel>(TKey id, TModel entity)
        {
            throw new NotImplementedException();
        }

        public virtual async Task DeleteAsync(TKey id)
        {
            var collection = Database.GetCollection<TEntity>(_tableName);
            var filter = Builders<TEntity>.Filter.Eq("Id", id);

            await collection.DeleteOneAsync(filter);
        }

        public virtual async Task<TModel> FindAsync<TModel, TValue>(Expression<Func<TModel, TValue>> expression, TValue value)
        {
            var memberExpression = expression.Body as MemberExpression;

            if (memberExpression == null)
                throw new InvalidOperationException();

            var collection = Database.GetCollection<TEntity>(_tableName);
            var filter = Builders<TEntity>.Filter.Eq(memberExpression.Member.Name, value);

            using (var entiteis = await collection.FindAsync(filter))
            {
                return _mapper.Map<TEntity, TModel>(entiteis.FirstOrDefault());
            }
        }
    }
}
