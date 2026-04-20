using Xunit;
using SolidDc.Principles;

namespace SolidDc.Tests
{
    public class DependencyInversionTests
    {
        [Fact]
        public void DIP_Run_Completes()
        {
            DependencyInversionExample.Run();
        }
    }
}
