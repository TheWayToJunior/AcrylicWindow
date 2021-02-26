using AcrylicWindow.ViewModel;
using MaterialDesignThemes.Wpf;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace AcrylicWindow.ViewModels
{
    public class AddDialogViewModel<TModel> : ViewModelBase
        where TModel : class, new()
    {
        public BindingList<PropertyViewModel<TModel>> _someCollection;

        public BindingList<PropertyViewModel<TModel>> SomeCollection
        {
            get => _someCollection;
            set => Set(ref _someCollection, value);
        }

        public ICommand AddCommand { get; }

        public AddDialogViewModel()
        {
            var properties = PropertyCreator.Create<TModel>();
            _someCollection = new BindingList<PropertyViewModel<TModel>>(properties.ToList());

            AddCommand = new DelegateCommand(
                _ => DialogHost.CloseDialogCommand.Execute(ModelCreater.Create(_someCollection), null),
                _ => _someCollection.All(i => i.IsValid));
        }
    }
}
