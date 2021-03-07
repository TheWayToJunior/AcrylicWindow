using AcrylicWindow.Client.Core.Models;
using System.Threading.Tasks;

namespace AcrylicWindow.Client.Core.IContract
{
    public interface ISessionService<T>
        where T : UserSession
    {
        Task SaveAsync(string sessionPath, T session);

        bool TryRecover(string sessionPath, out T result);
    }
}
