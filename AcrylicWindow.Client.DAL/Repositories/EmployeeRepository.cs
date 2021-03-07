using AcrylicWindow.Client.Core.IContract.IData;
using AcrylicWindow.Client.Entity.Entities;
using MongoDB.Driver;
using System;

namespace AcrylicWindow.Client.DAL.Repositories
{
    public class EmployeeRepository : RepositoryBase<EmployeeEntity, Guid>, IEmployeeRepository
    {
        public EmployeeRepository(IMongoDatabase database)
            : base(database, "Employees")
        {
        }
    }
}
