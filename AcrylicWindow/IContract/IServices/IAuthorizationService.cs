using AcrylicWindow.Model;
using System.Security;
using System.Threading.Tasks;

namespace AcrylicWindow.IContract
{
    public interface IAuthorizationService<TResult>
        where TResult : class, new()
    {
        Task<AuthorizationResult<TResult>> AuthorizeAsync(UserInfo userInfo);

        Task<AuthorizationResult<TResult>> AuthorizeAsync(string email, SecureString password);
    }
}
