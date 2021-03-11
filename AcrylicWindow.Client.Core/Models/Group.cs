using AcrylicWindow.Client.Core.IContract;
using System;
using System.Collections.Generic;

namespace AcrylicWindow.Client.Core.Models
{
    class Group : IModel
    {
        public Guid Id { get; set; }

        public Employee Teacher { get; set; }

        public ICollection<Student> Students { get; set; }
    }
}
