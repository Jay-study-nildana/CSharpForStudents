using System;
using Xunit;
using WebApiDotNetCore5point1SQLite;

namespace WebApiDotNetCore5point1SQLite.Tests
{
    public class UnitTest1
    {
        //a test that will always pass
        //we are just comparing the same thing to itself
        [Fact]
        public void TestThatAlwaysPasses()
        {
            Assert.Equal(4, 4);
        }

        //a test that will always fail
        //we are just comparing the thing to something else
        [Fact]
        public void TestThatAlwaysFails()
        {
            Assert.Equal(4, 2);
        }
    }
}
