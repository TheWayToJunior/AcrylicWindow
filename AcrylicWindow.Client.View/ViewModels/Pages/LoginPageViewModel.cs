using AcrylicWindow.Client.Core.Helpers;
using AcrylicWindow.Client.Core.IContract;
using AcrylicWindow.Client.View.Navigation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AcrylicWindow.ViewModels.Pages
{
    public class LoginPageViewModel : ViewModelBase
    {
        private readonly IAuthorizationProvider _authorizationProvider;
        private readonly NavigationPageService _pageService;

        private string _error;

        public string Error
        {
            get { return _error; }
            set { Set(ref _error, value); }
        }

        private string _email;

        public string Email
        {
            get { return _email; }
            set { Set(ref _email, value); }
        }

        public ICommand LoginCommand { get; }

        public ICommand CloseCommand { get; }

        public LoginPageViewModel(IAuthorizationProvider authorizationProvider, NavigationPageService pageService)
        {
            _authorizationProvider = Has.NotNull(authorizationProvider);
            _pageService = Has.NotNull(pageService);

            LoginCommand = new DelegateCommand(Login, pb =>
                !string.IsNullOrEmpty(Email) && (pb as PasswordBox)?.SecurePassword.Length > 0);

            CloseCommand = new DelegateCommand(_ => Application.Current.Shutdown());
        }

        private async void Login(object obj)
        {
            Error = string.Empty;

            var password = (obj as PasswordBox).SecurePassword;

            var state = await _authorizationProvider.Login(Email, password);

            if (!state.IsAuthenticated)
            {
                Error = state.ErrorMessage;
                return;
            }

            _pageService.NavigateTo(PageViewLocator.MainPage);
        }
    }
}
