using AcrylicWindow.Client.Core.Models;
using System;

namespace AcrylicWindow.Client.Core.IContract
{
    public interface ITokenStorage
    {
        event EventHandler<TokenEventArgs> TokenStateChanged;
        event EventHandler Cleared;

        string this[string key] { get; set; }

        string AddOrUpdate(string key, string value);

        string GetValue(string key);

        bool TryRemove(string key, out string value);

        void RemoveAll();
    }
}
