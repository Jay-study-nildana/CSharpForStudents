using System;

/// <summary>
/// Problem: Cohesion_Assessment
/// Presents a high-cohesion class and a low-cohesion "God" class stub with comments.
/// </summary>
class Cohesion_Assessment
{
    // High-cohesion: TemperatureSensor concerns only reading and converting temperature
    public class TemperatureSensor
    {
        private readonly Func<double> _readRaw;
        public TemperatureSensor(Func<double> readRaw) => _readRaw = readRaw;
        public double ReadCelsius() => _readRaw();
        public double ReadFahrenheit() => ReadCelsius() * 9 / 5 + 32;
    }

    // Low-cohesion (bad): one class doing many unrelated things
    public class GodClass
    {
        public void ReadSensor() { /* reads sensor */ }
        public void SaveToDb() { /* DB logic */ }
        public void RenderHtml() { /* HTML rendering */ }
        public void ComputeAnalytics() { /* heavy computation */ }
    }

    static void Main()
    {
        Console.WriteLine("High cohesion class: TemperatureSensor (single responsibility).");
        Console.WriteLine("Low cohesion class: GodClass (too many unrelated responsibilities). Prefer splitting into focused classes.");
    }
}