using AcrylicWindow.Client.DAL.Repositories;
using AcrylicWindow.Client.Data;
using AutoMapper;
using MongoDB.Driver;

namespace AcrylicWindow.Client.DAL
{
    public class DataProvider : IDataProvider
    {
        private readonly IMongoDatabase _database;

        public DataProvider(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        private IEmployeeRepository _employees;

        public IEmployeeRepository Employees => _employees ?? new EmployeeRepository(_database);

    }
}
