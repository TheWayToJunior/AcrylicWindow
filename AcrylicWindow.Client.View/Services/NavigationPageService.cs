using System;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace AcrylicWindow.Client.View.Services
{
    public class NavigationPageService
    {
        private Page _oldPage;

        public event Action<Page> OnPageChanged;

        public void NavigateTo(Page page)
        {
            /// Just in case, to avoid memory leaks
            if (_oldPage != null)
            {
                var navService = NavigationService.GetNavigationService(_oldPage);

                while (navService.CanGoBack)
                {
                    navService.RemoveBackEntry();
                }
            }

            OnPageChanged?.Invoke(page);
            _oldPage = page;
        }
    }
}
