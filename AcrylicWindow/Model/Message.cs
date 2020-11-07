using AcrylicWindow.IContract;
using System.Security;

namespace AcrylicWindow.Model
{
    public class UserMessage : IMessage
    {
        public string Email { get; set; }

        public string UserName { get; set; }

        public UserMessage(string userName, string email)
        {
            UserName = userName;
            Email = email;
        }
    }

    public class LogoutMessage : IMessage
    {
        public string Email { get; set; }

        public LogoutMessage(string email)
        {
            Email = email;
        }
    }
}
