using AcrylicWindow.Client.Data;
using AcrylicWindow.Client.Data.Entities;
using MongoDB.Driver;
using System;

namespace AcrylicWindow.Client.DAL.Repositories
{
    public class EmployeeRepository : RepositoryBase<EmployeeEntity, Guid>, IEmployeeRepository
    {
        public EmployeeRepository(IMongoDatabase database) : base(database)
        {
        }
    }
}
