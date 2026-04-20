using SolidDc.Principles;
using Xunit;

namespace SolidDc.Tests
{
    public class SingleResponsibilityTests
    {
        [Fact]
        public void SRP_Run_Completes()
        {
            // Ensure the SRP example runs without throwing
            SingleResponsibilityExample.Run();
        }
    }
}
