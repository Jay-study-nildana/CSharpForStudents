using System;
using System.Collections.Generic;

class Vehicle_Start_Virtual
{
    public class Vehicle
    {
        public virtual void Start() => Console.WriteLine("Vehicle starting (generic).");
    }

    public class Car : Vehicle
    {
        public override void Start() => Console.WriteLine("Car: turning key, engine starts.");
    }

    public class ElectricScooter : Vehicle
    {
        public override void Start() => Console.WriteLine("ElectricScooter: powering on quietly.");
    }

    public class DieselTruck : Vehicle
    {
        public override void Start() => Console.WriteLine("DieselTruck: cranking diesel engine.");
    }

    static void Main()
    {
        var vehicles = new List<Vehicle> { new Car(), new ElectricScooter(), new DieselTruck() };
        foreach (var v in vehicles) v.Start();

        // virtual used because base provides default but derived types need specific behavior.
    }
}