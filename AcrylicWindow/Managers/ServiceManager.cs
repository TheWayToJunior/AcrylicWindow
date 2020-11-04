using AcrylicWindow.View.Pages;
using AcrylicWindow.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace AcrylicWindow
{
    public class ServiceManager
    {
        private readonly IServiceProvider _provider;

        public ServiceManager(IServiceProvider provider)
        {
            _provider = provider;
        }

        public Page MainPage => _provider.GetService<MainPage>();

        public Page LoginPage => _provider.GetService<LoginPage>();

        public IDictionary<string, Page> Pages => _provider.GetServices<ITab>()
            .Cast<Page>()
            .ToDictionary(page => page.GetType().Name);
    }
}
