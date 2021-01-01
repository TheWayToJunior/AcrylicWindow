using AcrylicWindow.Helpers;
using AcrylicWindow.IContract;
using AcrylicWindow.IContract.IProviders;
using AcrylicWindow.Model;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AcrylicWindow.ViewModel
{
    public class LoginPageViewModel : ViewModelBase
    {
        private readonly IAuthorizationProvider _authorizationProvider;
        private readonly IMessageBus _messageBus;

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

        public LoginPageViewModel(IAuthorizationProvider authorizationProvider, IMessageBus messageBus)
        {
            _authorizationProvider = Has.NotNull(authorizationProvider);
            _messageBus = Has.NotNull(messageBus);

            LoginCommand = new DelegateCommand(Login, pb =>
                !string.IsNullOrEmpty(Email) && (pb as PasswordBox).SecurePassword.Length > 0);

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

            var userName = state.GetClaim("sub");
            await _messageBus.SendTo<MainWindowViewModel>(new UserMessage(userName, Email));
        }
    }
}
