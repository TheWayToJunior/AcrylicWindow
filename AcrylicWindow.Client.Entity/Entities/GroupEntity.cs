using System;
using System.Collections.Generic;

namespace AcrylicWindow.Client.Entity.Entities
{
    public class GroupEntity : EntityBase<Guid>, IReferencesDeleteable
    {
        public string Name { get; set; }

        public string Language { get; set; }

        public DateTime Begin { get; set; }

        public DateTime End { get; set; }

        public Guid TeacherId { get; set; }

        public ICollection<Guid> StudentsIds { get; set; } = new List<Guid>();

        public void DeleteReferenseStudent(Guid id) => StudentsIds.Remove(id);

        public void DeleteReferenseTeacher() => TeacherId = Guid.Empty;
    }
}
