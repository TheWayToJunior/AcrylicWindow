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

        public EmployeeViewModel(IEmployeeService service)
        {
            _service = service;

            ListItems = new ObservableCollection<RowCheckBoxViewModel<Employee>>();

            foreach (var item in _service.GetAll().Result)
            {
                ListItems.Add(new RowCheckBoxViewModel<Employee>(item));
            }

            SelectAllCommand = new DelegateCommand(SelectAll);
        }

        private void SelectAll(object obj)
        {
            ListItems.Select(row => row.Click((bool)obj))
                 .ToList();
        }
    }
}
