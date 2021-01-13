using AcrylicWindow.Client.Core;
using AcrylicWindow.Client.Core.IContract;
using AcrylicWindow.Client.Core.Model;
using AcrylicWindow.Client.Core.Providers;
using AcrylicWindow.Client.Core.Services;
using AcrylicWindow.Client.View.Navigation;
using AcrylicWindow.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace AcrylicWindow
{
    public class ViewModelLocator
    {
        public static IServiceProvider _provider;

        public static void Initialize()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);

            _provider = services.BuildServiceProvider();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<MainPageViewModel>();
            services.AddScoped<LoginPageViewModel>();
            services.AddScoped<SessionViewModel>();

            services.AddTransient<HomeViewModel>();
            services.AddTransient<OptionViewModel>();
            services.AddTransient<EmployeeViewModel>();

            services.AddSingleton<MainWindowViewModel>();

            services.AddScoped(typeof(IAuthorizationService<>), typeof(AuthorizationService<>));
            services.AddScoped<ISessionService<UserSession>, UserSessionService>();
            services.AddScoped<IAuthorizationProvider, AuthorizationProvider>();

            services.AddScoped<IEmployeeService, EmployeeService>();

            services.AddSingleton<ITokenStorage, InMemoryTokenStorage>();
            services.AddSingleton<NavigationPageService>();

            services.AddScoped<HttpClient>();
        }

        public MainWindowViewModel MainWindow => 
            _provider.GetRequiredService<MainWindowViewModel>();

        public MainPageViewModel MainPage =>
            _provider.GetRequiredService<MainPageViewModel>();

        public LoginPageViewModel LoginPage =>
            _provider.GetService<LoginPageViewModel>();

        public SessionViewModel Session => 
            _provider.GetRequiredService<SessionViewModel>();

        public EmployeeViewModel Employee =>
            _provider.GetRequiredService<EmployeeViewModel>();

        public HomeViewModel Home =>
            _provider.GetRequiredService<HomeViewModel>();

        public OptionViewModel Option =>
            _provider.GetRequiredService<OptionViewModel>();
    }
}
