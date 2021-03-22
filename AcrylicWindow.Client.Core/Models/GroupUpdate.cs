using AcrylicWindow.Client.Core.IContract;
using System;
using System.Collections.Generic;

namespace AcrylicWindow.Client.Core.Models
{
    public class GroupUpdate : IModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Language { get; set; }

        public Guid TeacherId { get; set; }

        public ICollection<Guid> StudentsIds { get; set; }
    }
}
