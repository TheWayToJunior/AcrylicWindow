using AcrylicWindow.Client.Core.Model;
using System.Security;
using System.Threading.Tasks;

namespace AcrylicWindow.Client.Core.IContract
{
    public interface IAuthorizationService<TResult>
        where TResult : class, new()
    {
        Task<AuthorizationResult<TResult>> AuthorizeAsync(string email, SecureString password);
    }
}
