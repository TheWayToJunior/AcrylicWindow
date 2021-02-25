using AcrylicWindow.ViewModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace AcrylicWindow.ViewModels
{
    public class TextBoxViewModel<TModel> : ViewModelBase
        where TModel : new()
    {
        public TextBoxViewModel(string nameProperty)
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
                Set(ref _text, value);
                ValidateProperty(value, NameProperty);
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
