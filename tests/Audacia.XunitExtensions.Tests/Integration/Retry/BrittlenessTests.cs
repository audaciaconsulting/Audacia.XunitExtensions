using System;
using Audacia.XunitExtensions.Retry;
using Xunit;

namespace Audacia.XunitExtensions.Tests.Integration.Retry
{
    public class BrittlenessTests
    {
        private static int _theoryCounter = 0;
        private static int _factCounter = 0;

        [BrittleTheory(3)]
        [InlineData("Richard")]
        [InlineData("Bob")]
        public void Brittle_Theory_Tests_Pass_If_They_Stop_Failing_Before_The_Retry_Count_Is_Reached(string name)
        {
            if (_theoryCounter == 0)
            {
                _theoryCounter += 1;
                throw new InvalidOperationException($"First try with name {name}.");
            }

            _theoryCounter += 1;
        }

        [BrittleFact(3)]
        public void Brittle_Fact_Tests_Pass_If_They_Stop_Failing_Before_The_Retry_Count_Is_Reached()
        {
            if (_factCounter == 0)
            {
                _factCounter += 1;
                throw new InvalidOperationException("First try");
            }

            _factCounter += 1;
        }
    }
}
