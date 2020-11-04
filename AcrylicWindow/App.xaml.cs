using AcrylicWindow.Helpers;
using AcrylicWindow.Services;
using AcrylicWindow.View.Pages;
using AcrylicWindow.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

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
            services.AddSingleton<ServiceManager>();
            services.AddSingleton<MessageBus>();

            services.AddSingleton<HomeViewModel>();
            services.AddSingletonView<ITab, HomeTab>(typeof(HomeViewModel));

            services.AddSingleton<ITab, OptionsTab>();
            services.AddSingleton<ITab, EmployeesTab>();

            services.AddScoped<MainWindow>();
            services.AddScoped<MainWindowViewModel>();

            services.AddScopedView<MainPage>(typeof(MainPageViewModel));

            services.AddScoped(provider => new MainPageViewModel(
                provider.GetService<MessageBus>(),
                provider.GetService<ServiceManager>().Pages));

            services.AddScoped<LoginPageViewModel>();
            services.AddScopedView<LoginPage>(typeof(LoginPageViewModel));
        }

        private void OnStartup(object sender, StartupEventArgs args)
        {
            var mainWindow = _provider.GetService<MainWindow>();
            mainWindow.DataContext = _provider.GetService<MainWindowViewModel>();

            mainWindow.Show();
        }
    }
}
