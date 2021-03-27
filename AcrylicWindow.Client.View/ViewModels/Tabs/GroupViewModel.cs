using AcrylicWindow.Client.Core.Helpers;
using AcrylicWindow.Client.Core.IContract;
using AcrylicWindow.Client.Core.Models;
using AcrylicWindow.Dialogs;
using AcrylicWindow.ViewModels.Dialogs;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace AcrylicWindow.ViewModels.Tabs
{
    public class GroupViewModel : ViewModelBase
    {
        private const int PageSize = 5;

        private readonly IGroupProvider _groupProvider;
        private readonly IDialogService _dialogService;

        public PaginationViewModel Pagination { get; set; }

        public BindingList<Group> Items { get; set; }

        public ICommand AddCommand { get; set; }

        public ICommand UpdateCommand { get; set; }

        public ICommand DeleteCommand { get; set; }

        public ICommand RefreshCommand { get; set; }

        public GroupViewModel(IGroupProvider groupProvider, IDialogService dialogService)
        {
            _groupProvider = Has.NotNull(groupProvider);
            _dialogService = Has.NotNull(dialogService);

            Pagination = new PaginationViewModel(ReceiveData, PageSize);

            Items = new BindingList<Group>();

            AddCommand = new DelegateCommand(OnAdd);
            UpdateCommand = new DelegateCommand(OnUpdate);
            DeleteCommand = new DelegateCommand(OnDelete);

            RefreshCommand = new DelegateCommand(_ =>
            {
                ReceiveData(Pagination.Index, PageSize);
            });

            ReceiveData(Pagination.Index, PageSize);
        }

        private async void OnUpdate(object obj)
        {
            var model = obj as Group;
            var result = await _dialogService.ShowAsync(new UpdateGroupDialogViewModel(model));

            if (result is GroupUpdate group)
            {
                await _groupProvider.UpdateAsync(model.Id, group);
                ReceiveData(Pagination.Index, PageSize);
            }
        }

        private async void OnAdd(object obj)
        {
            var result = await _dialogService.ShowAsync(new AddDialogViewModel<GroupCreate>());

            if (result is GroupCreate group)
            {
                await _groupProvider.InsertAsync(group);
                ReceiveData(Pagination.Index, PageSize);
            }
        }

        private async void OnDelete(object obj)
        {
            if (Items.Count == 1)
            {
                Pagination.Previous();
            }

            await _groupProvider.DeleteAsync(Guid.Parse(obj.ToString()));
            ReceiveData(Pagination.Index, PageSize);
        }

        public async void ReceiveData(int index, int size)
        {
            Items.Clear();

            var groups = await _groupProvider.GetAllAsync(index, size);

            foreach (var item in groups.Values)
            {
                Items.Add(item);
            }

            Pagination.SetCount(groups.TotalCount);
        }
    }
}
