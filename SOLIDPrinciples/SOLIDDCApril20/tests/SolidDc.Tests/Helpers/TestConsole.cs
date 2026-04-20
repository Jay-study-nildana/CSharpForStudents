using System;
using System.IO;

namespace SolidDc.Tests.Helpers
{
    // Helper to capture console output during tests.
    public sealed class TestConsole : IDisposable
    {
        private readonly StringWriter _writer = new();
        private readonly TextWriter _originalOut = Console.Out;

        public TestConsole() => Console.SetOut(_writer);

        public string GetOutput() => _writer.ToString();

        public void Dispose()
        {
            Console.SetOut(_originalOut);
            _writer.Dispose();
        }
    }
}
