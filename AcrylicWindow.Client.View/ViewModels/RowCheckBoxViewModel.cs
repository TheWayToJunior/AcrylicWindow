using AcrylicWindow.Client.Core.IContract;
using AcrylicWindow.ViewModels;
using System.Windows.Input;

namespace AcrylicWindow.ViewModel
{
    public interface IRowViewModel<TModel>
       where TModel : IModel
    {
        TModel Model { get; set; }
    }

    /// <summary>
    /// Provides functionality for binding data to a checkbox in a table row
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class RowCheckBoxViewModel<TModel> : ViewModelBase, IRowViewModel<TModel>
        where TModel : IModel
    {
        private bool _check;

        public bool Check
        {
            get => _check;
            set => Set(ref _check, value);
        }

        public TModel Model { get; set; }

        public ICommand ClickCommand { get; }

        public RowCheckBoxViewModel(TModel model = default)
        {
            Model = model;

            ClickCommand = new DelegateCommand(_ => Click(!Check));
        }

        public bool Click(bool value)
        {
            return Check = value;
        }
    }
}
