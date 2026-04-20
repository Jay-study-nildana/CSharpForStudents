using Xunit;
using SolidDc.Principles;

namespace SolidDc.Tests
{
    public class LiskovTests
    {
        [Fact]
        public void LSP_Run_Completes()
        {
            LiskovSubstitutionExample.Run();
        }
    }
}
