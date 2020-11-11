using AcrylicWindow.Helpers;
using AcrylicWindow.IContract;
using AcrylicWindow.Model;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace AcrylicWindow.ViewModel
{
    public class EmployeeViewModel : ViewModelBase
    {
        private readonly IEmployeeService _service;

        public ObservableCollection<RowCheckBoxViewModel<Employee>> _listItems;

        public ObservableCollection<RowCheckBoxViewModel<Employee>> ListItems
        {
            get => _listItems;
            set => Set(ref _listItems, value);
        }

        public ICommand SelectAllCommand { get; }

        public ICommand DeleteCommand { get; }

        public EmployeeViewModel(IEmployeeService service)
        {
            _service = Has.NotNull(service, nameof(service));

            ListItems = new ObservableCollection<RowCheckBoxViewModel<Employee>>();

            SelectAllCommand = new DelegateCommand(SelectAll);
            DeleteCommand = new DelegateCommand(Delete, _=> !_listItems.Any(i => i.Check));

            ReceiveData();
        }

        private async void Delete(object id)
        {
            await _service.DeleteAsync((int)id);
            ReceiveData();
        }

        private void SelectAll(object obj)
        {
            ListItems.Select(row => row.Click((bool)obj))
                 .ToList();
        }

        private void ReceiveData()
        {
            ListItems.Clear();

            foreach (var item in _service.GetAllAsync().Result)
            {
                ListItems.Add(new RowCheckBoxViewModel<Employee>(item));
            }
        }
    }
}
