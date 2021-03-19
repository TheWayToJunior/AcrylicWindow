using AcrylicWindow.Client.Core.Models;
using AcrylicWindow.ViewModels;
using AcrylicWindow.Views.Dialogs;
using System;
using System.Windows.Controls;

namespace AcrylicWindow.Dialogs
{
    public static class ViewDialogLocator
    {
        public static ContentControl View(ViewModelBase viewModel) =>
            viewModel switch
            {
                AddDialogViewModel<Employee> => new AddDialog(),
                UpdateDialogViewModel<Employee> => new UpdateDialog(),

                AddDialogViewModel<Student> => new AddDialog(),
                UpdateDialogViewModel<Student> => new UpdateDialog(),

                _ => throw new NotImplementedException(),
            };
    }
}
