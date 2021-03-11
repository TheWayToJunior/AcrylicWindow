using AcrylicWindow.Client.Core.IContract.IData;
using AcrylicWindow.Client.Entity.Entities;
using MongoDB.Driver;
using System;

namespace AcrylicWindow.Client.DAL.Repositories
{
    public class StudentRepository : RepositoryBase<StudentEntity, Guid>, IStudentRepository
    {
        public StudentRepository(IMongoDatabase database)
            : base(database, "Students")
        {
        }
    }
}
