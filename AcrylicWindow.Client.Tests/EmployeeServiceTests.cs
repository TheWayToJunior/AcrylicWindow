using AcrylicWindow.Client.Core.IContract.IData;
using AcrylicWindow.Client.Core.Models;
using AcrylicWindow.Client.Core.Services;
using AcrylicWindow.Client.Entity.Entities;
using AutoMapper;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AcrylicWindow.Client.Tests
{
    public class EmployeeServiceTests
    {
        private IQueryable<EmployeeEntity> GetEmployees() => new EmployeeEntity[]
        {
            new EmployeeEntity() { Name = "1" },
            new EmployeeEntity() { Name = "2" },
            new EmployeeEntity() { Name = "3" },
        }
        .AsQueryable();

        private IMapper CreateMapper()
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ApplicationProfile());
            });

            return mapper.CreateMapper();
        }

        [Fact]
        public async Task GetAll()
        {
            var mockRepo = new Mock<IEmployeeRepository>();
            mockRepo.Setup(repo => repo.GetAll()).Returns(GetEmployees());

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unit => unit.Employees).Returns(mockRepo.Object);

            var service = new EmployeeService(mockUnitOfWork.Object, CreateMapper());

            var result = await service.GetAll(1, 2);

            Assert.NotNull(result);
            Assert.IsType<PaginationResult<Employee>>(result);
            Assert.Equal(3, result.TotalCount);
            Assert.Equal(2, result.Values.Count());
            Assert.Equal("2", result.Values.Last().Name);
        }
    }
}
