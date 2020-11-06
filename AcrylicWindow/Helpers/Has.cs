using System;

namespace AcrylicWindow.Helpers
{
    internal static class Has
    {
        internal static T NotNull<T>(T value, string name = null)
            where T : class
        {
            return value ?? throw new ArgumentNullException($"The argument: {name ?? nameof(value)}, must not be null");
        }
    }
}
