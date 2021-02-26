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
        private const int PageSize = 7;
        private readonly IEmployeeService _service;

        public BindingList<RowCheckBoxViewModel<Employee>> ListItems { get; set; }

        public PaginationViewModel Pagination { get; set; }

        public bool IsAnyCheck => ListItems.Any(i => i.Check);

        private bool _isCheckAll;

        public bool IsCheckAll
        {
            get => _isCheckAll;
            set => Set(ref _isCheckAll, value);
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

            Pagination = new PaginationViewModel(ReceiveData, PageSize);

            RefreshCommand = new DelegateCommand(_ => ReceiveData(Pagination.Index, PageSize));
            DeleteCommand = new DelegateCommand(Delete, _ => !IsAnyCheck);
            AddCommand = new DelegateCommand(RunDialog, _ => !IsAnyCheck);

            CheckAllCommand = new DelegateCommand(Check);

            ReceiveData(Pagination.Index);
        }

        private AddDialog _addDialog = new AddDialog();

        private async void RunDialog(object obj)
        {
            _addDialog.DataContext = new AddDialogViewModel<Employee>();
            var result = await DialogHost.Show(_addDialog, "RootDialog");

            if (result is Employee employee)
            {
                await _service.InsertAsync(employee);
                ReceiveData(Pagination.Index);
            }
        }

        private void OnListChanged(object s, ListChangedEventArgs e)
        {
            var hasFlag = (ListChangedType.ItemAdded | ListChangedType.ItemChanged);

            if (hasFlag.HasFlag(e.ListChangedType) && e.ListChangedType != ListChangedType.Reset)
            {
                IsCheckAll = ListItems.All(i => i.Check);
            }
        }

        private async void Delete(object id)
        {
            await _service.DeleteAsync(new Guid(id.ToString()));

            if (ListItems.Count == 1)
                Pagination.Previous();

            ReceiveData(Pagination.Index);
        }

        private void Check(object obj)
        {
            /// We save the value so that we don't lose it when the ListChanged event occurs
            bool chack = IsCheckAll;

            ListItems.Select(row => row.Click(chack))
                 .ToList();
        }

        private async void ReceiveData(int page, int pageSize = PageSize)
        {
            ListItems.Clear();

            foreach (var item in await _service.GetAllAsync(page, pageSize))
            {
                ListItems.Add(new RowCheckBoxViewModel<Employee>(item));
            }

            Pagination.SetCount(await _service.CountAsync());
        }

        public override void Dispose(bool collect)
        {
            ListItems.ListChanged -= OnListChanged;
            ListItems.Clear();

            base.Dispose(collect);
        }
    }
}
