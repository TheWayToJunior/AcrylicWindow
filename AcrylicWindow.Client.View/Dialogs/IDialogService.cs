using AcrylicWindow.ViewModels;
using System.Threading.Tasks;

namespace AcrylicWindow.Dialogs
{
    public interface IDialogService
    {
        Task<object> Show(ViewModelBase viewModel);
    }
}
