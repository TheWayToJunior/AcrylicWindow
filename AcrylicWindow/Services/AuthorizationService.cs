using AcrylicWindow.IContract;
using AcrylicWindow.Model;
using System.Net;
using System.Security;
using System.Text.Json;

namespace AcrylicWindow.Services
{
    public class AuthorizationService<TResponse> : IAuthorizationService<TResponse>
        where TResponse: class, new()
    {
        public AuthorizationResult<TResponse> Authorize(UserInfo userInfo)
        {
            return Authorize(userInfo.Email, userInfo.Password);
        }

        public AuthorizationResult<TResponse> Authorize(string email, SecureString password)
        {
            var result = new AuthorizationResult<TResponse>();

            /// TODO: Authorize
            var passwordString = new NetworkCredential("", password).Password;

            if (email.Length < 4 || passwordString != "1932")
                result.ErrorMessage = "Неверный логин или пароль";
            else
                /// There must be a real DTO received from the API
                result.Response = JsonSerializer.Deserialize<TResponse>("{\"Text\": \"JsonText\"}",
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result;
        }
    }
}
