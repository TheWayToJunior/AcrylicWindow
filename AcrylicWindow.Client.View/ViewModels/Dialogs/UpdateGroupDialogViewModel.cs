using AcrylicWindow.Client.Core.Models;
using AcrylicWindow.ViewModel;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace AcrylicWindow.ViewModels.Dialogs
{
    public class UpdateGroupDialogViewModel : ViewModelBase
    {
        public ICommand UpdateCommand { get; }

        public Group Model { get; }

        public ObservableCollection<RowCheckBoxViewModel<Employee>> Employees { get; set; }
        public ObservableCollection<RowCheckBoxViewModel<Student>> Students { get; set; }

        public UpdateGroupDialogViewModel(Group group, IEnumerable<Employee> employees, IEnumerable<Student> students)
        {
            Employees = new ObservableCollection<RowCheckBoxViewModel<Employee>>();

            foreach (var item in employees)
            {
                Employees.Add(new RowCheckBoxViewModel<Employee>(item)
                {
                    Check = group.Teacher?.Id.Equals(item.Id) ?? false
                });
            }

            Students = new ObservableCollection<RowCheckBoxViewModel<Student>>();

            foreach (var item in students)
            {
                Students.Add(new RowCheckBoxViewModel<Student>(item) 
                {
                    Check = group.Students?.Select(s => s.Id).Contains(item.Id) ?? false
                });
            }

            Model = group;

            UpdateCommand = new DelegateCommand(OnUpdate, CanUpdate);
        }

        private bool CanUpdate(object arg)
        {
            return Employees.Where(r => r.Check).Count() == 1;
        }

        private void OnUpdate(object obj)
        {
            var selectedEmployee = Employees.Single(r => r.Check).Model;
            Model.Teacher = selectedEmployee;

            var selectedStudents = Students.Where(r => r.Check).Select(r => r.Model);
            Model.Students = selectedStudents.ToList();

            DialogHost.CloseDialogCommand.Execute(Model, null);
        }
    }
}
