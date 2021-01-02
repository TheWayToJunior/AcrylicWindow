using AcrylicWindow.Client.Core.Model;
using System.Security;
using System.Threading.Tasks;

namespace AcrylicWindow.Client.Core.IContract
{
    public interface IAuthorizationProvider
    {
        AuthenticationState GetAuthenticationState();

        Task<AuthenticationState> Login(string email, SecureString password);

        Task Logout();
    }
}
