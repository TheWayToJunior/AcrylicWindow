﻿using AcrylicWindow.Client.Entity;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace AcrylicWindow
{
    public partial class App : Application
    {
        public App()
        {
            ViewModelLocator.Initialize();
        }
    }
}
