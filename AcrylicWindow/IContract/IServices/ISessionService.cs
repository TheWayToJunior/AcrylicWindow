using AcrylicWindow.Model;
using System.Threading.Tasks;

namespace AcrylicWindow.IContract
{
    public interface ISessionService<T>
        where T : UserSession
    {
        Task SaveAsync(string sessionPath, T session);

        bool TryRecover(string sessionPath, out T result);
    }
}
