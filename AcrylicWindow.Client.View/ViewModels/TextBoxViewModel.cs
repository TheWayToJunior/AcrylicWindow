using AcrylicWindow.ViewModel;

namespace AcrylicWindow.ViewModels
{
    public class TextBoxViewModel : ViewModelBase
    {
        private string _name = "";

        public string Name
        {
            get { return _name; }
            set { Set(ref _name, value); }
        }

        private string _text = "";

        public string Text
        {
            get { return _text; }
            set { Set(ref _text, value); }
        }
    }
}
