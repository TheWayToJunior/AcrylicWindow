using AcrylicWindow.Client.Core.IContract;
using AcrylicWindow.Client.Entity;
using System;
using System.Collections.Generic;

namespace AcrylicWindow.Client.Core.Models
{
    public class GroupUpdate : IModel, IParticipantsReferences
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Language { get; set; }

        public DateTime Begin { get; set; }

        public DateTime End { get; set; }

        public Guid TeacherId { get; set; }

        public ICollection<Guid> StudentsIds { get; set; }
    }
}
