using AcrylicWindow.Client.Core.Helpers;
using AcrylicWindow.Client.Core.IContract;
using AcrylicWindow.Client.Core.Models;
using AcrylicWindow.Dialogs;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace AcrylicWindow.ViewModels.Tabs
{
    public class GroupViewModel : ViewModelBase
    {
        private readonly IGroupProvider _groupProvider;
        private readonly IDialogService _dialogService;

        public PaginationViewModel Pagination { get; set; }

        public BindingList<Group> Items { get; set; }

        public ICommand ClickCommand { get; set; }

        public GroupViewModel(IGroupProvider groupProvider, IDialogService dialogService)
        {
            _groupProvider = Has.NotNull(groupProvider);
            _dialogService = Has.NotNull(dialogService);

            Pagination = new PaginationViewModel(ReceiveData, 5);

            Items = new BindingList<Group>();

            ClickCommand = new DelegateCommand(Click);

            ReceiveData(1, 5);
        }

        private void Click(object obj)
        {
            var group = obj as Group;
            MessageBox.Show(group.Id.ToString());
        }

        public async void ReceiveData(int index, int size)
        {
            var groups = await _groupProvider.GetAllAsync(index, size);

            foreach (var item in groups.Values)
            {
                Items.Add(item);
            }

            Pagination.SetCount(groups.TotalCount);
        }
    }
}
