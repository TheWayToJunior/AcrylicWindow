using AcrylicWindow.Client.Core;
using AcrylicWindow.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace AcrylicWindow
{
    public sealed class PropertyCreator
    {
        public static IEnumerable<PropertyViewModel<TModel>> Create<TModel>(TModel model = null)
            where TModel : class, new()
        {
            var properties = typeof(TModel).GetProperties();

            foreach (var item in properties)
            {
                var attributes = item.GetCustomAttributes(false);
                var isIgnore = attributes.Any(a => a is DisplayIgnoreAttribute);

                if (isIgnore)
                    continue;

                string name = (attributes
                    .FirstOrDefault(a => a is DisplayedAttribute) as DisplayedAttribute)?.Name ?? item.Name;

                var property = new PropertyViewModel<TModel>(item.Name) 
                {
                    Title = name
                };

                if (model != null)
                    property.Text = item.GetValue(model).ToString();

                yield return property;
            }
        }
    }
}
