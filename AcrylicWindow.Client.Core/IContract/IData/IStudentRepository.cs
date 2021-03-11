using AcrylicWindow.Client.Entity.Entities;
using System;

namespace AcrylicWindow.Client.Core.IContract.IData
{
    public interface IStudentRepository : IRepository<StudentEntity, Guid>
    {
    }
}
