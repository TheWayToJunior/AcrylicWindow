using AcrylicWindow.Client.Core.Helpers;
using AcrylicWindow.Client.Core.IContract;
using AcrylicWindow.Client.Core.Model;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AcrylicWindow.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly PageHalper _pageHalper;
        private readonly IMessageBus _messageBus;

        private Page _currentPage;

        public Page CurrentPage
        {
            get { return _currentPage; }
            set { Set(ref _currentPage, value); }
        }

        public MainWindowViewModel(IAuthorizationProvider authorizationProvider, IMessageBus messageBus, PageHalper pageHalper)
        {
            var state = authorizationProvider.GetAuthenticationState();

            _pageHalper = Has.NotNull(pageHalper);
            _messageBus = Has.NotNull(messageBus);

            _messageBus.Receive<LoginMessage>(this, message =>
            {
                if (message.State.IsAuthenticated)
                    CurrentPage = _pageHalper.MainPage;

                return Task.CompletedTask;
            });

            _messageBus.Receive<LogoutMessage>(this, async message =>
            {
                await Task.Delay(500);
                CurrentPage = _pageHalper.LoginPage;
            });

            if (state.IsAuthenticated)
            {
                CurrentPage = _pageHalper.SessionPage;
                return;
            }

            CurrentPage = _pageHalper.LoginPage;
        }
    }
}
