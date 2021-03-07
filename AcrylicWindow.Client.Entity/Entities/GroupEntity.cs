using System;
using System.Collections.Generic;

namespace AcrylicWindow.Client.Entity.Entities
{
    public class GroupEntity : EntityBase<Guid>
    {
        public Guid TeacherId { get; set; }

        public ICollection<Guid> StudentsIds { get; set; }
    }
}
