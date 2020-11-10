using AcrylicWindow.IContract;
using System.Windows.Input;

namespace AcrylicWindow.ViewModel
{
    public class RowCheckBoxViewModel<T> : ViewModelBase
        where T : IModel
    {
        private bool _check;

        public bool Check
        {
            get => _check;
            set => Set(ref _check, value);
        }

        public T Model { get; set; }

        public ICommand ClickCommand { get; }

        public RowCheckBoxViewModel(T model = default)
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
