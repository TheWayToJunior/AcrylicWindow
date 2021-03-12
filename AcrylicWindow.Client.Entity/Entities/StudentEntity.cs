using System;

namespace AcrylicWindow.Client.Entity.Entities
{
    public class StudentEntity : EntityBase<Guid>
    {
        public string Name { get; set; }

        public string Training { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Img { get; set; }
    }
}
