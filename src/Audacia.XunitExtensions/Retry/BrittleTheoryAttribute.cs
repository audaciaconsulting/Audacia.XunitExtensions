using Xunit;
using Xunit.Sdk;

namespace Audacia.XunitExtensions.Retry
{
    /// <summary>
    /// Theory test that may intermittently fail.
    /// </summary>
    [XunitTestCaseDiscoverer("Audacia.XunitExtensions.Retry.BrittleTheoryDiscoverer", "Audacia.XunitExtensions")]
    public sealed class BrittleTheoryAttribute : TheoryAttribute, IRetryable
    {
        /// <inheritdoc />
        public int MaxRetries { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BrittleTheoryAttribute"/> class.
        /// </summary>
        /// <param name="maxRetries">The maximum number of retries that should be attempted.</param>
        public BrittleTheoryAttribute(int maxRetries = 2)
        {
            MaxRetries = maxRetries;
        }
    }
}