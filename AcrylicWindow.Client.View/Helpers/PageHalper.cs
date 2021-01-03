using AcrylicWindow.Client.Core.IContract;
using AcrylicWindow.View.Pages;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace AcrylicWindow
{
    public class PageHalper
    {
        private readonly IServiceProvider _provider;

        public PageHalper(IServiceProvider provider)
        {
            _provider = provider;
        }

        public Page MainPage => _provider.GetService<MainPage>();

        public Page LoginPage => _provider.GetService<LoginPage>();

        public Page SessionPage => _provider.GetService<SessionPage>();

        public IDictionary<string, Page> Tabs => _provider.GetServices<ITab>()
            .Cast<Page>()
            .ToDictionary(page => page.GetType().Name);
    }
}
