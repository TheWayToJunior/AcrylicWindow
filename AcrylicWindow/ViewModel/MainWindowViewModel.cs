using AcrylicWindow.Helpers;
using AcrylicWindow.IContract;
using AcrylicWindow.Model;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AcrylicWindow.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly ServiceManager _serviceManager;
        private readonly IMessageBus _messageBus;

        private Page _currentPage;

        public Page CurrentPage
        {
            get { return _currentPage; }
            set { Set(ref _currentPage, value); }
        }

        public MainWindowViewModel(IMessageBus messageBus, ServiceManager manager)
        {
            _serviceManager = Has.NotNull(manager);
            _messageBus = Has.NotNull(messageBus);

            _messageBus.Receive<LoginMessage>(this, async message =>
            {
                await Task.Delay(500);
                CurrentPage = _serviceManager.MainPage;
            });

            _messageBus.Receive<LogoutMessage>(this, async message =>
            {
                await Task.Delay(500);
                CurrentPage = _serviceManager.LoginPage;
            });

            CurrentPage = manager.LoginPage;
        }
    }
}
