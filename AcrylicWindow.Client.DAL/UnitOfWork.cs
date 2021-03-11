using AcrylicWindow.Client.Core.IContract.IData;
using AcrylicWindow.Client.DAL.Repositories;
using MongoDB.Driver;

namespace AcrylicWindow.Client.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IMongoDatabase _database;

        public UnitOfWork(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        private IEmployeeRepository _employees;
        public IEmployeeRepository Employees => _employees ??=  new EmployeeRepository(_database);
        
        private IStudentRepository _students;
        public IStudentRepository Students => _students ??=  new StudentRepository(_database);

    }
}
