﻿using AcrylicWindow.Model;
using AcrylicWindow.Services;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AcrylicWindow.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly ServiceManager _serviceManager;
        private readonly MessageBus _messageBus;

        private Page _currentPage;

        public Page CurrentPage
        {
            get { return _currentPage; }
            set { Set(ref _currentPage, value); }
        }

        public MainWindowViewModel(ServiceManager manager, MessageBus messageBus)
        {
            _serviceManager = manager;
            _messageBus = messageBus;

            _messageBus.Receive<LoginMessage>(this, async message => 
            {
                /// TODO: Authorize
                await Task.Delay(500);
                CurrentPage = _serviceManager.MainPage;
            });

            _messageBus.Receive<LogoutMessage>(this, async message =>
            {
                /// TODO: Logout
                await Task.Delay(500);
                CurrentPage = _serviceManager.LoginPage;
            });

            CurrentPage = manager.LoginPage;
        }
    }
}
