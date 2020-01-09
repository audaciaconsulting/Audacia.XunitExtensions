using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Audacia.XunitExtensions.Retry
{
    /// <summary>
    /// Subclass of <see cref="XunitTheoryTestCase"/> that runs test methods decorated with <see cref="BrittleFactAttribute"/>.
    /// </summary>
    [Serializable]
    public class BrittleTestCase : XunitTestCase
    {
        private int _maxRetries;

        /// <summary>
        /// Initializes a new instance of the <see cref="BrittleTestCase"/> class.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Called by the de-serializer", true)]
        public BrittleTestCase() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BrittleTestCase"/> class.
        /// </summary>
        /// <param name="diagnosticMessageSink">The message sink.</param>
        /// <param name="testMethodDisplay">Test method display data.</param>
        /// <param name="testMethodDisplayOptions">Test method display options.</param>
        /// <param name="testMethod">The test method.</param>
        /// <param name="maxRetries">The maximum number of retries.</param>
        public BrittleTestCase(
            IMessageSink diagnosticMessageSink,
            TestMethodDisplay testMethodDisplay,
            TestMethodDisplayOptions testMethodDisplayOptions,
            ITestMethod testMethod,
            int maxRetries)
            : base(diagnosticMessageSink, testMethodDisplay, testMethodDisplayOptions, testMethod, testMethodArguments: null)
        {
            _maxRetries = maxRetries;
        }

        /// <inheritdoc />
        public override Task<RunSummary> RunAsync(
            IMessageSink diagnosticMessageSink,
            IMessageBus messageBus,
            object[] constructorArguments,
            ExceptionAggregator aggregator,
            CancellationTokenSource cancellationTokenSource)
        {
            return RetryTestRunner.RetryTestAsync(
                base.RunAsync,
                _maxRetries,
                DisplayName,
                diagnosticMessageSink,
                messageBus,
                constructorArguments,
                aggregator,
                cancellationTokenSource);
        }

        /// <inheritdoc />
        public override void Serialize(IXunitSerializationInfo data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            base.Serialize(data);

            data.AddValue("MaxRetries", _maxRetries);
        }

        /// <inheritdoc />
        public override void Deserialize(IXunitSerializationInfo data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            base.Deserialize(data);

            _maxRetries = data.GetValue<int>("MaxRetries");
        }
    }
}