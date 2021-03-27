using AcrylicWindow.Client.Core.IContract.IData;
using AcrylicWindow.Client.Entity;
using AcrylicWindow.Client.Entity.Entities;
using System;
using System.Threading.Tasks;

namespace AcrylicWindow.Client.Core.Helpers
{
    public static class ReferenceHalper
    {
        /// <summary>
        /// Sets a reference to the group for all the specified objects
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="model"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static async Task SetAllReferences(this IUnitOfWork unitOfWork, GroupEntity model, Action<IGroupsReferense, Guid> action)
        {
            if (model.TeacherId != default)
            {
                var teacher = await unitOfWork.Employees.GetByIdAsync(model.TeacherId);
                /// teacher?.Groups.Add(model.Id);
                action?.Invoke(teacher, model.Id);

                await unitOfWork.Employees.UpdateAsync(teacher.Id, teacher);
            }

            await model.StudentsIds?.SelectAsync(async id =>
            {
                var student = await unitOfWork.Students.GetByIdAsync(id);
                /// student?.Groups.Add(model.Id);
                action?.Invoke(student, model.Id);

                await unitOfWork.Students.UpdateAsync(student.Id, student);
                return id;
            });
        }
    }
}
