using System.Security;

namespace AcrylicWindow.Model
{
    public class UserInfo
    {
        public UserInfo(string email, SecureString password)
        {
            Email = email;
            Password = password;
        }

        public string Email { get; set; }

        public SecureString Password { get; set; }
    }
}
