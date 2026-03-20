using System;
using System.Linq;

class Car_Defensive_Copying
{
    // Demonstrates defensive copying of arrays/lists to protect internal state.
    public class Car
    {
        private readonly string[] _features; // internal copy
        public string Model { get; }
        public string[] Features => (string[])_features.Clone(); // defensive copy on access

        public Car(string model, string[] features)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
            _features = features == null ? Array.Empty<string>() : (string[])features.Clone(); // defensive copy in ctor
        }

        public override string ToString() => $"{Model}: {string.Join(", ", _features)}";
    }

    static void Main()
    {
        var external = new string[] { "AC", "Radio" };
        var car = new Car("Sedan", external);

        // Modify external array after construction
        external[0] = "Broken AC";
        Console.WriteLine("External after change: " + string.Join(", ", external));
        Console.WriteLine("Car features (protected): " + string.Join(", ", car.Features));

        // Modify Features property copy - does not affect internal array
        var f = car.Features;
        f[0] = "Hacked";
        Console.WriteLine("Car features after modifying returned copy: " + string.Join(", ", car.Features));
    }
}