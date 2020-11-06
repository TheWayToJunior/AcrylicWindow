using AcrylicWindow.IContract;
using AcrylicWindow.Model;
using AcrylicWindow.Services;
using AcrylicWindow.View.Pages;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AcrylicWindow.ViewModel
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly IMessageBus _messageBus;
        private readonly IDictionary<string, Page> _pages;

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

        public ICommand CloseCommand { get; }

        public MainPageViewModel(IMessageBus messageBus, IDictionary<string, Page> pages)
        {
            _messageBus = messageBus;
            _pages = pages;

            CurrentPage = _pages[nameof(HomeTab)];
            CloseCommand = new DelegateCommand(Logout);
        }

        private async void Logout(object obj)
        {
            await _messageBus.SendTo<MainWindowViewModel>(new LogoutMessage("UserName"));
        }
    }
}
