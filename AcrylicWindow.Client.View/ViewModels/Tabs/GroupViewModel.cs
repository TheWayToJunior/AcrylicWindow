using AcrylicWindow.Client.Core.Helpers;
using AcrylicWindow.Client.Core.IContract;
using AcrylicWindow.Client.Core.IContract.IManagers;
using AcrylicWindow.Client.Core.IManagers;
using AcrylicWindow.Client.Core.Models;
using AcrylicWindow.Dialogs;
using AcrylicWindow.ViewModels.Dialogs;
using AutoMapper;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace AcrylicWindow.ViewModels.Tabs
{
    public class GroupViewModel : ViewModelBase
    {
        private const int PageSize = 5;

        private readonly IEmployeeManager _employeeManager;
        private readonly IStudentManager _studentManager;
        private readonly IGroupManager _groupManager;

        private readonly IDialogService _dialogService;
        private readonly IMapper _mapper;

        public PaginationViewModel Pagination { get; set; }

        public BindingList<Group> Items { get; set; }

        public ICommand AddCommand { get; set; }

        public ICommand UpdateCommand { get; set; }

        public ICommand DeleteCommand { get; set; }

        public ICommand RefreshCommand { get; set; }

        public GroupViewModel(IGroupManager groupProvider, IEmployeeManager employeeManager, IStudentManager studentManager, 
            IDialogService dialogService, IMapper mapper)
        {
            _employeeManager = Has.NotNull(employeeManager);
            _studentManager = Has.NotNull(studentManager);
            _groupManager = Has.NotNull(groupProvider);

            _dialogService = Has.NotNull(dialogService);
            _mapper = mapper;

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

            var result = await _dialogService.ShowAsync(new UpdateGroupDialogViewModel(
                group: model,
                /// ToDo: Make a pagination
                employees: (await _employeeManager.GetAll(1, 100)).Values,
                students: (await _studentManager.GetAll(1, 100)).Values));

            if (result is Group group)
            {
                var update = _mapper.Map<GroupUpdate>(group);
                await _groupManager.UpdateAsync(model.Id, update);
                ReceiveData(Pagination.Index, PageSize);
            }
        }

        private async void OnAdd(object obj)
        {
            var result = await _dialogService.ShowAsync(new AddDialogViewModel<GroupCreate>());

            if (result is GroupCreate group)
            {
                await _groupManager.InsertAsync(group);
                ReceiveData(Pagination.Index, PageSize);
            }
        }

        private async void OnDelete(object obj)
        {
            if (Items.Count == 1)
            {
                Pagination.Previous();
            }

            await _groupManager.DeleteAsync(Guid.Parse(obj.ToString()));
            ReceiveData(Pagination.Index, PageSize);
        }

        public async void ReceiveData(int index, int size)
        {
            Items.Clear();

            var groups = await _groupManager.GetAllAsync(index, size);

            foreach (var item in groups.Values)
            {
                Items.Add(item);
            }

            Pagination.SetCount(groups.TotalCount);
        }
    }
}
