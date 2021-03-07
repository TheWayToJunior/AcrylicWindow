using AcrylicWindow.Client.Core.IContract;
using AcrylicWindow.Client.Core.Models;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace AcrylicWindow.Client.Core.Services
{
    public class UserSessionService : ISessionService<UserSession>
    {
        public async Task SaveAsync(string sessionPath, UserSession session)
        {
            using (FileStream fs = new FileStream(sessionPath, FileMode.Create))
            {
                await JsonSerializer.SerializeAsync(fs, session);
            }
        }

        public bool TryRecover(string sessionPath, out UserSession result)
        {
            result = default;

            if (!File.Exists(sessionPath))
            {
                return false;
            }

            result = JsonSerializer.Deserialize<UserSession>(File.ReadAllText(sessionPath));
            return true;
        }
    }
}
