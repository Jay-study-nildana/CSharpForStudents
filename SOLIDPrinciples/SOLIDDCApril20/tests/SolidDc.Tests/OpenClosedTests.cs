using Xunit;
using SolidDc.Principles;

namespace SolidDc.Tests
{
    public class OpenClosedTests
    {
        [Fact]
        public void OCP_Run_Completes()
        {
            OpenClosedExample.Run();
        }
    }
}
