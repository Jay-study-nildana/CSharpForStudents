using System;
using System.IO;

namespace Capstone.Core.Utils
{
    // Centralized app configuration helpers. Consumers can override paths in tests.
    public static class AppConfig
    {
        public static string DefaultDataDirectory => Path.Combine(Environment.CurrentDirectory, "data");
        public static string DefaultDataFile => Path.Combine(DefaultDataDirectory, "tasks.json");
    }
}
