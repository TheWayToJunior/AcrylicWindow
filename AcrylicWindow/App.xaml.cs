using AcrylicWindow.View;
using AcrylicWindow.View.Pages;
using AcrylicWindow.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AcrylicWindow
{
    public partial class App : Application
    {
        private readonly IServiceProvider _provider;

        public App()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);

            _provider = services.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<HomeViewModel>();
            services.AddSingleton<Page, HomePage>(provider =>
                new HomePage { DataContext = provider.GetService<HomeViewModel>() });

            services.AddSingleton<Page, OptionsPage>();
            services.AddSingleton<Page, EmployeesPage>();

            services.AddScoped<MainWindow>();
            services.AddScoped(provider => new MainWindowViewModel(nameof(HomePage),
                provider.GetServices<Page>().ToDictionary(page => page.GetType().Name)));

            services.AddScoped<LoginWindowViewModel>();
        }

        private void OnStartup(object sender, StartupEventArgs args)
        {
            var mainWindow = _provider.GetService<MainWindow>();
            mainWindow.DataContext = _provider.GetService<MainWindowViewModel>();

            mainWindow.Show();
        }
    }
}
