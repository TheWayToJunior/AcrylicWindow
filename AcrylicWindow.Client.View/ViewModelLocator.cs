using AcrylicWindow.Client.Core;
using AcrylicWindow.Client.Core.IContract;
using AcrylicWindow.Client.Core.IContract.IServices;
using AcrylicWindow.Client.Core.Models;
using AcrylicWindow.Client.Core.Providers;
using AcrylicWindow.Client.Core.Services;
using AcrylicWindow.Client.View.Navigation;
using AcrylicWindow.Extensions;
using AcrylicWindow.ViewModel;
using AcrylicWindow.ViewModels.Tabs;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace AcrylicWindow
{
    internal class ViewModelLocator
    {
        public static IServiceProvider Provider { get; private set; }

        public static void Initialize()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);

            Provider = services.BuildServiceProvider();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            /// Pages
            services.AddTransient<MainPageViewModel>();
            services.AddScoped<LoginPageViewModel>();
            services.AddScoped<SessionViewModel>();

            /// Tabs
            services.AddTransient<HomeViewModel>();
            services.AddTransient<OptionViewModel>();
            services.AddTransient<EmployeesViewModel>();
            services.AddTransient<StudentsViewModel>();

            /// Windows
            services.AddSingleton<MainWindowViewModel>();

            /// Authorization
            services.AddScoped(typeof(IAuthorizationService<>), typeof(AuthorizationService<>));
            services.AddScoped<ISessionService<UserSession>, UserSessionService>();
            services.AddScoped<IAuthorizationProvider, AuthorizationProvider>();

            /// Infrastructure
            services.AddAutoMapper();
            services.AddScoped<HttpClient>();
            services.AddSingleton<NavigationPageService>();
            services.AddSingleton<ITokenStorage, InMemoryTokenStorage>()
                .AddTransient<IReaderTokenStore>(p => p.GetService<ITokenStorage>());

            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IStudentService, StudentService>();

            /// Data
            services.AddMongoProvider();
        }

        public MainWindowViewModel MainWindow =>
            Provider.GetRequiredService<MainWindowViewModel>();

        public MainPageViewModel MainPage =>
            Provider.GetRequiredService<MainPageViewModel>();

        public LoginPageViewModel LoginPage =>
            Provider.GetService<LoginPageViewModel>();

        public SessionViewModel Session =>
            Provider.GetRequiredService<SessionViewModel>();

        public EmployeesViewModel Employees =>
            Provider.GetRequiredService<EmployeesViewModel>();

        public StudentsViewModel Students =>
            Provider.GetRequiredService<StudentsViewModel>();

        public HomeViewModel Home =>
            Provider.GetRequiredService<HomeViewModel>();

        public OptionViewModel Option =>
            Provider.GetRequiredService<OptionViewModel>();
    }
}
