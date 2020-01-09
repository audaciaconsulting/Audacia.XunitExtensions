using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Audacia.XunitExtensions.Inconclusive
{
    /// <summary>
    /// Subclass of <see cref="XunitTestCase"/> that runs tests decorated with <see cref="InconclusiveFactAttribute"/>.
    /// </summary>
    [Serializable]
    public class InconclusiveTestCase : XunitTestCase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InconclusiveTestCase"/> class.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Called by the de-serializer", true)]
        public InconclusiveTestCase() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InconclusiveTestCase"/> class.
        /// </summary>
        /// <param name="diagnosticMessageSink">The message sink.</param>
        /// <param name="testMethodDisplay">Test method display data.</param>
        /// <param name="testMethodDisplayOptions">Test method display options.</param>
        /// <param name="testMethod">The test method.</param>
        public InconclusiveTestCase(
            IMessageSink diagnosticMessageSink,
            TestMethodDisplay testMethodDisplay,
            TestMethodDisplayOptions testMethodDisplayOptions,
            ITestMethod testMethod)
            : base(diagnosticMessageSink, testMethodDisplay, testMethodDisplayOptions, testMethod, testMethodArguments: null)
        {
        }

        /// <inheritdoc />
        public override async Task<RunSummary> RunAsync(
            IMessageSink diagnosticMessageSink,
            IMessageBus messageBus,
            object[] constructorArguments,
            ExceptionAggregator aggregator,
            CancellationTokenSource cancellationTokenSource)
        {
            var wrapperMessageBus = new InconclusiveTestInterceptorMessageBus(messageBus);
            var summary = await base.RunAsync(diagnosticMessageSink, wrapperMessageBus, constructorArguments, aggregator, cancellationTokenSource);

            if (wrapperMessageBus.InconclusiveTestResult)
            {
                summary.Failed = 0;
                summary.Skipped = 1;
            }

            return summary;
        }
    }
}