using System.Collections.Generic;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Audacia.XunitExtensions.Retry
{
    internal sealed class DelayedMessageBus : IMessageBus
    {
        private readonly IMessageBus _innerBus;
        private readonly object _messagesLock = new object();
        private readonly List<IMessageSinkMessage> _messages = new List<IMessageSinkMessage>();

        public DelayedMessageBus(IMessageBus innerBus)
        {
            _innerBus = innerBus;
        }

        public bool QueueMessage(IMessageSinkMessage message)
        {
            lock (_messagesLock)
            {
                _messages.Add(message);
            }

            // No way to ask the inner bus if they want to cancel without sending them the message, so
            // we just go ahead and continue always.
            return true;
        }

        public void Dispose()
        {
            foreach (var message in _messages)
            {
                _innerBus.QueueMessage(message);
            }
        }
    }
}