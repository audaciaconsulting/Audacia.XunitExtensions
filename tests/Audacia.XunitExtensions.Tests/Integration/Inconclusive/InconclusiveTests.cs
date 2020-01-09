using Audacia.XunitExtensions.Inconclusive;
using Xunit;

namespace Audacia.XunitExtensions.Tests.Integration.Inconclusive
{
    public class InconclusiveTests
    {
        [InconclusiveFact]
        public void Fact_Test_Marked_As_Skipped_If_Inconclusive_Test_Exception_Thrown()
        {
            throw new InconclusiveTestResultException("Inconclusive test message.");
        }

        [InconclusiveTheory]
        [InlineData(1)]
        public void Theory_Test_Marked_As_Skipped_If_Inconclusive_Test_Exception_Thrown(int i)
        {
            throw new InconclusiveTestResultException($"Inconclusive test message with parameter {i}.");
        }
    }
}