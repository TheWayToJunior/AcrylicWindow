using AcrylicWindow.Client.Core.Helpers;
using AcrylicWindow.Client.Core.IContract;
using AcrylicWindow.Client.Core.Models;
using System;
using System.IO;
using System.Security;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AcrylicWindow.Client.Core.Providers
{
    public class AuthorizationProvider : IAuthorizationProvider
    {
        private readonly IAuthorizationService<JwtResponse> _authorizationService;
        private readonly ISessionService<UserSession> _sessionService;
        private readonly ITokenStorage _tokenStorage;

        private readonly string _sessionPath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "session.json");

        private AuthenticationState Anonymous => new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        private AuthenticationState _authenticationState;
        public AuthenticationState AuthenticationState => _authenticationState ?? GetAuthenticationState();

        public AuthorizationProvider(IAuthorizationService<JwtResponse> authorizationService, ISessionService<UserSession> sessionService,
            ITokenStorage tokenStorage)
        {
            _authorizationService = Has.NotNull(authorizationService);
            _sessionService = Has.NotNull(sessionService);
            _tokenStorage = Has.NotNull(tokenStorage);
        }

        public async Task<AuthenticationState> Login(string email, SecureString password)
        {
            AuthorizationResult<JwtResponse> result = await _authorizationService.AuthorizeAsync(email, password);

            return await SignInAsync(result);
        }

        public async Task<AuthenticationState> ExtendSession()
        {
            AuthorizationResult<JwtResponse> response = await _authorizationService.RefreshAsync(_tokenStorage[Tokens.Refresh]);

            return await SignInAsync(response);
        }

        public async Task Logout()
        {
            await AuthenticationStateChangedAsync(accessToken: string.Empty, refreshToken: string.Empty);
            _tokenStorage.RemoveAll();
        }

        private AuthenticationState GetAuthenticationState()
        {
            if (!_sessionService.TryRecover(_sessionPath, out UserSession session))
            {
                return Anonymous;
            }

            return AuthenticationStateChangedAsync(session.AccessToken, session.RefreshToken, saveSession: false)
                .Result;
        }

        private async Task<AuthenticationState> SignInAsync(AuthorizationResult<JwtResponse> authorization)
        {
            if (!authorization.IsSuccess)
            {
                AuthenticationState anonymous = Anonymous;
                anonymous.ErrorMessage = authorization.ErrorMessage;

                return anonymous;
            }

            var response = authorization.Response;
            return await AuthenticationStateChangedAsync(response.AccessToken, response.RefreshToken);
        }

        /// <summary>
        /// Updates the authorization status and saves it to a session file on the local disk
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="refreshToken"></param>
        /// <param name="saveSession">Flag indicating whether the session will be overwritten in the session file</param>
        /// <returns></returns>
        private async Task<AuthenticationState> AuthenticationStateChangedAsync(string accessToken, string refreshToken,
            bool saveSession = true)
        {
            _tokenStorage[Tokens.Access] = accessToken;
            _tokenStorage[Tokens.Refresh] = refreshToken;

            if (saveSession)
            {
                await _sessionService.SaveAsync(_sessionPath, new UserSession
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                });
            }

            return BuildAuthenticationState(accessToken);
        }

        private AuthenticationState BuildAuthenticationState(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return _authenticationState = Anonymous;
            }

            return _authenticationState = new AuthenticationState(
                new ClaimsPrincipal(new ClaimsIdentity(token.ParseClaimsFromJwt(), "jwt")));
        }
    }
}

