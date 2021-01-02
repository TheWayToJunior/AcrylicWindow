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
            /// ToDo : Create a new page for session confirmation
            var state = authorizationProvider.GetAuthenticationState();

            _pageHalper = Has.NotNull(pageHalper);
            _messageBus = Has.NotNull(messageBus);

            _messageBus.Receive<UserMessage>(this, async message =>
            {
                CurrentPage = _pageHalper.MainPage;

                await _messageBus.SendTo<MainPageViewModel>(message);
            });

            _messageBus.Receive<LogoutMessage>(this, async message =>
            {
                await Task.Delay(500);
                CurrentPage = _pageHalper.LoginPage;
            });

            CurrentPage = pageHalper.LoginPage;
        }
    }
}
