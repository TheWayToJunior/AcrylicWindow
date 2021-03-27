using System;

namespace AcrylicWindow.Client.Entity
{
    public interface IReferencesDeleteable
    {
        public void DeleteReferenseTeacher();

        public void DeleteReferenseStudent(Guid id);
    }
}
