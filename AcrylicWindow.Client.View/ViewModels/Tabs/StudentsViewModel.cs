using AcrylicWindow.Client.Core.IManagers;
using AcrylicWindow.Client.Core.Models;
using AcrylicWindow.Dialogs;

namespace AcrylicWindow.ViewModels.Tabs
{
    public class StudentsViewModel : CheckOperationViewModel<Student>
    {
        public StudentsViewModel(IStudentManager manager, IDialogService dialogService)
            : base(manager, dialogService)
        {
            PageSize = 7;
            Pagination = new PaginationViewModel(ReceiveData, PageSize);

            ReceiveData(Pagination.Index, PageSize);
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
    }
}
