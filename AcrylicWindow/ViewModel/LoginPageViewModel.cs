using AcrylicWindow.Model;
using AcrylicWindow.Services;
using System.Net;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AcrylicWindow.ViewModel
{
    public class LoginPageViewModel : ViewModelBase
    {
        private readonly MessageBus _messageBus;

        private string _email;

        public string Email
        {
            get { return _email; }
            set { Set(ref _email, value); }
        }

        public ICommand LoginCommand { get; }

        public ICommand CloseCommand { get; }

        public LoginPageViewModel(MessageBus messageBus)
        {
            _messageBus = messageBus;

            LoginCommand = new DelegateCommand(Login);
            CloseCommand = new DelegateCommand(_=> Application.Current.Shutdown());
        }

        private async void Login(object obj)
        {
            var password = (obj as PasswordBox).SecurePassword;

            /// TODO: Validate user information

            await _messageBus.SendTo<MainWindowViewModel>(new LoginMessage(Email, password));
        }
    }
}
