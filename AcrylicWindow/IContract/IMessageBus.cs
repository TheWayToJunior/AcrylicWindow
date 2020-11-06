using System;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;

namespace AcrylicWindow.IContract
{
    public interface IMessageBus
    {
        Task SendTo<TReceiver>(IMessage message);

        IDisposable Receive<TMessage>(object receiver, Func<TMessage, Task> handler)
            where TMessage : IMessage;
    }
}
