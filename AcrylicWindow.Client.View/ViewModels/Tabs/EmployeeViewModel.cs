using AcrylicWindow.Client.Core.Helpers;
using AcrylicWindow.Client.Core.IContract.IServices;
using AcrylicWindow.Client.Core.Models;
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

        private string _filter;

        public string Filter
        {
            get => _filter;
            set => Set(ref _filter, value);
        }

        public ICommand CheckAllCommand { get; }

        public ICommand RefreshCommand { get; }

        public ICommand AddCommand { get; }

        public ICommand UpdateCommand { get; }

        public ICommand DeleteCommand { get; }

        public ICommand DeleteManyCommand { get; }

        public ICommand SearchCommand { get; }

        public EmployeeViewModel(IEmployeeService service)
        {
            _service = Has.NotNull(service, nameof(service));

            ListItems = new BindingList<RowCheckBoxViewModel<Employee>>();
            ListItems.ListChanged += OnListChanged;

            /// CRUD Command
            AddCommand = new DelegateCommand(RunAddDialog);
            UpdateCommand = new DelegateCommand(RunUpdateDialog);
            DeleteCommand = new DelegateCommand(Delete, _ => !IsAnyCheck);
            DeleteManyCommand = new DelegateCommand(DeleteMany, _ => IsAnyCheck);

            /// Additional commands
            Pagination = new PaginationViewModel(ReceiveData, PageSize);
            CheckAllCommand = new DelegateCommand(Check);

            RefreshCommand = new DelegateCommand(_ => 
            {
                if(!string.IsNullOrEmpty(Filter))
                    Pagination.Reset();

                Filter = string.Empty;
                ReceiveData(Pagination.Index, PageSize);
            });

            SearchCommand = new DelegateCommand(_ => 
            {
                Pagination.Reset();
                ReceiveData(Pagination.Index, PageSize);
            });

            ReceiveData(Pagination.Index);
        }

        private AddDialog _addDialog = new AddDialog();

        private async void RunAddDialog(object obj)
        {
            _addDialog.DataContext = new AddDialogViewModel<Employee>();
            var result = await DialogHost.Show(_addDialog, "RootDialog");

            if (result is Employee employee)
            {
                await _service.InsertAsync(employee);
                ReceiveData(Pagination.Index);
            }
        }

        private UpdateDialog _updateDialog = new UpdateDialog();

        private async void RunUpdateDialog(object obj)
        {
            var employee = await _service.GetByIdAsync(Guid.Parse(obj.ToString()));

            _updateDialog.DataContext = new UpdateDialogViewModel<Employee>(employee);
            var result = await DialogHost.Show(_updateDialog, "RootDialog");

            if (result is Employee model)
            {
                await _service.UpdateAsync(Guid.Parse(obj.ToString()), model);
                ReceiveData(Pagination.Index);
            }
        }

        private async void Delete(object id)
        {
            await _service.DeleteAsync(new Guid(id.ToString()));

            if (ListItems.Count == 1)
                Pagination.Previous();

            ReceiveData(Pagination.Index);
        }

        private async void DeleteMany(object obj)
        {
            var ckeckItems = ListItems.Where(i => i.Check);

            foreach (var item in ckeckItems)
            {
                await _service.DeleteAsync(item.Model.Id);
            }

            if (ListItems.Count == ckeckItems.Count())
                Pagination.Previous();

            ReceiveData(Pagination.Index);
        }

        /// <summary>
        /// Fills the list with the records received from the service in accordance with the parameters
        /// </summary>
        private async void ReceiveData(int page, int pageSize = PageSize)
        {
            ListItems.Clear();

            var result = await _service.GetAll(page, pageSize, Filter);

            foreach (var item in result.Values)
            {
                ListItems.Add(new RowCheckBoxViewModel<Employee>(item));
            }

            Pagination.SetCount(result.TotalCount);
        }

        private void Check(object obj)
        {
            /// We save the value so that we don't lose it when the ListChanged event occurs
            bool chack = IsCheckAll;

            ListItems.Select(row => row.Click(chack))
                 .ToList();
        }

        private void OnListChanged(object s, ListChangedEventArgs e)
        {
            var hasFlag = (ListChangedType.ItemAdded | ListChangedType.ItemChanged);

            if (hasFlag.HasFlag(e.ListChangedType) && e.ListChangedType != ListChangedType.Reset)
            {
                IsCheckAll = ListItems.All(i => i.Check);
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
