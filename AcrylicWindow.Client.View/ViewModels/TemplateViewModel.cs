using AcrylicWindow.Client.Core.Helpers;
using AcrylicWindow.Client.Core.IContract;
using AcrylicWindow.Client.Core.IContract.IServices;
using AcrylicWindow.Dialogs;
using System;
using System.Windows.Input;

namespace AcrylicWindow.ViewModels
{
    /// <summary>
    /// A base class that implements CRUD methods and logic for paginated output and filtering
    /// </summary>
    /// <typeparam name="TModel">Model type</typeparam>
    public abstract class TemplateViewModel<TModel> : ViewModelBase
        where TModel : class, IModel, new()
    {
        private readonly ICrudService<TModel, Guid> _service;
        private readonly IDialogService _dialogService;

        protected int PageSize { get; set; }

        public PaginationViewModel Pagination { get; set; }

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

        public TemplateViewModel(ICrudService<TModel, Guid> service, IDialogService dialogService)
        {
            _service = Has.NotNull(service);
            _dialogService = Has.NotNull(dialogService);

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
            var result = await _dialogService.Show(new AddDialogViewModel<TModel>());

            if (result is TModel model)
            {
                await _service.InsertAsync(model);
                ReceiveData(Pagination.Index, PageSize);
            }
        }

        protected virtual bool CanExecutUpdate(object obj) => true;

        protected virtual async void OnUpdateDialog(object obj)
        {
            var key = Guid.Parse(obj.ToString());
            var foundModel = await _service.GetByIdAsync(key);

            var result = await _dialogService.Show(new UpdateDialogViewModel<TModel>(foundModel));

            if (result is TModel model)
            {
                await _service.UpdateAsync(key, model);
                ReceiveData(Pagination.Index, PageSize);
            }
        }

        protected virtual bool CanExecutDelete(object obj) => true;

        protected virtual async void OnDelete(object obj)
        {
            await _service.DeleteAsync(new Guid(obj.ToString()));
            ReceiveData(Pagination.Index, PageSize);
        }

        /// <summary>
        /// Fills the list with the records received from the service in accordance with the parameters
        /// </summary>
        protected abstract void ReceiveData(int page, int pageSize);
    }
}
