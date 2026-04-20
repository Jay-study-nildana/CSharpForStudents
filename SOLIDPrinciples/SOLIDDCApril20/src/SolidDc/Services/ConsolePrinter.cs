using System;
using SolidDc.Services;

namespace SolidDc.Services
{
    // A concrete printer that writes to the console. In real apps
    // this could be swapped out for a file-printer or HTML exporter.
    public class ConsolePrinter<T> : IPrinter<T>
    {
        public void Print(T item)
        {
            Console.WriteLine(item?.ToString());
        }
    }
}
