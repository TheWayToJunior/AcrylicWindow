using AcrylicWindow.Client.Core.Helpers;
using AcrylicWindow.Client.Core.IContract;
using AcrylicWindow.Client.Core.IContract.IData;
using AcrylicWindow.Client.Core.IContract.IServices;
using AcrylicWindow.Client.Core.IManagers;
using AcrylicWindow.Client.Core.Models;
using System;
using System.Threading.Tasks;

namespace AcrylicWindow.Client.Core.Managers
{
    public class StudentManager : IStudentManager
    {
        private readonly IReferenceExcludable _excludable;
        private readonly IStudentService _studentService;
        private readonly IUnitOfWork _unitOfWork;

        public StudentManager(IUnitOfWork unitOfWork, IReferenceExcludable excludable, IStudentService studentService)
        {
            _excludable = Has.NotNull(excludable);
            _studentService = Has.NotNull(studentService);
            _unitOfWork = Has.NotNull(unitOfWork);
        }

        /// <summary>
        /// Deletes as well as links to it in all groups
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(Guid id)
        {
            var entity = await _unitOfWork.Students.GetByIdAsync(id);

            if (entity == null)
                throw new InvalidOperationException();

            await _excludable.Exclude(g => g.DeleteReferenseStudent(id), entity);
            await _studentService.DeleteAsync(id);
        }

        public async Task<PaginationResult<Student>> GetAll(int page, int pageSize, string filter = null)
        {
            return await _studentService.GetAll(page, pageSize, filter);
        }

        public async Task<Student> GetByIdAsync(Guid id)
        {
            return await _studentService.GetByIdAsync(id);
        }

        public async Task InsertAsync(Student model)
        {
            await _studentService.InsertAsync(Has.NotNull(model));
        }

        public async Task UpdateAsync(Guid id, Student model)
        {
            await _studentService.UpdateAsync(id, Has.NotNull(model));
        }
    }
}
