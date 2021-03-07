using System.Security.Claims;
using System.Linq;

namespace AcrylicWindow.Client.Core.Models
{
    public class AuthenticationState
    {
        public ClaimsPrincipal User { get; private set; }

        public string ErrorMessage { get; set; }

        public bool IsAuthenticated => User.Identity.IsAuthenticated;

        public AuthenticationState(ClaimsPrincipal principal)
        {
            User = principal;
        }

        public string GetClaim(string nameClaim) =>
            User.Claims.FirstOrDefault(claim => claim.Type.Equals(nameClaim))?.Value;
    }
}
