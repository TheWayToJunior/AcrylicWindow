using AcrylicWindow.Client.Core.Helpers;
using AcrylicWindow.ViewModels;
using MaterialDesignThemes.Wpf;
using System.Threading.Tasks;

namespace AcrylicWindow.Dialogs
{
    public class DialogService : IDialogService
    {
        public async Task<object> ShowAsync(ViewModelBase viewModel)
        {
            Has.NotNull(viewModel);

            var dialogView = ViewDialogLocator.View(viewModel);
            dialogView.DataContext = viewModel;

            return await DialogHost.Show(dialogView, "RootDialog");
        }
    }
}
