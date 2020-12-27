using AcrylicWindow.Helpers;
using AcrylicWindow.IContract;
using AcrylicWindow.Model;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AcrylicWindow.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly PageManager _manager;
        private readonly IMessageBus _messageBus;

        private Page _currentPage;

        public Page CurrentPage
        {
            get { return _currentPage; }
            set { Set(ref _currentPage, value); }
        }

        public MainWindowViewModel(IMessageBus messageBus, PageManager manager)
        {
            _manager = Has.NotNull(manager);
            _messageBus = Has.NotNull(messageBus);

            _messageBus.Receive<UserMessage>(this, async message =>
            {
                CurrentPage = _manager.MainPage;

                await _messageBus.SendTo<MainPageViewModel>(message);
            });

            _messageBus.Receive<LogoutMessage>(this, async message =>
            {
                await Task.Delay(500);
                CurrentPage = _manager.LoginPage;
            });

            CurrentPage = manager.LoginPage;
        }
    }
}
