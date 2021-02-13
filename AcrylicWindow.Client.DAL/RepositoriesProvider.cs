using AcrylicWindow.Client.DAL.Repositories;
using AcrylicWindow.Client.Data;
using MongoDB.Driver;

namespace AcrylicWindow.Client.DAL
{
    public class RepositoriesProvider
    {
        private readonly IMongoDatabase _database;

        public RepositoriesProvider(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);


        }

        private IEmployeeRepository _employees;

        public IEmployeeRepository Employees => _employees ?? new EmployeeRepository(_database);

    }
}
