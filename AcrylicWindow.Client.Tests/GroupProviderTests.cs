using AcrylicWindow.Client.Core.IContract.IData;
using AcrylicWindow.Client.Core.IContract.IServices;
using AcrylicWindow.Client.Core.Models;
using AcrylicWindow.Client.Core.Providers;
using AcrylicWindow.Client.Entity.Entities;
using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AcrylicWindow.Client.Tests
{
    public class GroupProviderTests
    {
        private const string TeacherId = "106ca36e-0fbf-46e7-b641-b2d40ae8ac99";
        private const string StudentId = "105ca36e-0fbf-46e7-b641-b2d40ae8ac99";
        private const string GroupId   = "104ca36e-0fbf-46e7-b641-b2d40ae8ac99";

        private IQueryable<GroupEntity> GetGroups() => new GroupEntity[]
        {
            new GroupEntity()
            {
                Id = new Guid(GroupId),
                Name = "Test Group",
                TeacherId = Guid.Parse(TeacherId),
                StudentsIds = new List<Guid>
                {
                    Guid.Parse(StudentId)
                }
            }
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
        public async Task GetAllAsync()
        {
            // Arrange
            var mockStudents = new Mock<IStudentService>();
            mockStudents.Setup(repo => repo.GetByIdAsync(Guid.Parse(StudentId)))
                .Returns(Task.FromResult(new Student
                {
                    Name = "Test Student"
                }));

            var mockEmployees = new Mock<IEmployeeService>();
            mockEmployees.Setup(repo => repo.GetByIdAsync(Guid.Parse(TeacherId)))
                .Returns(Task.FromResult(new Employee
                {
                    Name = "Test Employee"
                }));

            var mockRepo = new Mock<IGroupRepository>();
            mockRepo.Setup(repo => repo.GetAll()).Returns(GetGroups());

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(uow => uow.Groups).Returns(mockRepo.Object);

            var provider = new GroupProvider(mockUnitOfWork.Object, mockStudents.Object, mockEmployees.Object, CreateMapper());

            // Act
            var result = await provider.GetAllAsync(1, 1);

            // Assert
            Assert.NotNull(result);

            var firstGroup = result.Values.FirstOrDefault();
            Assert.Equal("Test Group", firstGroup.Name);

            Assert.NotNull(firstGroup.Teacher);
            Assert.IsType<Employee>(firstGroup.Teacher);
            Assert.Equal("Test Employee", firstGroup.Teacher.Name);

            Assert.NotEmpty(firstGroup.Students);
            Assert.IsType<Student>(firstGroup.Students.First());
            Assert.Equal("Test Student", firstGroup.Students.First().Name);
        }

        [Fact]
        public async Task GetByIdAsync()
        {
            // Arrange
            var groupId = new Guid(GroupId);

            var mockStudents = new Mock<IStudentService>();
            mockStudents.Setup(repo => repo.GetByIdAsync(Guid.Parse(StudentId)))
                .Returns(Task.FromResult(new Student
                {
                    Name = "Test Student"
                }));

            var mockEmployees = new Mock<IEmployeeService>();
            mockEmployees.Setup(repo => repo.GetByIdAsync(Guid.Parse(TeacherId)))
                .Returns(Task.FromResult(new Employee
                {
                    Name = "Test Employee"
                }));

            var mockRepo = new Mock<IGroupRepository>();
            mockRepo.Setup(repo => repo.GetByIdAsync(groupId)).Returns(Task.FromResult(GetGroups().First()));

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(uow => uow.Groups).Returns(mockRepo.Object);

            var provider = new GroupProvider(mockUnitOfWork.Object, mockStudents.Object, mockEmployees.Object, CreateMapper());

            // Act
            var result = await provider.GetByIdAsync(groupId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(groupId, result.Id);
            Assert.Equal("Test Group", result.Name);

            Assert.NotNull(result.Teacher);
            Assert.IsType<Employee>(result.Teacher);
            Assert.Equal("Test Employee", result.Teacher.Name);

            Assert.NotEmpty(result.Students);
            Assert.IsType<Student>(result.Students.First());
            Assert.Equal("Test Student", result.Students.First().Name);
        }

        [Fact]
        public async Task Exclude_DeleteReferenseTeacher()
        {
            // Arrange
            var groupId = new Guid(GroupId);
            GroupEntity testEntity = null;

            var teacher = new EmployeeEntity { Id = new Guid(TeacherId) };
            teacher.Groups.Add(groupId);

            var mockEmployee = new Mock<IEmployeeService>();
            var mockStudent = new Mock<IStudentService>();

            var mockRepo = new Mock<IGroupRepository>();
            mockRepo.Setup(repo => repo.GetByIdAsync(groupId)).Returns(Task.FromResult(GetGroups().First()));
            mockRepo.Setup(repo => repo.UpdateAsync(groupId, It.IsAny<GroupEntity>()))
                .Callback((Guid id, GroupEntity entity) =>
                {
                    testEntity = entity;
                });

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(uow => uow.Groups).Returns(mockRepo.Object);

            var provider = new GroupProvider(mockUnitOfWork.Object, mockStudent.Object, mockEmployee.Object, CreateMapper());

            // Act
            await provider.Exclude(g => g.DeleteReferenseTeacher(), teacher);

            // Assert
            Assert.Equal(default, testEntity.TeacherId);
        }

        [Fact]
        public async Task Exclude_DeleteReferenseStudent()
        {
            // Arrange
            var groupId = new Guid(GroupId);
            GroupEntity testEntity = null;

            var studentId = new Guid(StudentId);
            var student = new StudentEntity { Id = studentId };
            student.Groups.Add(groupId);

            var mockEmployee = new Mock<IEmployeeService>();
            var mockStudent = new Mock<IStudentService>();

            var mockRepo = new Mock<IGroupRepository>();
            mockRepo.Setup(repo => repo.GetByIdAsync(groupId)).Returns(Task.FromResult(GetGroups().First()));
            mockRepo.Setup(repo => repo.UpdateAsync(groupId, It.IsAny<GroupEntity>()))
                .Callback((Guid id, GroupEntity entity) =>
                {
                    testEntity = entity;
                });

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(uow => uow.Groups).Returns(mockRepo.Object);

            var provider = new GroupProvider(mockUnitOfWork.Object, mockStudent.Object, mockEmployee.Object, CreateMapper());

            // Act
            await provider.Exclude(g => g.DeleteReferenseStudent(studentId), student);

            // Assert
            Assert.False(testEntity.StudentsIds.Contains(studentId));
        }
    }
}
