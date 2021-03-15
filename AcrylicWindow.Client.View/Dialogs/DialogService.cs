using AcrylicWindow.Client.Core.Helpers;
using AcrylicWindow.Client.Core.Models;
using AcrylicWindow.ViewModels;
using AcrylicWindow.Views.Dialogs;
using MaterialDesignThemes.Wpf;
using System;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AcrylicWindow.Dialogs
{
    public class DialogService : IDialogService
    {
        public async Task<object> Show(ViewModelBase viewModel)
        {
            Has.NotNull(viewModel);

            var dialogView = ViewDialogLocator.View(viewModel);
            dialogView.DataContext = viewModel;

            return await DialogHost.Show(dialogView, "RootDialog");
        }
    }

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
