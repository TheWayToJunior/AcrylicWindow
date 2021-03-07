using AcrylicWindow.Client.Core.IContract;
using System;
using System.ComponentModel.DataAnnotations;

namespace AcrylicWindow.Client.Core.Models
{
    public class Client : IModel
    {
        [DisplayIgnore]
        public Guid Id { get; set; }

        [Displayed("Full name")]
        [Required(ErrorMessage = "This is a required field")]
        public string Name { get; set; }

        [Required(ErrorMessage = "This is a required field")]
        [Phone(ErrorMessage = "Please enter your phone")]
        public string Phone { get; set; }

        [EmailAddress(ErrorMessage = "Please enter your email address")]
        public string Email { get; set; }

        [Displayed("Image")]
        public string Img { get; set; }
    }
}
