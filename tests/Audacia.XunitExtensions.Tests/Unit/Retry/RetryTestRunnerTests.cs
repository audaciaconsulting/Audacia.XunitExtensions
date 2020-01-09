using System.Threading;
using System.Threading.Tasks;
using Audacia.XunitExtensions.Retry;
using FluentAssertions;
using Moq;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Audacia.XunitExtensions.Tests.Unit.Retry
{
    public class RetryTestRunnerTests
    {
        private readonly Mock<IMessageBus> _messageBusMock = new Mock<IMessageBus>();
        private readonly Mock<IMessageSink> _messageSinkMock = new Mock<IMessageSink>();

        /*
         * 1. Retries up to max count, then fails
         * 2. Doesn't retry if passes
         * 3. Writes message bus before exiting
         */

        [Fact]
        public async Task Retries_Up_To_Maximum_Count_Then_Fails()
        {
            var executionCount = 0;

            Task<RunSummary> Executor(IMessageSink messageSink, IMessageBus messageBus, object[] constructorArgs, ExceptionAggregator exceptionAggregator, CancellationTokenSource cancellationTokenSource)
            {
                executionCount++;
                return Task.FromResult(new RunSummary { Failed = 1 });
            }

            const int retryCount = 2;
            await RetryTestRunner.RetryTestAsync(
                Executor,
                retryCount,
                "Test",
                _messageSinkMock.Object,
                _messageBusMock.Object,
                null,
                new ExceptionAggregator(),
                null);

            executionCount.Should().Be(retryCount + 1);
        }

        [Fact]
        public async Task Does_Not_Retry_If_Test_Passes()
        {
            var executionCount = 0;

            Task<RunSummary> Executor(IMessageSink messageSink, IMessageBus messageBus, object[] constructorArgs, ExceptionAggregator exceptionAggregator, CancellationTokenSource cancellationTokenSource)
            {
                executionCount++;
                return Task.FromResult(new RunSummary());
            }

            const int retryCount = 2;
            await RetryTestRunner.RetryTestAsync(
                Executor,
                retryCount,
                "Test",
                _messageSinkMock.Object,
                _messageBusMock.Object,
                null,
                new ExceptionAggregator(),
                null);

            executionCount.Should().Be(1);
        }

        [Fact]
        public async Task Writes_To_Message_Bus_Before_Exiting()
        {
            var expectedMessage = new DiagnosticMessage();
            Task<RunSummary> Executor(IMessageSink messageSink, IMessageBus messageBus, object[] constructorArgs, ExceptionAggregator exceptionAggregator, CancellationTokenSource cancellationTokenSource)
            {
                messageBus.QueueMessage(expectedMessage);
                return Task.FromResult(new RunSummary());
            }

            const int retryCount = 2;
            await RetryTestRunner.RetryTestAsync(
                Executor,
                retryCount,
                "Test",
                _messageSinkMock.Object,
                _messageBusMock.Object,
                null,
                new ExceptionAggregator(),
                null);

            _messageBusMock.Verify(b => b.QueueMessage(expectedMessage));
        }
    }
}