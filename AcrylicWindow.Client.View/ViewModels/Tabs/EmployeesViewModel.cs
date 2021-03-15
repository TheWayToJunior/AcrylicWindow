using AcrylicWindow.Client.Core.Helpers;
using AcrylicWindow.Client.Core.IContract.IServices;
using AcrylicWindow.Client.Core.Models;
using AcrylicWindow.Dialogs;
using AcrylicWindow.ViewModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace AcrylicWindow.ViewModels.Tabs
{
    public class EmployeesViewModel : TemplateViewModel<Employee>
    {
        private readonly IEmployeeService _service;

        public BindingList<RowCheckBoxViewModel<Employee>> ListItems { get; set; }

        public bool IsAnyCheck => ListItems.Any(i => i.Check);

        private bool _isCheckAll;

        public bool IsCheckAll
        {
            get => _isCheckAll;
            set => Set(ref _isCheckAll, value);
        }

        public ICommand CheckAllCommand { get; }

        public ICommand DeleteManyCommand { get; }

        public EmployeesViewModel(IEmployeeService service, IDialogService dialogService)
            : base(service, dialogService)
        {
            _service = Has.NotNull(service, nameof(service));

            PageSize = 7;
            Pagination = new PaginationViewModel(ReceiveData, PageSize);

            CheckAllCommand = new DelegateCommand(Check);
            DeleteManyCommand = new DelegateCommand(DeleteMany, _ => IsAnyCheck);

            ListItems = new BindingList<RowCheckBoxViewModel<Employee>>();
            ListItems.ListChanged += OnListChanged;

            ReceiveData(Pagination.Index, PageSize);
        }

        protected override bool CanExecutDelete(object obj) => !IsAnyCheck;

        protected override void OnDelete(object obj)
        {
            if (ListItems.Count == 1)
            {
                Pagination.Previous();
            }

            base.OnDelete(obj);
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

            ReceiveData(Pagination.Index, PageSize);
        }

        protected override async void ReceiveData(int page, int pageSize)
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
            bool chack = IsCheckAll;

            foreach (var row in ListItems)
            {
                row.Click(chack);
            }
        }

        private void OnListChanged(object s, ListChangedEventArgs e)
        {
            var hasFlag = ListChangedType.ItemAdded | ListChangedType.ItemChanged;

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
