using System;
using System.Collections.Generic;

namespace AcrylicWindow.Client.Entity.Entities
{
    public class GroupEntity : EntityBase<Guid>
    {
        public string Name { get; set; }

        public string Language { get; set; }

        public Guid TeacherId { get; set; }

        public ICollection<Guid> StudentsIds { get; set; }
    }
}
