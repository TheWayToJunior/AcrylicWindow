using AcrylicWindow.Client.Core.Helpers;
using AcrylicWindow.Client.Core.IContract;
using AcrylicWindow.Dialogs;
using System;
using System.Windows.Input;

namespace AcrylicWindow.ViewModels
{
    /// <summary>
    /// A base class that implements CRUD methods and logic for paginated output and filtering
    /// </summary>
    /// <typeparam name="TModel">Model type</typeparam>
    public abstract class OperationBaseViewModel<TModel> : ViewModelBase
        where TModel : class, IModel, new()
    {
        protected readonly IOperationBase<TModel, Guid> CrudService;
        protected readonly IDialogService DialogService;

        protected int PageSize { get; set; }

        public PaginationViewModel Pagination { get; set; }

        /// <summary>
        /// ViewModel that will be set when the add dialog is called 
        /// </summary>
        public ViewModelBase AddDialogViewModel { get; set; }

        /// <summary>
        /// ViewModel that will be set when the update dialog is called
        /// </summary>
        public ViewModelBase UpdateDialogViewModel { get; set; }

        private string _filter;

        public string Filter
        {
            get => _filter;
            set => Set(ref _filter, value);
        }

        public ICommand AddCommand { get; }

        public ICommand UpdateCommand { get; }

        public ICommand DeleteCommand { get; }

        public ICommand SearchCommand { get; }

        public ICommand RefreshCommand { get; }

        public OperationBaseViewModel(IOperationBase<TModel, Guid> service, IDialogService dialogService)
        {
            CrudService = Has.NotNull(service);
            DialogService = Has.NotNull(dialogService);

            /// CRUD Command
            AddCommand = new DelegateCommand(OnAddDialog, CanExecuteAdd);
            UpdateCommand = new DelegateCommand(OnUpdateDialog, CanExecutUpdate);
            DeleteCommand = new DelegateCommand(OnDelete, CanExecutDelete);

            RefreshCommand = new DelegateCommand(_ =>
            {
                if (!string.IsNullOrEmpty(Filter))
                    Pagination.Reset();

                Filter = string.Empty;
                ReceiveData(Pagination.Index, PageSize);
            });

            SearchCommand = new DelegateCommand(_ =>
            {
                Pagination.Reset();
                ReceiveData(Pagination.Index, PageSize);
            });
        }

        protected virtual bool CanExecuteAdd(object obj) => true;

        protected virtual async void OnAddDialog(object obj)
        {
            /// AddDialogViewModel<TModel> is default ViewModel
            var result = await DialogService.ShowAsync(AddDialogViewModel ?? new AddDialogViewModel<TModel>());

            if (result is TModel model)
            {
                await CrudService.InsertAsync(model);
                ReceiveData(Pagination.Index, PageSize);
            }
        }

        protected virtual bool CanExecutUpdate(object obj) => true;

        protected virtual async void OnUpdateDialog(object obj)
        {
            var key = Guid.Parse(obj.ToString());
            var foundModel = await CrudService.GetByIdAsync(key);

            /// UpdateDialogViewModel<TModel> is default ViewModel
            var result = await DialogService.ShowAsync(UpdateDialogViewModel ?? new UpdateDialogViewModel<TModel>(foundModel));

            if (result is TModel model)
            {
                await CrudService.UpdateAsync(key, model);
                ReceiveData(Pagination.Index, PageSize);
            }
        }

        protected virtual bool CanExecutDelete(object obj) => true;

        protected virtual async void OnDelete(object obj)
        {
            await CrudService.DeleteAsync(new Guid(obj.ToString()));
            ReceiveData(Pagination.Index, PageSize);
        }

        /// <summary>
        /// Fills the list with the records received from the service in accordance with the parameters
        /// </summary>
        protected abstract void ReceiveData(int page, int pageSize);
    }
}
