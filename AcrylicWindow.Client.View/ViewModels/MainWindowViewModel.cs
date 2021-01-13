﻿using AcrylicWindow.Client.Core.IContract;
using AcrylicWindow.Client.View.Navigation;
using System.Windows.Controls;

namespace AcrylicWindow.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private Page _currentPage;

        public Page CurrentPage
        {
            get { return _currentPage; }
            set { Set(ref _currentPage, value); }
        }

        public MainWindowViewModel(IAuthorizationProvider authorizationProvider, NavigationPageService pageService)
        {
            pageService.OnPageChanged += (page) => CurrentPage = page;

            var state = authorizationProvider.AuthenticationState;

            if (state.IsAuthenticated)
            {
                pageService.NavigateTo(PageHalper.SessionPage);
                return;
            }

            pageService.NavigateTo(PageHalper.LoginPage);
        }
    }
}
