﻿using AcrylicWindow.Client.Core.Helpers;
using AcrylicWindow.Client.Core.Model;
using AcrylicWindow.Client.Data;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace AcrylicWindow.ViewModel
{
    public class EmployeeViewModel : ViewModelBase
    {
        private readonly IEmployeeRepository _service;

        public BindingList<RowCheckBoxViewModel<Employee>> _listItems;

        public BindingList<RowCheckBoxViewModel<Employee>> ListItems
        {
            get => _listItems;
            set => Set(ref _listItems, value);
        }

        private bool _checkAll;

        public bool CheckAll
        {
            get => _checkAll;
            set => Set(ref _checkAll, value);
        }

        public ICommand CheckAllCommand { get; }

        public ICommand DeleteCommand { get; }

        public EmployeeViewModel(IDataProvider provider)
        {
            _service = Has.NotNull(provider, nameof(provider)).Employees;

            ListItems = new BindingList<RowCheckBoxViewModel<Employee>>();
            ListItems.ListChanged += OnListChanged;

            CheckAllCommand = new DelegateCommand(Check);
            DeleteCommand = new DelegateCommand(Delete, _ => !_listItems.Any(i => i.Check));

            ReceiveData();
        }

        private void OnListChanged(object s, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemChanged)
                CheckAll = !_listItems.Any(i => !i.Check);
        }

        private async void Delete(object id)
        {
            await _service.DeleteAsync(new Guid(id.ToString()));
            ReceiveData();
        }

        private void Check(object obj)
        {
            /// We save the value so that we don't lose it when the ListChanged event occurs
            bool chack = CheckAll;

            ListItems.Select(row => row.Click(chack))
                 .ToList();
        }

        private async void ReceiveData()
        {
            ListItems.Clear();

            foreach (var item in await _service.GetAllAsync<Employee>(1, 7))
            {
                ListItems.Add(new RowCheckBoxViewModel<Employee>(item));
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
