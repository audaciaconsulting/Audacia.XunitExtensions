using Xunit;
using Xunit.Sdk;

namespace Audacia.XunitExtensions.Retry
{
    /// <summary>
    /// Fact test the may intermittently fail.
    /// </summary>
    [XunitTestCaseDiscoverer("Audacia.XunitExtensions.Retry.BrittleFactDiscoverer", "Audacia.XunitExtensions")]
    public sealed class BrittleFactAttribute : FactAttribute, IRetryable
    {
        /// <inheritdoc />
        public int MaxRetries { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BrittleFactAttribute"/> class.
        /// </summary>
        /// <param name="maxRetries">The maximum number of retries that should be attempted.</param>
        public BrittleFactAttribute(int maxRetries = 1)
        {
            MaxRetries = maxRetries;
        }
    }
}