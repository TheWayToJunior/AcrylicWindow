using System;
using System.Collections.Generic;

namespace AcrylicWindow.Client.Entity
{
    public interface IGroupsReferense
    {
        public ICollection<Guid> Groups { get; set; }
    }
}
