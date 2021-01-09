using AcrylicWindow.Client.Core.Helpers;
using AcrylicWindow.Client.Core.IContract;
using AcrylicWindow.Client.Core.Model;
using AcrylicWindow.View.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AcrylicWindow.ViewModel
{
    public class MainPageViewModel : ViewModelBase, IDisposable
    {
        private readonly IMessageBus _messageBus;
        private readonly IDictionary<string, Page> _pages;
        private IDisposable _subscription;

        private Page _currentPage;

        public Page CurrentPage
        {
            get { return _currentPage; }
            set { Set(ref _currentPage, value); }
        }

        private FrameworkElement _selectedElement;

        public FrameworkElement SelectedElement
        {
            set
            {
                if (_pages.TryGetValue(value.Name, out var page) &&
                    Set(ref _selectedElement, value))
                {
                    CurrentPage = page;
                }
            }
        }

        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set
            {
                Set(ref _userName, value);
                Icon = _userName.ToUpper().First();
            }
        }

        private char _icon;

        public char Icon
        {
            get { return _icon; }
            set { Set(ref _icon, value); }
        }

        public ICommand LogoutCommand { get; }

        public ICommand CloseCommand { get; }

        public MainPageViewModel(IMessageBus messageBus, PageHalper pageHalper)
        {
            _messageBus = Has.NotNull(messageBus, nameof(messageBus));
            _pages = pageHalper.Tabs;

            _subscription = _messageBus.Receive<UserMessage>(this, message =>
            {
                UserName = message.UserName;
                return Task.CompletedTask;
            });

            CurrentPage = _pages[nameof(HomeTab)];

            LogoutCommand = new DelegateCommand(Logout);
            CloseCommand = new DelegateCommand(_ => Application.Current.Shutdown());
        }

        private async void Logout(object obj)
        {
            await _messageBus.SendTo<MainWindowViewModel>(new LogoutMessage(UserName));
        }

        public void Dispose()
        {
            _subscription.Dispose();
        }
    }
}
