using AcrylicWindow.Client.Core.Helpers;
using AcrylicWindow.Client.Core.IContract;
using AcrylicWindow.Client.View.Navigation;
using System.Windows;
using System.Windows.Input;

namespace AcrylicWindow.ViewModel
{
    public class SessionViewModel : ViewModelBase
    {
        private readonly IAuthorizationProvider _authorizationProvider;
        private readonly NavigationPageService _pageService;

        private string _error;

        public string Error
        {
            get { return _error; }
            set { Set(ref _error, value); }
        }

        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set { Set(ref _userName, value); }
        }

        public ICommand LoginCommand { get; }

        public ICommand LogoutCommand { get; }

        public ICommand CloseCommand { get; }

        public SessionViewModel(IAuthorizationProvider authorizationProvider, NavigationPageService pageService)
        {
            _authorizationProvider = Has.NotNull(authorizationProvider);
            _pageService = Has.NotNull(pageService);

            UserName = authorizationProvider.AuthenticationState
                .GetClaim("sub");

            LoginCommand = new DelegateCommand(Login);
            LogoutCommand = new DelegateCommand(Logout);

            CloseCommand = new DelegateCommand(_ => Application.Current.Shutdown());
        }

        private async void Login(object obj)
        {
            var state = await _authorizationProvider.ExtendSession();

            if (!state.IsAuthenticated)
            {
                Error = state.ErrorMessage;
                return;
            }

            _pageService.NavigateTo(PageHalper.MainPage);
        }

        private async void Logout(object obj)
        {
            await _authorizationProvider.Logout();

            _pageService.NavigateTo(PageHalper.LoginPage);
        }
    }
}
