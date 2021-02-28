using AcrylicWindow.ViewModel;
using MaterialDesignThemes.Wpf;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace AcrylicWindow.ViewModels
{
    public class UpdateDialogViewModel<TModel> : ViewModelBase
        where TModel : class, new()
    {
        private BindingList<PropertyViewModel<TModel>> _someCollection;

        public BindingList<PropertyViewModel<TModel>> SomeCollection
        {
            get => _someCollection;
            set => Set(ref _someCollection, value);
        }

        public TModel Model { get; }

        public BitmapImage Image { get; }

        public ICommand UpdateCommand { get; }

        public UpdateDialogViewModel(TModel model)
        {
            Model = model;

            var properties = PropertyCreator.Create(Model);
            _someCollection = new BindingList<PropertyViewModel<TModel>>(properties.ToList());

            var img = typeof(TModel)
                .GetProperties()
                .FirstOrDefault(p => p.Name.Equals("Img"))?
                .GetValue(Model)
                .ToString();

            var created = Uri.TryCreate(img, UriKind.Absolute, out var uri);

            if (created)
                Image = new BitmapImage(uri);

            UpdateCommand = new DelegateCommand(
                _ => DialogHost.CloseDialogCommand.Execute(ModelCreator.Create(_someCollection), null),
                _ => _someCollection.All(i => i.IsValid));
        }
    }
}
