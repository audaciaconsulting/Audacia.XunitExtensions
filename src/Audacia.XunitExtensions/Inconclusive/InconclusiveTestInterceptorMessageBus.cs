using System.Linq;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Audacia.XunitExtensions.Inconclusive
{
    /// <summary>
    /// Implementation of <see cref="IMessageBus"/> that checks if an inconclusive test exception has been raised.
    /// </summary>
    internal class InconclusiveTestInterceptorMessageBus : IMessageBus
    {
        private readonly IMessageBus _wrappedMessageBus;

        public bool InconclusiveTestResult { get; private set; }

        public InconclusiveTestInterceptorMessageBus(IMessageBus wrappedMessageBus)
        {
            _wrappedMessageBus = wrappedMessageBus;
        }

        public void Dispose()
        {
        }

        public bool QueueMessage(IMessageSinkMessage message)
        {
            if (message is TestFailed failedMessage &&
                failedMessage.ExceptionTypes.Any(ex => ex == typeof(InconclusiveTestResultException).FullName))
            {
                InconclusiveTestResult = true;
                message = new TestSkipped(
                    failedMessage.Test,
                    failedMessage.Messages.FirstOrDefault() ?? "Inconclusive test result.");
            }

            return _wrappedMessageBus.QueueMessage(message);
        }
    }
}