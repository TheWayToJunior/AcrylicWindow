using AcrylicWindow.View.Pages;
using System.Collections.Generic;
using System.Windows.Controls;

namespace AcrylicWindow
{
    public static class PageHalper
    {
        public static Page MainPage => new MainPage();

        public static Page LoginPage => new LoginPage();

        public static Page SessionPage => new SessionPage();

        public  static IDictionary<string, Page> Tabs => new Dictionary<string, Page>()
        {
            [nameof(HomeTab)] = new HomeTab(),
            [nameof(EmployeesTab)] = new EmployeesTab(),
            [nameof(OptionsTab)] = new OptionsTab()
        };
    }
}
