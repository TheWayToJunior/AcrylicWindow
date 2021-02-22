using AcrylicWindow.Client.Core.Helpers;
using AcrylicWindow.Client.Core.IContract.IServices;
using AcrylicWindow.Client.Core.Model;
using AcrylicWindow.ViewModels;
using AcrylicWindow.Views.Dialogs;
using MaterialDesignThemes.Wpf;
using System;
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

        public ICommand RefreshCommand { get; }

        public ICommand AddCommand { get; }

        public ICommand DeleteCommand { get; }

        public EmployeeViewModel(IEmployeeService service)
        {
            _service = Has.NotNull(service, nameof(service));

            ListItems = new BindingList<RowCheckBoxViewModel<Employee>>();
            ListItems.ListChanged += OnListChanged;

            CheckAllCommand = new DelegateCommand(Check);
            AddCommand = new DelegateCommand(RunDialog);

            RefreshCommand = new DelegateCommand(_ => ReceiveData());
            DeleteCommand = new DelegateCommand(Delete, _ => !_listItems.Any(i => i.Check));

            ReceiveData();
        }

        private async void RunDialog(object obj)
        {
            var view = new AddDialog
            {
                DataContext = new AddDialogViewModel<Employee>()
            };

            var result = await DialogHost.Show(view, "RootDialog");

            if (result is Employee employee)
            {
                await _service.InsertAsync(employee);
                ReceiveData();
            }
        }

        private void OnListChanged(object s, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemChanged)
                CheckAll = !_listItems.Any(i => !i.Check);
        }

        private async void Delete(object id)
        {
            await _service.DeleteAsync(new Guid(id.ToString()));
            ReceiveData();
        }

        private void Check(object obj)
        {
            /// We save the value so that we don't lose it when the ListChanged event occurs
            bool chack = CheckAll;

            ListItems.Select(row => row.Click(chack))
                 .ToList();
        }

        private async void ReceiveData()
        {
            ListItems.Clear();

            foreach (var item in await _service.GetAllAsync(1, 7))
            {
                ListItems.Add(new RowCheckBoxViewModel<Employee>(item));
            }
        }

        public override void Dispose(bool collect)
        {
            ListItems.ListChanged -= OnListChanged;
            ListItems.Clear();

            base.Dispose(collect);
        }
    }
}
