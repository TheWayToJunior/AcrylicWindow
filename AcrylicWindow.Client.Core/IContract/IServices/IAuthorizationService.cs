using AcrylicWindow.Client.Core.Models;
using System.Security;
using System.Threading.Tasks;

namespace AcrylicWindow.Client.Core.IContract
{
    public interface IAuthorizationService<TResult>
        where TResult : class, new()
    {
        Task<AuthorizationResult<TResult>> AuthorizeAsync(string email, SecureString password);

        Task<AuthorizationResult<TResult>> RefreshAsync(string refreshToken);
    }
}
