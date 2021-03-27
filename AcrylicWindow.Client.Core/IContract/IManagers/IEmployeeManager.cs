using AcrylicWindow.Client.Core.IContract;
using AcrylicWindow.Client.Core.Models;
using System;

namespace AcrylicWindow.Client.Core.IManagers
{
    public interface IEmployeeManager : IOperationBase<Employee, Guid>
    {
    }
}
