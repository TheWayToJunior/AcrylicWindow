using AcrylicWindow.Client.Core.Models;
using System;
using System.Threading.Tasks;

namespace AcrylicWindow.Client.Core.IContract.IServices
{
    public interface IStudentService : IOperationBase<Student, Guid>
    {
        Task<Student> SingleOrDefaultAsync(Guid id);
    }
}
