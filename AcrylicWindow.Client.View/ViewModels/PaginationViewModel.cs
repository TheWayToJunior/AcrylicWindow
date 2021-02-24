using AcrylicWindow.ViewModel;
using System;
using System.Windows.Input;

namespace AcrylicWindow.ViewModels
{
    public class PaginationViewModel : ViewModelBase
    {
        private int _pageSize;
        private int _index = 1;

        public int Index
        {
            get => _index;
            set => Set(ref _index, value);
        }

        private int _count;

        public int PageCount
        {
            get => _count;
            set => Set(ref _count, value);
        }

        public ICommand LeftCommand { get; }

        public ICommand RightCommand { get; }

        public PaginationViewModel(Action<int, int> action, int pageSize)
        {
            _pageSize = pageSize;

            LeftCommand  = new DelegateCommand(_ => action?.Invoke(--Index, pageSize), _ => 1 < Index);
            RightCommand = new DelegateCommand(_ => action?.Invoke(++Index, pageSize), _ => Index < PageCount);
        }

        public void SetCount(long value) =>
            PageCount = Convert.ToInt32(Math.Ceiling((double)value / _pageSize));
    }
}
