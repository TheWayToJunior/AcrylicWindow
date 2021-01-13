using AcrylicWindow.Client.Core.Helpers;
using AcrylicWindow.Client.Core.IContract;
using AcrylicWindow.Client.View.Navigation;
using AcrylicWindow.View.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AcrylicWindow.ViewModel
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly IAuthorizationProvider _authorizationProvider;
        private readonly NavigationPageService _pageService;

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

        public MainPageViewModel(IAuthorizationProvider authorizationProvider, NavigationPageService pageService)
        {
            _authorizationProvider = Has.NotNull(authorizationProvider);
            _pageService = Has.NotNull(pageService);

            _pages = PageHalper.Tabs;

            UserName = _authorizationProvider.AuthenticationState.GetClaim("sub");

            CurrentPage = _pages[nameof(HomeTab)];

            LogoutCommand = new DelegateCommand(Logout);
            CloseCommand = new DelegateCommand(_ => Application.Current.Shutdown());
        }

        private async void Logout(object obj)
        {
            await _authorizationProvider.Logout();
            _pageService.NavigateTo(PageHalper.LoginPage);

            Dispose(true);
        }

        public override void Dispose(bool collect)
        {
            foreach (var item in _pages)
            {
                (item.Value.DataContext as ViewModelBase).Dispose(false);
            }

            _pages.Clear();
            _currentPage = null;

            base.Dispose(collect);
        }
    }
}
