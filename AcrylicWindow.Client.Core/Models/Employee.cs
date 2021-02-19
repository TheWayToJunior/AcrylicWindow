using AcrylicWindow.Client.Core.IContract;
using System;

namespace AcrylicWindow.Client.Core.Model
{
    public class Employee : IModel
    {
        [DisplayIgnore]
        public Guid Id { get; set; }

        [Displayed("Full name")]
        public string Name { get; set; }

        public string Position { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        [Displayed("Image")]
        public string Img { get; set; }
    }
}
