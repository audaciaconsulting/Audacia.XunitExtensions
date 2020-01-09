using System.Collections.Generic;
using System.Linq;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Audacia.XunitExtensions.Inconclusive
{
    /// <summary>
    /// Implementation of <see cref="IXunitTestCaseDiscoverer"/> that discovers <see cref="InconclusiveTheoryAttribute"/>s.
    /// </summary>
    public class InconclusiveTheoryDiscoverer : TheoryDiscoverer
    {
        private readonly IMessageSink _diagnosticMessageSink;

        /// <summary>
        /// Initializes a new instance of the <see cref="InconclusiveTheoryDiscoverer"/> class.
        /// </summary>
        /// <param name="diagnosticMessageSink">The message sink.</param>
        public InconclusiveTheoryDiscoverer(IMessageSink diagnosticMessageSink) : base(diagnosticMessageSink)
        {
            _diagnosticMessageSink = diagnosticMessageSink;
        }

        /// <inheritdoc />
        public override IEnumerable<IXunitTestCase> Discover(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo theoryAttribute)
        {
            var results = base.Discover(discoveryOptions, testMethod, theoryAttribute).ToArray();

            for (int i = 0; i < results.Length; i++)
            {
                var current = results[i];
                if (current is XunitTheoryTestCase || current is XunitTestCase)
                {
                    // Replace XUnitTheoryTestCase with our 'Inconclusive' implementation
                    // Anything else will be a skipped test or an error condition, so they can stay as-is
                    results[i] = new InconclusiveTheoryTestCase(
                        _diagnosticMessageSink,
                        discoveryOptions.MethodDisplayOrDefault(),
                        discoveryOptions.MethodDisplayOptionsOrDefault(),
                        testMethod);
                }
            }

            return results;
        }
    }
}