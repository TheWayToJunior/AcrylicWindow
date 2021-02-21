using AcrylicWindow.Client.Core.Model;
using AcrylicWindow.Client.DAL.Repositories;
using AcrylicWindow.Client.Data.Entities;
using AutoMapper;
using MongoDB.Driver;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AcrylicWindow.Client.Tests
{
    public class EmployeeRepositoryTests
    {
        [Fact]
        public async Task GetAllOnPage()
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ApplicatinProfile());
            });

            var mockMongoDatabase = new Mock<IMongoDatabase>();

            var asyncCursor = new Mock<IAsyncCursor<EmployeeEntity>>();
            asyncCursor.SetupSequence(_async => _async.MoveNext(default)).Returns(true).Returns(false);
            asyncCursor.SetupGet(_async => _async.Current).Returns(new EmployeeEntity[]
            {
                new EmployeeEntity() { Name = "1" },
                new EmployeeEntity() { Name = "2" }
            });

            var mockCollection = new Mock<IMongoCollection<EmployeeEntity>>();
            mockCollection.Setup(x => x.FindAsync(
                Builders<EmployeeEntity>.Filter.Empty,
                It.IsAny<FindOptions<EmployeeEntity>>(), default))
                .Returns(Task.FromResult(asyncCursor.Object));

            mockMongoDatabase.Setup(mdb => mdb.GetCollection<EmployeeEntity>("Employees", null))
                .Returns(mockCollection.Object);

            var repository = new EmployeeRepository(mockMongoDatabase.Object, mapper.CreateMapper());

            var res = await repository.GetAllAsync<Employee>(2, 1);

            Assert.True(res.Any());
            Assert.Equal("2", res.First().Name);
        }

        [Fact]
        public async Task FindByName()
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ApplicatinProfile());
            });

            var mockMongoDatabase = new Mock<IMongoDatabase>();

            var asyncCursor = new Mock<IAsyncCursor<EmployeeEntity>>();
            asyncCursor.SetupSequence(_async => _async.MoveNext(default)).Returns(true).Returns(false);
            asyncCursor.SetupGet(_async => _async.Current).Returns(new EmployeeEntity[]
            {
                new EmployeeEntity() { Name = "2" },
                new EmployeeEntity() { Name = "3" }
            });

            var mockCollection = new Mock<IMongoCollection<EmployeeEntity>>();
            mockCollection.Setup(x => x.FindAsync(
                It.IsAny<FilterDefinition<EmployeeEntity>>(), // Builders<EmployeeEntity>.Filter.Eq("Name", "2");
                It.IsAny<FindOptions<EmployeeEntity>>(), default))
                .Returns(Task.FromResult(asyncCursor.Object));

            mockMongoDatabase.Setup(mdb => mdb.GetCollection<EmployeeEntity>("Employees", null))
                .Returns(mockCollection.Object);

            var repository = new EmployeeRepository(mockMongoDatabase.Object, mapper.CreateMapper());

            var res = await repository.FindAsync<Employee, string>(e => e.Name, "2");

            Assert.True(res.Any());
            Assert.IsType<Employee>(res.First());
            Assert.Equal("2", res.First().Name);
        }
    }
}
