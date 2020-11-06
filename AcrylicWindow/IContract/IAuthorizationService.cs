using AcrylicWindow.Model;
using System.Security;

namespace AcrylicWindow.IContract
{
    public interface IAuthorizationService<TResult>
        where TResult : class, new()
    {
        AuthorizationResult<TResult> Authorize(UserInfo userInfo);

        AuthorizationResult<TResult> Authorize(string email, SecureString password);
    }
}
