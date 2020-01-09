using System.Collections.Generic;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Audacia.XunitExtensions.Inconclusive
{
    /// <summary>
    /// Implementation of <see cref="IXunitTestCaseDiscoverer"/> that discovers <see cref="InconclusiveFactAttribute"/>s.
    /// </summary>
    public class InconclusiveFactDiscoverer : IXunitTestCaseDiscoverer
    {
        private readonly IMessageSink _diagnosticMessageSink;

        /// <summary>
        /// Initializes a new instance of the <see cref="InconclusiveFactDiscoverer"/> class.
        /// </summary>
        /// <param name="diagnosticMessageSink">The message sink.</param>
        public InconclusiveFactDiscoverer(IMessageSink diagnosticMessageSink)
        {
            _diagnosticMessageSink = diagnosticMessageSink;
        }

        /// <inheritdoc />
        public IEnumerable<IXunitTestCase> Discover(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo factAttribute)
        {
            yield return new InconclusiveTestCase(_diagnosticMessageSink, discoveryOptions.MethodDisplayOrDefault(), discoveryOptions.MethodDisplayOptionsOrDefault(), testMethod);
        }
    }
}