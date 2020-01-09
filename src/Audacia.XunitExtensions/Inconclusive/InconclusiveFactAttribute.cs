using Xunit;
using Xunit.Sdk;

namespace Audacia.XunitExtensions.Inconclusive
{
    /// <summary>
    /// Fact test that can produce an inconclusive result.
    /// An <see cref="InconclusiveTestResultException"/> should be thrown to produce such a result.
    /// </summary>
    [XunitTestCaseDiscoverer("Audacia.XunitExtensions.Inconclusive.InconclusiveFactDiscoverer", "Audacia.XunitExtensions")]
    public sealed class InconclusiveFactAttribute : FactAttribute
    {
    }
}