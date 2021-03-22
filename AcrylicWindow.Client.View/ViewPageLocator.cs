using AcrylicWindow.View.Pages;
using AcrylicWindow.Views.Tabs;
using System.Collections.Generic;
using System.Windows.Controls;

namespace AcrylicWindow
{
    public static class ViewPageLocator
    {
        public static Page MainPage => new MainPage();

        public static Page LoginPage => new LoginPage();

        public static Page SessionPage => new SessionPage();

        public  static IDictionary<string, Page> Tabs => new Dictionary<string, Page>()
        {
            [nameof(HelpTab)]      = new HelpTab(),
            [nameof(HomeTab)]      = new HomeTab(),
            [nameof(GroupTab)]     = new GroupTab(),
            [nameof(OptionsTab)]   = new OptionsTab(),
            [nameof(StudentsTab)]  = new StudentsTab(),
            [nameof(EmployeesTab)] = new EmployeesTab()
        };
    }
}
