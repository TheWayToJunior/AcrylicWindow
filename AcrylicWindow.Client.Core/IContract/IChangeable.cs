using System;

namespace AcrylicWindow.Client.Core.IContract
{
    public interface IChangeable<T>
    {
        event EventHandler<T> Changed;
    }

    public interface IClearable
    {
        event EventHandler Cleared;
    }
}
