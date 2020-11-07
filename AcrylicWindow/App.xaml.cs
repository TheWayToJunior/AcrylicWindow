using AcrylicWindow.Helpers;
using AcrylicWindow.IContract;
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
            services.AddSingleton<HomeViewModel>();
            services.AddSingletonView<ITab, HomeTab>(typeof(HomeViewModel));

            services.AddSingleton<ITab, OptionsTab>();
            services.AddSingleton<ITab, EmployeesTab>();

            services.AddScopedView<MainPage>(typeof(MainPageViewModel));
            services.AddScoped(provider => new MainPageViewModel(
                provider.GetService<IMessageBus>(),
                provider.GetService<ServiceManager>().Pages));

            services.AddScoped(typeof(LoginPageViewModel));
            services.AddScopedView<LoginPage>(typeof(LoginPageViewModel));

            services.AddScopedView<MainWindow>(typeof(MainWindowViewModel));
            services.AddScoped<MainWindowViewModel>();

            services.AddScoped<ServiceManager>();
            services.AddScoped<IMessageBus, MessageBus>();
            services.AddScoped(typeof(IAuthorizationService<>), typeof(AuthorizationService<>));
        }

        private void OnStartup(object sender, StartupEventArgs args)
        {
            _provider.GetService<MainWindow>()
                .Show();
        }
    }
}
