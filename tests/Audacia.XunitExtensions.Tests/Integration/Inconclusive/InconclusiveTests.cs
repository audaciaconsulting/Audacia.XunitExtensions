using Audacia.XunitExtensions.Inconclusive;
using Xunit;

namespace Audacia.XunitExtensions.Tests.Integration.Inconclusive
{
    public class InconclusiveTests
    {

        {
            throw new InconclusiveTestResultException($"Inconclusive test message with parameter {i}.");
        }
    }
}