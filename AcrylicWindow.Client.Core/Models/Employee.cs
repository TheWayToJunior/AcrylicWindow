using AcrylicWindow.Client.Core.IContract;
using System;

namespace AcrylicWindow.Client.Core.Model
{
    public class Employee : IModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Position { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Img { get; set; }
    }
}
