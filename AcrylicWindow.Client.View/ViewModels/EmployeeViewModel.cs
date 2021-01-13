using AcrylicWindow.Client.Core.Helpers;
using AcrylicWindow.Client.Core.IContract;
using AcrylicWindow.Client.Core.Model;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace AcrylicWindow.ViewModel
{
    public class EmployeeViewModel : ViewModelBase
    {
        private readonly IEmployeeService _service;

        public BindingList<RowCheckBoxViewModel<Employee>> _listItems;

        public BindingList<RowCheckBoxViewModel<Employee>> ListItems
        {
            get => _listItems;
            set => Set(ref _listItems, value);
        }

        private bool _checkAll;

        public bool CheckAll
        {
            get => _checkAll;
            set => Set(ref _checkAll, value);
        }

        public ICommand CheckAllCommand { get; }

        public ICommand DeleteCommand { get; }

        public EmployeeViewModel(IEmployeeService service)
        {
            _service = Has.NotNull(service, nameof(service));

            ListItems = new BindingList<RowCheckBoxViewModel<Employee>>();
            ListItems.ListChanged += OnListChanged;

            CheckAllCommand = new DelegateCommand(Check);
            DeleteCommand = new DelegateCommand(Delete, _ => !_listItems.Any(i => i.Check));

            ReceiveData();
        }

        private void OnListChanged(object s, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemChanged)
                CheckAll = !_listItems.Any(i => !i.Check);
        }

        private async void Delete(object id)
        {
            await _service.DeleteAsync(int.Parse(id.ToString()));
            ReceiveData();
        }

        private void Check(object obj)
        {
            /// We save the value so that we don't lose it when the ListChanged event occurs
            bool chack = CheckAll;

            ListItems.Select(row => row.Click(chack))
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

        public override void Dispose(bool disposing)
        {
            ListItems.ListChanged -= OnListChanged;
            ListItems.Clear();

            base.Dispose(disposing);
        }
    }
}
