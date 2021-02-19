using AcrylicWindow.Client.Core.Helpers;
using System;

namespace AcrylicWindow.Client.Core
{
    public class DisplayedAttribute : Attribute
    {
        public string Name { get; set; }

        public DisplayedAttribute(string name)
        {
            Name = Has.NotNullOrEmpty(name);
        }
    }
}
