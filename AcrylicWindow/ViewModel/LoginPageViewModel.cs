using AcrylicWindow.Helpers;
using AcrylicWindow.IContract;
using AcrylicWindow.Model;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AcrylicWindow.ViewModel
{
    public class LoginPageViewModel : ViewModelBase
    {
        private readonly IAuthorizationService<JwtResponse> _authorizationService;
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

        public LoginPageViewModel(IAuthorizationService<JwtResponse> authorizationService, IMessageBus messageBus)
        {
            _authorizationService = Has.NotNull(authorizationService);
            _messageBus = Has.NotNull(messageBus);

            LoginCommand = new DelegateCommand(Login, pb =>
                !string.IsNullOrEmpty(Email) && (pb as PasswordBox).SecurePassword.Length > 0);

            CloseCommand = new DelegateCommand(_ => Application.Current.Shutdown());
        }

        private async void Login(object obj)
        {
            Error = string.Empty;

            var password = (obj as PasswordBox).SecurePassword;

            var result = await _authorizationService.AuthorizeAsync(Email, password);

            if(!result.IsSuccess)
            {
                Error = result.ErrorMessage;
                return;
            }

            await _messageBus.SendTo<MainWindowViewModel>(new UserMessage(Email, Email));
        }
    }
}
