using AcrylicWindow.View.Pages;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace AcrylicWindow
{
    public class PageManager
    {
        private readonly IServiceProvider _provider;

        public PageManager(IServiceProvider provider)
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
