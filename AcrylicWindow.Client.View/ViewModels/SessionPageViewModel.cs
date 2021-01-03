using AcrylicWindow.Client.Core.Helpers;
using AcrylicWindow.Client.Core.IContract;
using AcrylicWindow.Client.Core.Model;
using System.Windows;
using System.Windows.Input;

namespace AcrylicWindow.ViewModel
{
    public class SessionViewModel : ViewModelBase
    {
        private readonly IAuthorizationProvider _authorizationProvider;
        private readonly IMessageBus _messageBus;

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
            set
            {
                Set(ref _userName, value);
            }
        }

        public ICommand LoginCommand { get; }

        public ICommand LogoutCommand { get; }

        public ICommand CloseCommand { get; }

        public SessionViewModel(IAuthorizationProvider authorizationProvider, IMessageBus messageBus)
        {
            _authorizationProvider = Has.NotNull(authorizationProvider);
            _messageBus = Has.NotNull(messageBus);

            UserName = authorizationProvider
                .GetAuthenticationState()
                .GetClaim("sub");

            LoginCommand  = new DelegateCommand(Login);
            LogoutCommand = new DelegateCommand(Logout);

            CloseCommand = new DelegateCommand(_ => Application.Current.Shutdown());
        }

        private async void Login(object obj)
        {
            /// ToDo: Сheck the connection to the server

            var state = _authorizationProvider.GetAuthenticationState();

            if (!state.IsAuthenticated)
            {
                Logout(null);
            }

            await _messageBus.SendTo<MainWindowViewModel>(new LoginMessage(state, UserName));
        }

        private async void Logout(object obj)
        {
            await _authorizationProvider.Logout();
            await _messageBus.SendTo<MainWindowViewModel>(new LogoutMessage(UserName));
        }
    }
}
