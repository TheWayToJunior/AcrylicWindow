using AcrylicWindow.Client.Core.IContract.IData;
using AcrylicWindow.Client.Entity.Entities;
using MongoDB.Driver;
using System;

namespace AcrylicWindow.Client.DAL.Repositories
{
    public class GroupRepository : RepositoryBase<GroupEntity, Guid>, IGroupRepository
    {
        public GroupRepository(IMongoDatabase database)
            : base(database, "Groups", searchable: true)
        {
        }
    }
}
