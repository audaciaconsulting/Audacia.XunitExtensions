﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Audacia.XunitExtensions.Retry
{
    /// <summary>
    /// Helper class to retry tests.
    /// </summary>
    public static class RetryTestRunner
    {
        /// <summary>
        /// Retries the given <paramref name="test"/> in the event of a test failure, up to <paramref name="maxRetries"/> times.
        /// </summary>
        /// <param name="test">The test to run.</param>
        /// <param name="maxRetries">The maximum number of times to retry in the event of failure.</param>
        /// <param name="testMethodDisplayName">The display name of the test method.</param>
        /// <param name="diagnosticMessageSink">The message sink.</param>
        /// <param name="messageBus">The message bus.</param>
        /// <param name="constructorArguments">The test constructor arguments.</param>
        /// <param name="aggregator">An exception aggregator.</param>
        /// <param name="cancellationTokenSource">The cancellation token source.</param>
        [SuppressMessage("Maintainability", "ACL1003:Signature contains too many parameters", Justification = "Needs all parameters.")]
        public static async Task<RunSummary> RetryTestAsync(
            Func<IMessageSink, IMessageBus, object[], ExceptionAggregator, CancellationTokenSource, Task<RunSummary>> test,
            int maxRetries,
            string testMethodDisplayName,
            IMessageSink diagnosticMessageSink,
            IMessageBus messageBus,
            object[] constructorArguments,
            ExceptionAggregator aggregator,
            CancellationTokenSource cancellationTokenSource)
        {
            var runCount = 0;

            while (true)
            {
                // This is really the only tricky bit: we need to capture and delay messages (since those will
                // contain run status) until we know we've decided to accept the final result;
#pragma warning disable IDISP001 // Dispose created - deliberate as we only want to dispose (which sends the messages) in certain circumstances
                var delayedMessageBus = new DelayedMessageBus(messageBus);
#pragma warning restore IDISP001 // Dispose created.

                var summary = await test(
                    diagnosticMessageSink,
                    delayedMessageBus,
                    constructorArguments,
                    aggregator,
                    cancellationTokenSource);

                if (aggregator.HasExceptions || summary.Failed == 0 || runCount >= maxRetries)
                {
                    delayedMessageBus.Dispose(); // Sends all the delayed messages

                    return summary;
                }

                runCount++;
                var message = new DiagnosticMessage(
                    "Execution of '{0}' failed (attempt #{1}), retrying...",
                    testMethodDisplayName,
                    runCount);

                diagnosticMessageSink.OnMessage(message);
            }
        }
    }
}