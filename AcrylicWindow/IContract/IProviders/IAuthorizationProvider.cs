using AcrylicWindow.Model;
using System.Security;
using System.Threading.Tasks;

namespace AcrylicWindow.IContract.IProviders
{
    public interface IAuthorizationProvider
    {
        AuthenticationState GetAuthenticationState();

        Task<AuthenticationState> Login(string email, SecureString password);

        Task Logout();
    }
}
