using System;
using System.Collections.Generic;

namespace AcrylicWindow.Client.Entity.Entities
{
    public class StudentEntity : EntityBase<Guid>, IGroupsReferense
    {
        public string Name { get; set; }

        public string Training { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Img { get; set; }

        public ICollection<Guid> Groups { get; set; } = new List<Guid>();
    }
}
