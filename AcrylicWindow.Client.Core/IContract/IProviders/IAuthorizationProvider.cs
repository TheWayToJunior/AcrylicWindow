using AcrylicWindow.Client.Core.Models;
using System.Security;
using System.Threading.Tasks;

namespace AcrylicWindow.Client.Core.IContract
{
    public interface IAuthorizationProvider
    {
        AuthenticationState AuthenticationState { get; }

        Task<AuthenticationState> Login(string email, SecureString password);

        Task<AuthenticationState> ExtendSession();

        Task Logout();
    }
}
