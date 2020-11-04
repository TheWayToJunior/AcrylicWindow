using AcrylicWindow.IContract;

namespace AcrylicWindow.Model
{
    public class LoginMessage : IMessage
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public LoginMessage(string email, string password)
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
