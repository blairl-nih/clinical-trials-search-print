using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace NCI.OCPL.ClinicalTrialSearchPrint.Tests
{
    public class TestTests
    {
        [Fact]
        public void PassingTest()
        {
            Assert.True(true);
        }

        [Fact]
        public void FailingTest()
        {
            Assert.False(true);
        }
    }
}
