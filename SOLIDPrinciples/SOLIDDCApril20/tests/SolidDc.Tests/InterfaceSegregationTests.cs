using Xunit;
using SolidDc.Principles;

namespace SolidDc.Tests
{
    public class InterfaceSegregationTests
    {
        [Fact]
        public void ISP_Run_Completes()
        {
            InterfaceSegregationExample.Run();
        }
    }
}
