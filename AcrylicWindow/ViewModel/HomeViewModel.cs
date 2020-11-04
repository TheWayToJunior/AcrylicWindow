using System.Windows.Input;

namespace AcrylicWindow.ViewModel
{
    public class HomeViewModel : ViewModelBase
    {
        private int _count;

        public int Count
        {
            get { return _count; }
            set { Set(ref _count, value); }
        }

        public ICommand CountCommand { get; }

        public HomeViewModel()
        {
            CountCommand = new DelegateCommand(obj => Count++, obj => Count < 10);
        }
    }
}
