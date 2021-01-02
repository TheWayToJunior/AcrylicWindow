using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace AcrylicWindow
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers the view as "Singleton" and sets the specified DataContext
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        /// <param name="services"></param>
        /// <param name="viewModelType"></param>
        /// <returns></returns>
        public static IServiceCollection AddSingletonView<TView>(this IServiceCollection services, Type viewModelType)
            where TView : FrameworkElement, new()
        {
            return services.AddSingleton(provider =>
                new TView { DataContext = provider.GetService(viewModelType) });
        }

        /// <summary>
        /// Registers the view implementation as a "Singleton" and sets the specified DataContext
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="services"></param>
        /// <param name="viewModelType"></param>
        /// <returns></returns>
        public static IServiceCollection AddSingletonView<TView, TImplementation>(this IServiceCollection services, Type viewModelType)
            where TImplementation : FrameworkElement, TView, new()
            where TView : class
        {

            return services.AddSingleton<TView, TImplementation>(provider =>
                new TImplementation { DataContext = provider.GetService(viewModelType) });
        }

        /// <summary>
        /// Registers the view as "Scoped" and sets the specified DataContext
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        /// <param name="services"></param>
        /// <param name="viewModelType"></param>
        /// <returns></returns>
        public static IServiceCollection AddScopedView<TView>(this IServiceCollection services, Type viewModelType)
            where TView : FrameworkElement, new()
        {
            return services.AddScoped(provider =>
                new TView { DataContext = provider.GetService(viewModelType) });
        }

        /// <summary>
        /// Registers the view implementation as a "Scoped" and sets the specified DataContext
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="services"></param>
        /// <param name="viewModelType"></param>
        /// <returns></returns>
        public static IServiceCollection AddScopedView<TView, TImplementation>(this IServiceCollection services, Type viewModelType)
            where TImplementation : FrameworkElement, TView, new()
            where TView : class
        {

            return services.AddScoped<TView, TImplementation>(provider =>
                new TImplementation { DataContext = provider.GetService(viewModelType) });
        }
    }
}
