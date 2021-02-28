using AcrylicWindow.Client.Core;
using AcrylicWindow.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AcrylicWindow
{
    public sealed class ModelCreator
    {
        public static TModel Create<TModel>(IEnumerable<PropertyViewModel<TModel>> properties)
            where TModel : class, new()
        {
            var result = new TModel();
            var type = result.GetType();

            string propertyName = string.Empty;

            foreach (var item in properties)
            {
                propertyName = item.Title;
                var property = type.GetProperty(propertyName);

                if (property == null)
                {
                    propertyName = type.GetProperties().FirstOrDefault(prop =>
                    {
                        var attribure = prop.GetCustomAttribute(typeof(DisplayedAttribute), true);
                        return (attribure as DisplayedAttribute)?.Name?.Equals(item.Title) ?? false;

                    }).Name;

                    property = type.GetProperty(propertyName);
                }

                property.SetValue(result, item.Text);
            }

            return result;
        }
    }
}
