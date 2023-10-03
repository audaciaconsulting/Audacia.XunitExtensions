using System;
using System.Collections.Generic;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Audacia.XunitExtensions.Retry
{
    /// <summary>
    /// Implementation of <see cref="IXunitTestCaseDiscoverer"/> that discovers <see cref="BrittleFactAttribute"/>s.
    /// </summary>
    public class BrittleFactDiscoverer : IXunitTestCaseDiscoverer
    {
        private readonly IMessageSink _diagnosticMessageSink;

        /// <summary>
        /// Initializes a new instance of the <see cref="BrittleFactDiscoverer"/> class.
        /// </summary>
        /// <param name="diagnosticMessageSink">The message sink.</param>
        public BrittleFactDiscoverer(IMessageSink diagnosticMessageSink)
        {
            _diagnosticMessageSink = diagnosticMessageSink;
        }

        /// <inheritdoc />
        public IEnumerable<IXunitTestCase> Discover(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo factAttribute)
        {
            if (factAttribute == null) throw new ArgumentNullException(nameof(factAttribute));

            return DiscoverImpl();

            // Implement local function so that arguments are validated correctly
            // See https://github.com/JosefPihrt/Roslynator/blob/master/docs/analyzers/RCS1227.md
            IEnumerable<IXunitTestCase> DiscoverImpl()
            {
                var maxRetries = factAttribute.GetNamedArgument<int>("MaxRetries");
                if (maxRetries < 1)
                {
                    maxRetries = 3;
                }

                var methodDisplay = discoveryOptions.MethodDisplayOrDefault();
                var methodDisplayOptions = discoveryOptions.MethodDisplayOptionsOrDefault();
                yield return new BrittleTestCase(
                    _diagnosticMessageSink,
                    methodDisplay, 
                    methodDisplayOptions, 
                    testMethod, 
                    maxRetries);
            }
        }
    }
}