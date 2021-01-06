using System;

namespace AcrylicWindow.Client.Core.Models
{
    public class TokenEventArgs : EventArgs
    {
        public string Key { get; set; }

        public string Value { get; set; }

        public string OldValue { get; set; }

        public TokenEventArgs(string key, string value, string oldValue)
        {
            Key = key;
            Value = value;
            OldValue = oldValue;
        }
    }
}
