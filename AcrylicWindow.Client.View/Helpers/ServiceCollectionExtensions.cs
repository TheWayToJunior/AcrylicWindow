using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace AcrylicWindow
{
    /// <summary>
    /// Registers a View paired with ViewModel
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSingletonView<TView>(this IServiceCollection services, Type viewModelType)
            where TView : FrameworkElement, new()
        {
            return services
                .AddSingleton(viewModelType)
                .AddSingleton(provider => new TView { DataContext = provider.GetService(viewModelType) });
        }

        public static IServiceCollection AddSingletonView<TView, TImplementation>(this IServiceCollection services, Type viewModelType)
            where TImplementation : FrameworkElement, TView, new()
            where TView : class
        {
            return services
                .AddSingleton(viewModelType)
                .AddSingleton<TView, TImplementation>(provider => 
                    new TImplementation { DataContext = provider.GetService(viewModelType) });
        }

        public static IServiceCollection AddScopedView<TView>(this IServiceCollection services, Type viewModelType)
            where TView : FrameworkElement, new()
        {
            return services
                .AddScoped(viewModelType)
                .AddScoped(provider => new TView { DataContext = provider.GetService(viewModelType) });
        }

        public static IServiceCollection AddScopedView<TView, TImplementation>(this IServiceCollection services, Type viewModelType)
            where TImplementation : FrameworkElement, TView, new()
            where TView : class
        {
            return services
                .AddScoped(viewModelType)
                .AddScoped<TView, TImplementation>(provider => 
                    new TImplementation { DataContext = provider.GetService(viewModelType) });
        }

        public static IServiceCollection AddTransientView<TView>(this IServiceCollection services, Type viewModelType)
            where TView : FrameworkElement, new()
        {
            return services
                .AddTransient(viewModelType)
                .AddTransient(provider => new TView { DataContext = provider.GetService(viewModelType) });
        }
    }
}
