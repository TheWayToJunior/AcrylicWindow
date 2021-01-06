using AcrylicWindow.Client.Core;
using AcrylicWindow.Client.Core.IContract;
using AcrylicWindow.Client.Core.Model;
using AcrylicWindow.Client.Core.Providers;
using AcrylicWindow.Client.Core.Services;
using AcrylicWindow.View.Pages;
using AcrylicWindow.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
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
            services.AddScopedView<ITab, HomeTab>(typeof(HomeViewModel));
            services.AddScopedView<ITab, OptionsTab>(typeof(OptionViewModel));
            services.AddScopedView<ITab, EmployeesTab>(typeof(EmployeeViewModel));

            services.AddScopedView<MainPage>(typeof(MainPageViewModel));
            services.AddScopedView<LoginPage>(typeof(LoginPageViewModel));
            services.AddScopedView<SessionPage>(typeof(SessionViewModel));

            services.AddScopedView<MainWindow>(typeof(MainWindowViewModel));

            services.AddScoped(typeof(IAuthorizationService<>), typeof(AuthorizationService<>));
            services.AddScoped<ISessionService<UserSession>, UserSessionService>();
            services.AddScoped<IAuthorizationProvider, AuthorizationProvider>();

            services.AddTransient<IEmployeeService, EmployeeService>();

            services.AddSingleton<ITokenStorage, InMemoryTokenStorage>();
            services.AddSingleton<IMessageBus, MessageBus>();

            services.AddScoped<HttpClient>();
            services.AddTransient<PageHalper>();
        }

        private void OnStartup(object sender, StartupEventArgs args)
        {
            _provider.GetService<MainWindow>()
                .Show();
        }
    }
}
