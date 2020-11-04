using AcrylicWindow.Model;
using AcrylicWindow.Services;
using System.Windows;
using System.Windows.Input;

namespace AcrylicWindow.ViewModel
{
    public class LoginPageViewModel : ViewModelBase
    {
        private readonly MessageBus _messageBus;

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
            await _messageBus.SendTo<MainWindowViewModel>(new LoginMessage("Miha", "1932"));
        }
    }
}
