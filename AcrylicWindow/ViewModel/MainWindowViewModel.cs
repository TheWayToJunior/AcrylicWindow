using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AcrylicWindow.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IDictionary<string, Page> _pages;

        private Page _currentPage;

        public Page CurrentPage
        {
            get { return _currentPage; }
            set { Set(ref _currentPage, value); }
        }

        private FrameworkElement _selectedElement;

        public FrameworkElement SelectedElement
        {
            set
            {
                if (_pages.TryGetValue(value.Name, out var page) &&
                    Set(ref _selectedElement, value))
                {
                    CurrentPage = page;
                }
            }
        }

        public ICommand CloseCommand { get; }

        public MainWindowViewModel(string startPage, IDictionary<string, Page> pages)
        {
            _pages = pages;

            CurrentPage = _pages[startPage];
            CloseCommand = new DelegateCommand(obj => (obj as Window).Close());
        }
    }
}
