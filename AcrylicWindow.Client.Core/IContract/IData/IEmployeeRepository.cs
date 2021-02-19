using AcrylicWindow.Client.Data.Entities;
using System;

namespace AcrylicWindow.Client.Data
{
    public interface IEmployeeRepository : IRepository<EmployeeEntity, Guid>
    {
    }
}
