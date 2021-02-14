using System;

namespace AcrylicWindow.Client.Core.Helpers
{
    public static class Has
    {
        public static T NotNull<T>(T value, string name = null)
            where T : class
        {
            return value ?? throw new ArgumentNullException($"The argument: {name ?? nameof(value)}, must not be null");
        }

        public static string NotNullOrEmpty(string value, string name = null)
        {
            return string.IsNullOrEmpty(value) 
                ? throw new ArgumentException($"The argument: {name ?? nameof(value)}, must not be null or empty") 
                : value;
        }
    }
}
