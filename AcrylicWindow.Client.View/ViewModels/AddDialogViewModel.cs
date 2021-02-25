using AcrylicWindow.Client.Core;
using AcrylicWindow.ViewModel;
using MaterialDesignThemes.Wpf;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;

namespace AcrylicWindow.ViewModels
{
    public class AddDialogViewModel<TModel> : ViewModelBase
        where TModel : class, new()
    {
        public BindingList<TextBoxViewModel<TModel>> _someCollection;

        public BindingList<TextBoxViewModel<TModel>> SomeCollection
        {
            get => _someCollection;
            set => Set(ref _someCollection, value);
        }

        public ICommand TestCommand { get; }

        public AddDialogViewModel()
        {
            _someCollection = new BindingList<TextBoxViewModel<TModel>>();

            /// ToDo: Refactoring
            var properties = typeof(TModel).GetProperties();

            foreach (var item in properties)
            {
                var attributes = item.GetCustomAttributes(false);
                var isIgnore = attributes.Any(a => a is DisplayIgnoreAttribute);

                if (isIgnore)
                    continue;

                string name = (attributes
                    .FirstOrDefault(a => a is DisplayedAttribute) as DisplayedAttribute)?.Name ?? item.Name;

                SomeCollection.Add(new TextBoxViewModel<TModel>(item.Name)
                {
                    Name = name
                });
            }

            TestCommand = new DelegateCommand(p =>
            {
                /// ToDo: Refactoring
                var result = new TModel();
                var type = result.GetType();

                string propertyName = string.Empty;

                foreach (var item in SomeCollection.ToList())
                {
                    propertyName = item.Name;
                    var property = type.GetProperty(propertyName);

                    if (property == null)
                    {
                        propertyName = type.GetProperties().FirstOrDefault(prop =>
                        {
                            var attribure = prop.GetCustomAttribute(typeof(DisplayedAttribute), true);
                            return (attribure as DisplayedAttribute)?.Name?.Equals(item.Name) ?? false;

                        }).Name;

                        property = type.GetProperty(propertyName);
                    }

                    property.SetValue(result, item.Text);
                }

                DialogHost.CloseDialogCommand.Execute(result, null);
            }, _ => !_someCollection.Any(i => !i.IsValid));
        }
    }
}
