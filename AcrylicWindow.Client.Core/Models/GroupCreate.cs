using AcrylicWindow.Client.Core.IContract;
using System;

namespace AcrylicWindow.Client.Core.Models
{
    public class GroupCreate : IModel
    {
        [DisplayIgnore]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Language { get; set; }
    }
}
