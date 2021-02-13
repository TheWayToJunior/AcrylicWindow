using System;

namespace AcrylicWindow.Client.Data.Entities
{
    public class EmployeeEntity : EntityBase<Guid>
    {
        public string Name { get; set; }

        public string Position { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Img { get; set; }
    }
}
