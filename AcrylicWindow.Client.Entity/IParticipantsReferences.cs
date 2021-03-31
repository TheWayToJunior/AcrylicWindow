using System;
using System.Collections.Generic;

namespace AcrylicWindow.Client.Entity
{
    public interface IParticipantsReferences
    {
        /// <summary>
        /// Group ID
        /// </summary>
        public Guid Id { get; set; }

        public Guid TeacherId { get; set; }

        public ICollection<Guid> StudentsIds { get; set; }
    }
}
