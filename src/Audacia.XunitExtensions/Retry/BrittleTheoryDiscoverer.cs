using System;
using System.Collections.Generic;
using System.Linq;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Audacia.XunitExtensions.Retry
{
    /// <summary>
    /// Implementation of <see cref="IXunitTestCaseDiscoverer"/> that discovers <see cref="BrittleTheoryAttribute"/>s.
    /// </summary>
    public class BrittleTheoryDiscoverer : TheoryDiscoverer
    {
        private readonly IMessageSink _diagnosticMessageSink;

        /// <summary>
        /// Initializes a new instance of the <see cref="BrittleTheoryDiscoverer"/> class.
        /// </summary>
        /// <param name="diagnosticMessageSink">The message sink.</param>
        public BrittleTheoryDiscoverer(IMessageSink diagnosticMessageSink) : base(diagnosticMessageSink)
        {
            _diagnosticMessageSink = diagnosticMessageSink;
        }

        /// <inheritdoc />
        public override IEnumerable<IXunitTestCase> Discover(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo theoryAttribute)
        {
            if (theoryAttribute == null) throw new ArgumentNullException(nameof(theoryAttribute));

            var results = base.Discover(discoveryOptions, testMethod, theoryAttribute).ToArray();

            for (int i = 0; i < results.Length; i++)
            {
                var current = results[i];
                if (current is XunitTheoryTestCase || current is XunitTestCase)
                {
                    // Replace XUnitTheoryTestCase with our 'Brittle' implementation
                    // Anything else will be a skipped test or an error condition, so they can stay as-is
                    var maxRetries = theoryAttribute.GetNamedArgument<int>("MaxRetries");
                    if (maxRetries < 1)
                    {
                        maxRetries = 3;
                    }

                    results[i] = new BrittleTheoryTestCase(
                        _diagnosticMessageSink,
                        discoveryOptions.MethodDisplayOrDefault(),
                        discoveryOptions.MethodDisplayOptionsOrDefault(),
                        testMethod,
                        maxRetries);
                }
            }

            return results;
        }
    }
}