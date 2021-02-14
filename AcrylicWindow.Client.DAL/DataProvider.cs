using AcrylicWindow.Client.Core.Helpers;
using AcrylicWindow.Client.DAL.Repositories;
using AcrylicWindow.Client.Data;
using AutoMapper;
using MongoDB.Driver;

namespace AcrylicWindow.Client.DAL
{
    public class DataProvider : IDataProvider
    {
        private readonly IMongoDatabase _database;
        private readonly IMapper _mapper;

        public DataProvider(IMapper mapper, string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);

            _mapper = Has.NotNull(mapper);
        }

        private IEmployeeRepository _employees;

        public IEmployeeRepository Employees => _employees ?? new EmployeeRepository(_database, _mapper);

    }
}
