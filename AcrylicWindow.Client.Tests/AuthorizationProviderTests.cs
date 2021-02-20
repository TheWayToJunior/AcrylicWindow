using AcrylicWindow.Client.Core.Helpers;
using AcrylicWindow.Client.Core.IContract;
using AcrylicWindow.Client.Core.Model;
using AcrylicWindow.Client.Core.Providers;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace AcrylicWindow.Client.Tests
{
    public class AuthorizationProviderTests
    {
        #region The inconvenience
        /// Failed to mock extension method for jwt parsing, perhaps in the future the implementation will be changed
        private const string TestToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";
        #endregion

        [Fact]
        public async Task Login_Authorize_IsSuccess()
        {
            string email = "test";
            var password = new System.Security.SecureString();

            var mockAuthService = new Mock<IAuthorizationService<JwtResponse>>();
            mockAuthService.Setup(auth => auth.AuthorizeAsync(email, password))
                .Returns(Task.FromResult(new AuthorizationResult<JwtResponse>(new JwtResponse() { AccessToken = TestToken })));

            var mockSessionService = new Mock<ISessionService<UserSession>>();
            mockSessionService.Setup(session => session.SaveAsync("test", new UserSession()))
                .Returns(Task.CompletedTask);

            var mockStorege = new Mock<ITokenStorage>();
            mockStorege.Setup(storege => storege[Tokens.Access]).Verifiable();

            var provider = new AuthorizationProvider(mockAuthService.Object, mockSessionService.Object, mockStorege.Object);

            /// Act
            var state = await provider.Login(email, password);

            Assert.True(state.IsAuthenticated);
            Assert.True(string.IsNullOrEmpty(state.ErrorMessage));
            Assert.Equal("John Doe", state.GetClaim("name"));
        }

        [Fact]
        public async Task Login_Authorize_Failed()
        {
            string email = "test";
            var password = new System.Security.SecureString();

            var mockAuthService = new Mock<IAuthorizationService<JwtResponse>>();
            mockAuthService.Setup(auth => auth.AuthorizeAsync(email, password))
                .Returns(Task.FromResult(new AuthorizationResult<JwtResponse>() { ErrorMessage = "Error" }));

            var mockSessionService = new Mock<ISessionService<UserSession>>();
            mockSessionService.Setup(session => session.SaveAsync("test", new UserSession()))
                .Returns(Task.CompletedTask);

            var mockStorege = new Mock<ITokenStorage>();
            mockStorege.Setup(storege => storege[Tokens.Access]).Verifiable();

            var provider = new AuthorizationProvider(mockAuthService.Object, mockSessionService.Object, mockStorege.Object);

            /// Act
            var state = await provider.Login(email, password);

            Assert.False(state.IsAuthenticated);
            Assert.False(string.IsNullOrEmpty(state.ErrorMessage));
            Assert.Equal("Error", state.ErrorMessage);
            Assert.Null(state.GetClaim("name"));
        }

        [Fact]
        public async Task ExtendSession_IsSuccess()
        {
            string refreshToken = "test";

            var mockAuthService = new Mock<IAuthorizationService<JwtResponse>>();
            mockAuthService.Setup(auth => auth.RefreshAsync(refreshToken))
                .Returns(Task.FromResult(new AuthorizationResult<JwtResponse>(new JwtResponse() { AccessToken = TestToken })));

            var mockSessionService = new Mock<ISessionService<UserSession>>();
            mockSessionService.Setup(session => session.SaveAsync("test", new UserSession()))
                .Returns(Task.CompletedTask);

            var mockStorege = new Mock<ITokenStorage>();
            mockStorege.Setup(storege => storege[Tokens.Refresh]).Returns(refreshToken);

            var provider = new AuthorizationProvider(mockAuthService.Object, mockSessionService.Object, mockStorege.Object);

            /// Act
            var state = await provider.ExtendSession();

            Assert.True(state.IsAuthenticated);
            Assert.True(string.IsNullOrEmpty(state.ErrorMessage));
        }

        [Fact]
        public async Task ExtendSession_Failed()
        {
            string refreshToken = "test";

            var mockAuthService = new Mock<IAuthorizationService<JwtResponse>>();
            mockAuthService.Setup(auth => auth.RefreshAsync(refreshToken))
                .Returns(Task.FromResult(new AuthorizationResult<JwtResponse>() {  ErrorMessage = "Error" }));

            var mockSessionService = new Mock<ISessionService<UserSession>>();
            mockSessionService.Setup(session => session.SaveAsync("test", new UserSession()))
                .Returns(Task.CompletedTask);

            var mockStorege = new Mock<ITokenStorage>();
            mockStorege.Setup(storege => storege[Tokens.Refresh]).Returns(refreshToken);

            var provider = new AuthorizationProvider(mockAuthService.Object, mockSessionService.Object, mockStorege.Object);

            /// Act
            var state = await provider.ExtendSession();

            Assert.False(state.IsAuthenticated);
            Assert.False(string.IsNullOrEmpty(state.ErrorMessage));
            Assert.Equal("Error", state.ErrorMessage);
        }

        [Fact]
        public async Task Logout_IsSuccess()
        {
            var mockAuthService = new Mock<IAuthorizationService<JwtResponse>>();

            var mockSessionService = new Mock<ISessionService<UserSession>>();

            var mockStorege = new Mock<ITokenStorage>();
            mockStorege.Setup(storege => storege.RemoveAll()).Verifiable();

            var provider = new AuthorizationProvider(mockAuthService.Object, mockSessionService.Object, mockStorege.Object);

            /// Act
            await provider.Logout();
            var state = provider.AuthenticationState;

            Assert.False(state.IsAuthenticated);
            Assert.True(string.IsNullOrEmpty(state.ErrorMessage));
        }
    }
}
