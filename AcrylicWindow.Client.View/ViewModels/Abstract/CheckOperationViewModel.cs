using AcrylicWindow.Client.Core.IContract;
using AcrylicWindow.Dialogs;
using AcrylicWindow.ViewModel;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace AcrylicWindow.ViewModels
{
    /// <summary>
    /// A class that implements the logic of working with marked objects in the table
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public abstract class CheckOperationViewModel<TModel> : OperationBaseViewModel<TModel>
        where TModel : class, IModel, new()
    {
        protected bool IsAnyCheck => ListItems.Any(i => i.Check);

        public BindingList<RowCheckBoxViewModel<TModel>> ListItems { get; set; }

        private bool _isCheckAll;

        public bool IsCheckAll
        {
            get => _isCheckAll;
            set => Set(ref _isCheckAll, value);
        }

        public ICommand CheckAllCommand { get; }

        public ICommand DeleteManyCommand { get; }

        public CheckOperationViewModel(IOperationBase<TModel, Guid> service, IDialogService dialogService)
            : base(service, dialogService)
        {
            CheckAllCommand = new DelegateCommand(OnCheck);
            DeleteManyCommand = new DelegateCommand(OnDeleteMany, _ => IsAnyCheck);

            ListItems = new BindingList<RowCheckBoxViewModel<TModel>>();
            ListItems.ListChanged += OnListChanged;
        }

        protected override async void ReceiveData(int page, int pageSize)
        {
            ListItems.Clear();

            var result = await CrudService.GetAll(page, pageSize, Filter);

            foreach (var item in result.Values)
            {
                ListItems.Add(new RowCheckBoxViewModel<TModel>(item));
            }

            Pagination.SetCount(result.TotalCount);
        }

        /// <summary>
        /// Deletes all marked objects in the table
        /// </summary>
        /// <param name="obj"></param>
        protected virtual async void OnDeleteMany(object obj)
        {
            var ckeckItems = ListItems.Where(i => i.Check);

            foreach (var item in ckeckItems)
            {
                await CrudService.DeleteAsync(item.Model.Id);
            }

            if (ListItems.Count == ckeckItems.Count())
                Pagination.Previous();

            ReceiveData(Pagination.Index, PageSize);
        }

        /// <summary>
        /// Sets all objects in the check box table to true or false
        /// </summary>
        protected virtual void OnCheck(object obj)
        {
            bool chack = IsCheckAll;

            foreach (var row in ListItems)
            {
                row.Click(chack);
            }
        }

        protected virtual void OnListChanged(object sender, ListChangedEventArgs e)
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
