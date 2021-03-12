using AcrylicWindow.Client.Core.Helpers;
using AcrylicWindow.Client.Core.IContract.IServices;
using AcrylicWindow.Client.Core.Models;
using AcrylicWindow.ViewModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace AcrylicWindow.ViewModels.Tabs
{
    public class StudentsViewModel : ViewModel<Student>
    {
        private readonly IStudentService _service;

        private bool IsAnyCheck => ListItems.Any(i => i.Check);

        public BindingList<RowCheckBoxViewModel<Student>> ListItems { get; set; }

        private bool _isCheckAll;

        public bool IsCheckAll
        {
            get => _isCheckAll;
            set => Set(ref _isCheckAll, value);
        }

        public ICommand CheckAllCommand { get; }

        public ICommand DeleteManyCommand { get; }

        public StudentsViewModel(IStudentService service)
            : base(service)
        {
            _service = Has.NotNull(service);

            base.PageSize = 7;
            base.Pagination = new PaginationViewModel(ReceiveData, PageSize);

            CheckAllCommand = new DelegateCommand(Check);
            DeleteManyCommand = new DelegateCommand(DeleteMany, _ => IsAnyCheck);

            ListItems = new BindingList<RowCheckBoxViewModel<Student>>();
            ListItems.ListChanged += OnListChanged;

            ReceiveData(1, PageSize);
        }

        protected override async void ReceiveData(int page, int pageSize)
        {
            ListItems.Clear();

            var result = await _service.GetAll(page, pageSize, Filter);

            foreach (var item in result.Values)
            {
                ListItems.Add(new RowCheckBoxViewModel<Student>(item));
            }

            Pagination.SetCount(result.TotalCount);
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

        private void Check(object obj)
        {
            bool chack = IsCheckAll;

            foreach (var row in ListItems)
            {
                row.Click(chack);
            }
        }

        private void OnListChanged(object sender, ListChangedEventArgs e)
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
