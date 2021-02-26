using AcrylicWindow.ViewModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace AcrylicWindow.ViewModels
{
    public class PropertyViewModel<TModel> : ViewModelBase
        where TModel : new()
    {
        public PropertyViewModel(string nameProperty)
        {
            NameProperty = nameProperty;
        }

        public bool IsValid { get; private set; }

        public string NameProperty { get; }

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
            set 
            {
                ValidateProperty(value, NameProperty);
                Set(ref _text, value);
            }
        }

        private void ValidateProperty<T>(T value, string name)
        {
            try
            {
                Validator.ValidateProperty(value, new ValidationContext(new TModel(), null, null)
                {
                    MemberName = name
                });

                IsValid = true;
            }
            catch (Exception)
            {
                IsValid = false;
                throw;
            }
        }
    }
}
