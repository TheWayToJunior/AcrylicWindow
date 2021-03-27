using AcrylicWindow.Client.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcrylicWindow.ViewModels.Dialogs
{
    public class UpdateGroupDialogViewModel : ViewModelBase
    {
        public UpdateGroupDialogViewModel(Group group)
        {
            //AllStudents = students;
            Model = group;
        }

        public IEnumerable<Student> AllStudents { get; }
        public Group Model { get; }
    }
}
