using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AcrylicWindow.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (field == null && value == null)
                return false;

            if (field?.Equals(value) ?? false)
                return false;

            field = value;

            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
