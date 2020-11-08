using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace AcrylicWindow.ViewModel
{
    public class EmployeeViewModel : ViewModelBase
    {
        public ObservableCollection<RowEmployeeViewModel> _listItems;

        public ObservableCollection<RowEmployeeViewModel> ListItems 
        {
            get => _listItems;
            set => Set(ref _listItems, value);
        }

        public ICommand ClickCommand { get; }

        public ICommand ClipboardCommand { get; }

        public EmployeeViewModel()
        {
            _listItems = new ObservableCollection<RowEmployeeViewModel>()
            {
                /// Test data
                new RowEmployeeViewModel
                {  
                    Id = 1,
                    Check = true, Name = "Смоленский М.С", 
                    Position = "Backend", Phone = "071-311-25-29",
                    Email = "miha.smoelnsky2000@mail.ru",
                    Img = "https://sun2.48276.userapi.com/c629508/v629508849/e7f2/RMYOFC_9YDg.jpg"
                },
                new RowEmployeeViewModel
                {
                    Id = 2,
                    Check = false, Name = "Филин Д.С", 
                    Position = "FullStack", Phone = "071-329-75-31",
                    Email = "Dema.saw12q1wqsa@mail.ru",
                    Img = "https://sun9-55.userapi.com/c841639/v841639638/399c0/r3jChpCwgAE.jpg"
                },
                new RowEmployeeViewModel
                {
                    Id = 3,
                    Check = false, Name = "Мелиневский Р.В", 
                    Position = "Frontend", Phone = "071-316-11-17",
                    Email = "Roro.195623@mail.ru",
                    Img = "https://sun9-56.userapi.com/vr7-StutElseiY4lR19Lpuz43SkyPRHCJPICxg/5MWXrvYwhMQ.jpg"
                },
                new RowEmployeeViewModel
                {
                    Id = 4,
                    Check = true, Name = "Виталий В.В",
                    Position = "Backend",Phone = "071-311-25-29",
                    Email = "vv.Voutow@mail.ru", 
                    Img = "https://sun2.48276.userapi.com/c837137/v837137120/3631f/lcSv9z8FBxY.jpg"}
            };

            ClickCommand = new DelegateCommand(Click);
        }

        private void Click(object obj)
        {
            var a = ListItems.Single(i => i.Id == (int)obj);
            a.Check = !a.Check;
        }
    }

    /// TODO: To make a generic
    public class RowEmployeeViewModel : ViewModelBase
    {
        public int Id { get; set; }

        private bool _check;

        public bool Check 
        {
            get => _check;
            set => Set(ref _check, value);
        }


        /// <summary>
        /// TODO: Move to a separate entity
        /// </summary>
        public string Name { get; set; }

        public string Position { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Img { get; set; }
    }
}
