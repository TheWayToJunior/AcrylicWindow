using System;
using System.Threading.Tasks;

namespace AcrylicWindow.Client.Core.IContract
{
    public interface IMessageBus
    {
        Task SendTo<TReceiver>(IMessage message);

        IDisposable Receive<TMessage>(object receiver, Func<TMessage, Task> handler)
            where TMessage : IMessage;
    }
}
