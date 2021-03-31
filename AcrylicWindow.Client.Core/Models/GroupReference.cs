using AcrylicWindow.Client.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AcrylicWindow.Client.Core.Models
{
    /// <summary>
    /// Data transfer object for deleting group references
    /// </summary>
    public sealed class DeletedGroupReference : IParticipantsReferences
    {
        public Guid Id { get; set; }

        public Guid TeacherId { get; set; }

        public ICollection<Guid> StudentsIds { get; set; }


        private DeletedGroupReference()
        {
        }

        public static DeletedGroupReference GetInstance(IParticipantsReferences entity, IParticipantsReferences update)
        {
            return new DeletedGroupReference
            {
                Id = entity.Id,
                TeacherId = update.TeacherId != entity.TeacherId
                    ? entity.TeacherId
                    : default,

                StudentsIds = entity.StudentsIds
                    .Except(update.StudentsIds)
                    .ToList()
            };
        }
    }

    /// <summary>
    /// Data transfer object for inserting group references
    /// </summary>
    public sealed class InsertedGroupReference : IParticipantsReferences
    {
        public Guid Id { get; set; }

        public Guid TeacherId { get; set; }

        public ICollection<Guid> StudentsIds { get; set; }

        private InsertedGroupReference()
        {
        }

        public static InsertedGroupReference GetInstance(IParticipantsReferences entity, IParticipantsReferences update)
        {
            return new InsertedGroupReference
            {
                Id = entity.Id,
                TeacherId = update.TeacherId != entity.TeacherId
                    ? update.TeacherId
                    : default,

                StudentsIds = update.StudentsIds
                    .Except(entity.StudentsIds)
                    .ToList()
            };
        }
    }
}
