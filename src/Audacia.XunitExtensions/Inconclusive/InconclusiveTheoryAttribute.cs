using Xunit;
using Xunit.Sdk;

namespace Audacia.XunitExtensions.Inconclusive
{
    /// <summary>
    /// Theory test that can produce an inconclusive result.
    /// An <see cref="InconclusiveTestResultException"/> should be thrown to produce such a result.
    /// </summary>
    [XunitTestCaseDiscoverer("Audacia.XunitExtensions.Inconclusive.InconclusiveTheoryDiscoverer", "Audacia.XunitExtensions")]
    public sealed class InconclusiveTheoryAttribute : TheoryAttribute
    {
    }
}