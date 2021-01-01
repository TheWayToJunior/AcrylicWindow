using AcrylicWindow.Model;
using System.Security;
using System.Threading.Tasks;

namespace AcrylicWindow.IContract.IProviders
{
    public interface IAuthorizationProvider
    {
        AuthenticationState GetAuthenticationStateAsync();

        Task<AuthenticationState> Login(string email, SecureString password);

        Task Logout();
    }
}
