using AcrylicWindow.ViewModels;
using System.Threading.Tasks;

namespace AcrylicWindow.Dialogs
{
    public interface IDialogService
    {
        Task<object> ShowAsync(ViewModelBase viewModel);
    }
}
