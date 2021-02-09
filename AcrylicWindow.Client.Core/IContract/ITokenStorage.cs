using AcrylicWindow.Client.Core.Models;

namespace AcrylicWindow.Client.Core.IContract
{
    public interface IReaderTokenStore : IChangeable<TokenEventArgs>, IClearable
    {
        string this[string key] { get; set; }

        string GetValue(string key);
    }

    public interface IWriterTokenStore : IChangeable<TokenEventArgs>, IClearable
    {
        string AddOrUpdate(string key, string value);
    }

    public interface ITokenStorage : IReaderTokenStore, IWriterTokenStore
    {
        bool TryRemove(string key, out string value);

        void RemoveAll();
    }
}
