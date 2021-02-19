using AcrylicWindow.ViewModel;

namespace AcrylicWindow.ViewModels
{
    public class TextBoxViewModel : ViewModelBase
    {
        private string _name = string.Empty;

        public string Name
        {
            get { return _name; }
            set { Set(ref _name, value); }
        }

        private string _text = string.Empty;

        public string Text
        {
            get { return _text; }
            set { Set(ref _text, value); }
        }
    }
}
