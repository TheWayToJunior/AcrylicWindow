using AcrylicWindow.Client.Core.IContract;
using AcrylicWindow.Client.Core.Models;
using System;
using System.Collections.Concurrent;

namespace AcrylicWindow.Client.Core
{
    public class InMemoryTokenStorage : ITokenStorage
    {
        public event EventHandler<TokenEventArgs> TokenStateChanged;
        public event EventHandler Cleared;

        private readonly ConcurrentDictionary<string, string> _tokens;

        public InMemoryTokenStorage()
        {
            _tokens = new ConcurrentDictionary<string, string>();
        }

        public string this[string key]
        {
            get => this.GetValue(key);
            set => this.AddOrUpdate(key, value);
        }

        public string AddOrUpdate(string key, string value)
        {
            var oldToken = _tokens.AddOrUpdate(key, value, (k, oldValue) => oldValue);

            TokenStateChanged?.Invoke(this, new TokenEventArgs(key, value, oldToken));
            return this[key];
        }

        public string GetValue(string key)
        {
            _tokens.TryGetValue(key, out string token);
            return token;
        }

        public bool TryRemove(string key, out string value) => _tokens.TryRemove(key, out value);

        public void RemoveAll()
        {
            _tokens.Clear();
            Cleared?.Invoke(this, null);
        }
    }
}
