using AcrylicWindow.IContract;
using System.Security;

namespace AcrylicWindow.Model
{
    public class LoginMessage : IMessage
    {
        public string Email { get; set; }

        public SecureString Password { get; set; }

        public LoginMessage(string email, SecureString password)
        {
            Email = email;
            Password = password;
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
