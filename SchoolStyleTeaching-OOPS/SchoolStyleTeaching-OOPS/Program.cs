// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

//Uncomment and comment each region to learn individual OOP concepts

#region intro to OOP

//// Display a welcome message.
//Console.WriteLine("Welcome to Formula 1 OOP Example!");

//// Create a new Car object and set its properties.
//Car ferrariCar = new Car
//{
//    Model = "SF-24",
//    MaxSpeed = 360,
//    Team = "Ferrari"
//};

//// Call the Race method to simulate the car racing.
//ferrariCar.Race();

//// You can create more Car objects for other teams and models.
//// This demonstrates how OOP helps model real-world entities in code.

//// Object-Oriented Programming (OOP) models real-world entities as "objects".
//// In Formula 1, we can represent cars, drivers, and teams as objects.

//// Define a Car class to represent a Formula 1 car.
//public class Car
//{
//    // The model of the car (e.g., "SF-24", "W15").
//    public string Model { get; set; }

//    // The maximum speed the car can reach (in km/h).
//    public int MaxSpeed { get; set; }

//    // The team the car belongs to (e.g., "Ferrari", "Mercedes").
//    public string Team { get; set; }

//    // Method to simulate the car racing.
//    public void Race()
//    {
//        // Print a message showing which car is racing and its speed.
//        Console.WriteLine($"{Model} from {Team} is racing at {MaxSpeed} km/h!");
//    }
//}

#endregion

#region reusability and maintainability

//// Code organization, reusability, and maintainability are key benefits of OOP.
//// Let's improve our Formula 1 example by introducing a base class and inheritance.

//// Create a Car object using the improved class structure.
//Car ferrariCar = new Car
//{
//    Model = "SF-24",
//    MaxSpeed = 360,
//    Team = "Ferrari"
//};

//// Reusable method from base class.
//ferrariCar.ShowInfo();

//// Car-specific method.
//ferrariCar.Race();

//// You can easily add more vehicle types (e.g., Truck, SafetyCar) by inheriting from Vehicle.
//// This makes the code organized, reusable, and easy to maintain.

//// Base class representing a general vehicle in Formula 1.
//public class Vehicle
//{
//    public string Model { get; set; }
//    public int MaxSpeed { get; set; }

//    // Method to display basic vehicle info.
//    public virtual void ShowInfo()
//    {
//        Console.WriteLine($"Model: {Model}, Max Speed: {MaxSpeed} km/h");
//    }
//}

//// Car class inherits from Vehicle, adding specific properties.
//public class Car : Vehicle
//{
//    // The team the car belongs to.
//    public string Team { get; set; }

//    // Override ShowInfo to include team information.
//    public override void ShowInfo()
//    {
//        Console.WriteLine($"{Model} from {Team} has a max speed of {MaxSpeed} km/h.");
//    }

//    // Method to simulate the car racing.
//    public void Race()
//    {
//        Console.WriteLine($"{Model} from {Team} is racing at {MaxSpeed} km/h!");
//    }
//}



#endregion

#region set and get 

//// Fields and Properties in C#
//// Fields are variables declared inside a class.
//// Properties use get/set accessors to control access to fields.

//// Usage example (top-level statements, no Main method needed)
//Driver verstappen = new Driver();
//verstappen.Name = "Max Verstappen";         // Using property to set name
//verstappen.Nationality = "Dutch";           // Using auto-property
//verstappen.RaceWins = 60;                   // Using property to set race wins

//Console.WriteLine($"{verstappen.Name} ({verstappen.Nationality}) has {verstappen.RaceWins} race wins.");

//// Formula 1 themed example: Driver class
//public class Driver
//{
//    // Field: stores the driver's name (private, not directly accessible outside the class)
//    private string name;

//    // Property: allows controlled access to the 'name' field
//    public string Name
//    {
//        get { return name; }           // Getter: returns the value of 'name'
//        set { name = value; }          // Setter: sets the value of 'name'
//    }

//    // Auto-implemented property for Nationality (no explicit field needed)
//    public string Nationality { get; set; }

//    // Field: stores the number of race wins (private)
//    private int raceWins;

//    // Property: allows controlled access to 'raceWins'
//    public int RaceWins
//    {
//        get { return raceWins; }
//        set
//        {
//            // Only allow non-negative values
//            if (value >= 0)
//                raceWins = value;
//        }
//    }
//}

//// The getter (get) is used to retrieve the value of a private field, providing controlled read access from outside the class.
//// The setter (set) is used to assign a value to a private field, providing controlled write access from outside the class.
////
//// In most .NET Web API projects with Entity Framework, you typically use auto-implemented properties ({ get; set; }) 
//// because EF requires public properties for mapping and handles data access automatically.
////
//// You rarely need custom getters and setters in EF models unless you want to add validation, transformation, or logic 
//// when reading or writing property values.
////
//// Use custom getters and setters extensively when you need to:
////   - Validate input before assigning to a field (e.g., prevent negative values)
////   - Trigger additional actions when a property changes (e.g., update related data)
////   - Hide or transform internal data before exposing it (e.g., format output)
////   - Implement business rules or security checks on property access
////
//// For simple data models, auto-properties are preferred for simplicity and compatibility with EF.
//// Custom logic is added only when necessary.

#endregion

#region access modifiers

//// Access Modifiers in C#
//// Access modifiers control the visibility and accessibility of class members.
//// public: accessible from anywhere
//// private: accessible only within the same class
//// protected: accessible within the class and derived classes

//// Usage example (top-level statements)
//Team ferrari = new Team();
//ferrari.Name = "Ferrari";
//ferrari.SetBudget(500_000_000);

//Driver leclerc = new Driver();
//leclerc.Name = "Ferrari";
//leclerc.DriverName = "Charles Leclerc";

//// Accessing public members
//Console.WriteLine($"{ferrari.Name} team budget: ${ferrari.GetBudget()}");

//// Accessing protected member via derived class method
//leclerc.DisplayChampionships();

//// Formula 1 themed example: Team and Driver

//public class Team
//{
//    // public property: accessible from anywhere
//    public string Name { get; set; }

//    // private field: only accessible within Team class
//    private double budget;

//    // protected property: accessible in Team and derived classes
//    protected int ChampionshipsWon { get; set; }

//    // public method to set the budget (demonstrates controlled access)
//    public void SetBudget(double amount)
//    {
//        // Only allow positive budget values
//        if (amount > 0)
//            budget = amount;
//    }

//    // public method to get the budget (read-only access)
//    public double GetBudget()
//    {
//        return budget;
//    }
//}

//public class Driver : Team
//{
//    // public property: accessible from anywhere
//    public string DriverName { get; set; }

//    // Method to access protected member from base class
//    public void DisplayChampionships()
//    {
//        Console.WriteLine($"{DriverName} drives for {Name} with {ChampionshipsWon} team championships.");
//    }
//}


#endregion

#region inheritance

//// Inheritance in C#
//// Inheritance allows you to create new classes based on existing ones, enabling code reuse and organization.
//// The base class contains common properties and methods, while derived classes add or override functionality.

//// Usage example (top-level statements)
//Car redBullCar = new Car { Model = "RB20", MaxSpeed = 355, Team = "Red Bull" };
//redBullCar.ShowInfo();
//redBullCar.Race();

//SafetyCar mercedesSafetyCar = new SafetyCar { Model = "AMG GT Black Series", MaxSpeed = 300, SafetyFeature = "Track Control" };
//mercedesSafetyCar.ShowInfo();
//mercedesSafetyCar.Deploy();
////
//// Example (Formula 1 themed):
//// - Vehicle is a base class for all vehicles in Formula 1.
//// - Car and SafetyCar are derived classes that inherit from Vehicle and add their own features.

//public class Vehicle
//{
//    public string Model { get; set; }
//    public int MaxSpeed { get; set; }

//    public void ShowInfo()
//    {
//        Console.WriteLine($"Model: {Model}, Max Speed: {MaxSpeed} km/h");
//    }
//}

//// Car inherits from Vehicle and adds a Team property
//public class Car : Vehicle
//{
//    public string Team { get; set; }

//    public void Race()
//    {
//        Console.WriteLine($"{Model} from {Team} is racing at {MaxSpeed} km/h!");
//    }
//}

//// SafetyCar inherits from Vehicle and adds a SafetyFeature property
//public class SafetyCar : Vehicle
//{
//    public string SafetyFeature { get; set; }

//    public void Deploy()
//    {
//        Console.WriteLine($"{Model} deployed with {SafetyFeature} at {MaxSpeed} km/h.");
//    }
//}



//// Inheritance helps you avoid repeating code and makes it easy to add new vehicle types with shared functionality.

#endregion

#region base and derived classes

// Base and Derived Classes in C#
// A base class (parent) defines common properties and methods that can be shared by multiple classes.
// A derived class (child) inherits from the base class and can add or override functionality.
//

//// Usage example (top-level statements)
//Car ferrariCar = new Car { Model = "SF-24", MaxSpeed = 360, Team = "Ferrari" };
//ferrariCar.ShowInfo();
//ferrariCar.Race();

//SafetyCar mercedesSafetyCar = new SafetyCar { Model = "AMG GT Black Series", MaxSpeed = 300, SafetyFeature = "Track Control" };
//mercedesSafetyCar.ShowInfo();
//mercedesSafetyCar.Deploy();
//// Example (Formula 1 themed):
//// - Vehicle is the base class (parent) for all vehicles in Formula 1.
//// - Car and SafetyCar are derived classes (children) that inherit from Vehicle.

//public class Vehicle // Base class (parent)
//{
//    public string Model { get; set; }
//    public int MaxSpeed { get; set; }

//    public void ShowInfo()
//    {
//        Console.WriteLine($"Model: {Model}, Max Speed: {MaxSpeed} km/h");
//    }
//}

//public class Car : Vehicle // Derived class (child)
//{
//    public string Team { get; set; }

//    public void Race()
//    {
//        Console.WriteLine($"{Model} from {Team} is racing at {MaxSpeed} km/h!");
//    }
//}

//public class SafetyCar : Vehicle // Derived class (child)
//{
//    public string SafetyFeature { get; set; }

//    public void Deploy()
//    {
//        Console.WriteLine($"{Model} deployed with {SafetyFeature} at {MaxSpeed} km/h.");
//    }
//}


// The base class provides shared functionality, while derived classes extend or customize it for specific needs.

#endregion

#region Overriding

// Overriding Methods in C#
// Overriding allows a derived class to change or extend the behavior of a method defined in its base class.
// The base class method must be marked as 'virtual', and the derived class uses the 'override' keyword.
//

//// Usage example (top-level statements)
//Vehicle genericVehicle = new Vehicle { Model = "Generic F1 Vehicle", MaxSpeed = 320 };
//genericVehicle.ShowInfo(); // Calls base class method

//Car ferrariCar = new Car { Model = "SF-24", MaxSpeed = 360, Team = "Ferrari" };
//ferrariCar.ShowInfo(); // Calls overridden method in Car

//// Example (Formula 1 themed):
//// - Vehicle has a virtual method ShowInfo.
//// - Car overrides ShowInfo to include team information.

//public class Vehicle
//{
//    public string Model { get; set; }
//    public int MaxSpeed { get; set; }

//    // Virtual method: can be overridden in derived classes
//    public virtual void ShowInfo()
//    {
//        Console.WriteLine($"Model: {Model}, Max Speed: {MaxSpeed} km/h");
//    }
//}

//public class Car : Vehicle
//{
//    public string Team { get; set; }

//    // Override the base class method to add more details
//    public override void ShowInfo()
//    {
//        Console.WriteLine($"{Model} from {Team} has a max speed of {MaxSpeed} km/h.");
//    }
//}



// Overriding lets you customize or extend base class behavior for specific scenarios.

#endregion

#region Polymorphism

//// Polymorphism in C#
//// Polymorphism means "many forms" and allows you to use a base class reference to work with objects of derived classes.
//// This enables flexible code that can handle different types of objects in a uniform way.
////

//// Usage example (top-level statements)
//Vehicle[] vehicles = new Vehicle[]
//{
//    new Car { Model = "SF-24", MaxSpeed = 360, Team = "Ferrari" },
//    new SafetyCar { Model = "AMG GT Black Series", MaxSpeed = 300, SafetyFeature = "Track Control" }
//};

//foreach (Vehicle v in vehicles)
//{
//    v.ShowInfo(); // Calls the correct overridden method for each object
//}
//// Example (Formula 1 themed):
//// - Vehicle is the base class.
//// - Car and SafetyCar are derived classes.
//// - You can store both Car and SafetyCar objects in a Vehicle array and call overridden methods.

//public class Vehicle
//{
//    public string Model { get; set; }
//    public int MaxSpeed { get; set; }

//    // Virtual method for polymorphic behavior
//    public virtual void ShowInfo()
//    {
//        Console.WriteLine($"Model: {Model}, Max Speed: {MaxSpeed} km/h");
//    }
//}

//public class Car : Vehicle
//{
//    public string Team { get; set; }

//    public override void ShowInfo()
//    {
//        Console.WriteLine($"{Model} from {Team} has a max speed of {MaxSpeed} km/h.");
//    }
//}

//public class SafetyCar : Vehicle
//{
//    public string SafetyFeature { get; set; }

//    public override void ShowInfo()
//    {
//        Console.WriteLine($"{Model} (Safety Car) with {SafetyFeature}, max speed: {MaxSpeed} km/h.");
//    }
//}


// Polymorphism lets you treat different derived objects as their base type and still get their specific behavior.

#endregion

#region method overloading

// Method Overloading in C#
// Method overloading allows you to define multiple methods with the same name but different parameter lists.
// The compiler determines which method to call based on the arguments provided.
//

// Usage example (top-level statements)
//Car ferrariCar = new Car { Model = "SF-24", MaxSpeed = 360, Team = "Ferrari" };
//ferrariCar.Race(); // Calls the basic method
//ferrariCar.Race(58); // Calls the overloaded method with laps
//ferrariCar.Race(58, "Monza"); // Calls the overloaded method with laps and track
//// Example (Formula 1 themed):
//// - The Race method can be overloaded to accept different types of input.

//public class Car
//{
//    public string Model { get; set; }
//    public int MaxSpeed { get; set; }
//    public string Team { get; set; }

//    // Basic Race method
//    public void Race()
//    {
//        Console.WriteLine($"{Model} from {Team} is racing at {MaxSpeed} km/h!");
//    }

//    // Overloaded Race method with laps
//    public void Race(int laps)
//    {
//        Console.WriteLine($"{Model} from {Team} is racing for {laps} laps at {MaxSpeed} km/h!");
//    }

//    // Overloaded Race method with laps and track name
//    public void Race(int laps, string track)
//    {
//        Console.WriteLine($"{Model} from {Team} is racing for {laps} laps at {track} at {MaxSpeed} km/h!");
//    }
//}

// Method overloading makes your code flexible and easier to use for different scenarios.

#endregion

#region abstract class 

// Abstract Classes in C#
// An abstract class cannot be instantiated directly; it serves as a base for other classes.
// Abstract classes can define abstract methods (no implementation) that must be implemented by derived classes.
// Use the 'abstract' keyword to declare an abstract class or method.
//

//// Usage example (top-level statements)
//// Vehicle v = new Vehicle(); // Error: cannot create an instance of an abstract class
//Car ferrariCar = new Car { Model = "SF-24", MaxSpeed = 360, Team = "Ferrari" };
//ferrariCar.ShowInfo();

//SafetyCar mercedesSafetyCar = new SafetyCar { Model = "AMG GT Black Series", MaxSpeed = 300, SafetyFeature = "Track Control" };
//mercedesSafetyCar.ShowInfo();

//// Example (Formula 1 themed):
//// - Vehicle is an abstract class with an abstract method ShowInfo.
//// - Car and SafetyCar inherit from Vehicle and provide their own implementation of ShowInfo.

//public abstract class Vehicle // Abstract class: cannot be instantiated
//{
//    public string Model { get; set; }
//    public int MaxSpeed { get; set; }

//    // Abstract method: must be implemented by derived classes
//    public abstract void ShowInfo();
//}

//public class Car : Vehicle
//{
//    public string Team { get; set; }

//    // Implement the abstract method
//    public override void ShowInfo()
//    {
//        Console.WriteLine($"{Model} from {Team} has a max speed of {MaxSpeed} km/h.");
//    }
//}

//public class SafetyCar : Vehicle
//{
//    public string SafetyFeature { get; set; }

//    // Implement the abstract method
//    public override void ShowInfo()
//    {
//        Console.WriteLine($"{Model} (Safety Car) with {SafetyFeature}, max speed: {MaxSpeed} km/h.");
//    }
//}



// Abstract classes are useful for defining a common base with required behaviors for all derived classes.

#endregion

#region abstract methods

// Abstract Methods in C#
// An abstract method has no implementation in the base (abstract) class.
// It must be overridden and implemented in any non-abstract derived class.
// Use the 'abstract' keyword for both the method and the class.
//

//// Usage example (top-level statements)
//Car ferrariCar = new Car { Model = "SF-24", MaxSpeed = 360, Team = "Ferrari" };
//ferrariCar.ShowInfo();

//SafetyCar mercedesSafetyCar = new SafetyCar { Model = "AMG GT Black Series", MaxSpeed = 300, SafetyFeature = "Track Control" };
//mercedesSafetyCar.ShowInfo();
//// Example (Formula 1 themed):
//// - Vehicle is an abstract class with an abstract method ShowInfo.
//// - Car and SafetyCar override and implement ShowInfo.

//public abstract class Vehicle
//{
//    public string Model { get; set; }
//    public int MaxSpeed { get; set; }

//    // Abstract method: no implementation here
//    public abstract void ShowInfo();
//}

//public class Car : Vehicle
//{
//    public string Team { get; set; }

//    // Must override and implement the abstract method
//    public override void ShowInfo()
//    {
//        Console.WriteLine($"{Model} from {Team} has a max speed of {MaxSpeed} km/h.");
//    }
//}

//public class SafetyCar : Vehicle
//{
//    public string SafetyFeature { get; set; }

//    // Must override and implement the abstract method
//    public override void ShowInfo()
//    {
//        Console.WriteLine($"{Model} (Safety Car) with {SafetyFeature}, max speed: {MaxSpeed} km/h.");
//    }
//}



// Abstract methods enforce that all derived classes provide their own specific implementation.

#endregion

#region interfaces

// Interfaces in C#
// An interface defines a contract for classes: it contains only method signatures (no implementation or fields).
// Classes that implement an interface must provide their own implementation for all its methods.
// Use the 'interface' keyword to declare an interface.
//

//// Usage example (top-level statements)
//IRaceable raceCar = new Car { Model = "SF-24", Team = "Ferrari" };
//raceCar.Race();

//IRaceable safetyCar = new SafetyCar { Model = "AMG GT Black Series" };
//safetyCar.Race();
//// Example (Formula 1 themed):
//// - IRaceable interface defines a contract for racing behavior.
//// - Car and SafetyCar implement IRaceable and provide their own Race method.

//public interface IRaceable
//{
//    // Method signature only, no implementation
//    void Race();
//}

//public class Car : IRaceable
//{
//    public string Model { get; set; }
//    public string Team { get; set; }

//    // Implement the interface method
//    public void Race()
//    {
//        Console.WriteLine($"{Model} from {Team} is racing!");
//    }
//}

//public class SafetyCar : IRaceable
//{
//    public string Model { get; set; }

//    // Implement the interface method
//    public void Race()
//    {
//        Console.WriteLine($"{Model} (Safety Car) is controlling the race!");
//    }
//}


// Interfaces allow different classes to be treated uniformly if they share the same contract.

#endregion

#region multiple inheritance 

// Multiple Inheritance in C#
// C# does NOT allow a class to inherit from more than one base class (no multiple inheritance for classes).
// However, a class CAN implement multiple interfaces, allowing it to inherit behavior from several sources.
//

//// Usage example (top-level statements)
//Car ferrariCar = new Car { Model = "SF-24", Team = "Ferrari" };
//ferrariCar.Race();
//ferrariCar.ShowInfo();
//// Example (Formula 1 themed):
//// - Car implements both IRaceable and IDisplayable interfaces.

//public interface IRaceable
//{
//    void Race();
//}

//public interface IDisplayable
//{
//    void ShowInfo();
//}

//public class Car : IRaceable, IDisplayable // Implements multiple interfaces
//{
//    public string Model { get; set; }
//    public string Team { get; set; }

//    public void Race()
//    {
//        Console.WriteLine($"{Model} from {Team} is racing!");
//    }

//    public void ShowInfo()
//    {
//        Console.WriteLine($"{Model} from {Team} is ready for display.");
//    }
//}

// You can use multiple interfaces to add different capabilities to your classes in C#.

#endregion

#region property vs field

// Properties vs Fields in C#
// Fields are used for internal data storage within a class and are usually kept private.
// Properties provide controlled access to fields, allowing validation, logic, or encapsulation.
//

//// Usage example (top-level statements)
//Driver hamilton = new Driver();
//hamilton.Name = "Lewis Hamilton";
//hamilton.Salary = 50000000; // Valid assignment
//hamilton.Salary = -1000;    // Invalid, salary remains unchanged

//Console.WriteLine($"{hamilton.Name}'s salary: {hamilton.Salary}");

//// Example (Formula 1 themed):
//// - The Driver class uses a private field for salary and a property for controlled access.

//public class Driver
//{
//    // Field: internal data, not accessible outside the class
//    private double salary;

//    // Property: provides controlled access to the salary field
//    public double Salary
//    {
//        get { return salary; }
//        set
//        {
//            // Only allow positive salary values
//            if (value > 0)
//                salary = value;
//        }
//    }

//    // Auto-property: for simple public data
//    public string Name { get; set; }
//}



// Use fields for internal/private data, and properties for safe, controlled access from outside the class.

#endregion

#region static members 

// Static Members in C#
// Static members belong to the class itself, not to any specific instance.
// Use the 'static' keyword to declare static fields, properties, or methods.
// Static members are shared across all instances and accessed using the class name.
//

//// Usage example (top-level statements)
//Team ferrari = new Team("Ferrari");
//Team mercedes = new Team("Mercedes");

//Team.ShowTotalTeams(); // Output: Total Formula 1 teams: 2

//// Example (Formula 1 themed):
//// - The Team class has a static field to count total teams.

//public class Team
//{
//    // Static field: shared by all Team instances
//    public static int TotalTeams = 0;

//    // Instance property: unique to each Team object
//    public string Name { get; set; }

//    public Team(string name)
//    {
//        Name = name;
//        TotalTeams++; // Increment static field whenever a new team is created
//    }

//    // Static method: can be called without creating an instance
//    public static void ShowTotalTeams()
//    {
//        Console.WriteLine($"Total Formula 1 teams: {TotalTeams}");
//    }
//}


// Static members are useful for data or behavior that should be shared across all instances of a class.

#endregion

#region constructors 

// Demonstration of Constructors in C#
// Types: Default Constructor, Parameterized Constructor, Copy Constructor

//// Example usage of the constructors:

//// Using the Default Constructor
//Car defaultCar = new Car(); // Model: "Unknown", MaxSpeed: 0, Team: "Unknown"
//defaultCar.Race(); // Output: Unknown from Unknown is racing at 0 km/h!

//// Using the Parameterized Constructor
//Car ferrariCar = new Car("SF-24", 360, "Ferrari");
//ferrariCar.Race(); // Output: SF-24 from Ferrari is racing at 360 km/h!

//// Using the Copy Constructor
//Car copiedCar = new Car(ferrariCar); // Copies all properties from ferrariCar
//copiedCar.Race(); // Output: SF-24 from Ferrari is racing at 360 km/h!

//public class Car
//{
//    // Property: Model of the car (e.g., "SF-24")
//    public string Model { get; set; }

//    // Property: Maximum speed of the car in km/h
//    public int MaxSpeed { get; set; }

//    // Property: Team the car belongs to (e.g., "Ferrari")
//    public string Team { get; set; }

//    // 1. Default Constructor
//    // This constructor is called when no arguments are provided.
//    // It sets default values for all properties.
//    public Car()
//    {
//        Model = "Unknown";
//        MaxSpeed = 0;
//        Team = "Unknown";
//    }

//    // 2. Parameterized Constructor
//    // This constructor allows you to set all properties when creating a new Car object.
//    public Car(string model, int maxSpeed, string team)
//    {
//        Model = model;
//        MaxSpeed = maxSpeed;
//        Team = team;
//    }

//    // 3. Copy Constructor
//    // This constructor creates a new Car object by copying the properties from another Car object.
//    public Car(Car other)
//    {
//        Model = other.Model;
//        MaxSpeed = other.MaxSpeed;
//        Team = other.Team;
//    }

//    // Method: Simulate the car racing
//    public void Race()
//    {
//        Console.WriteLine($"{Model} from {Team} is racing at {MaxSpeed} km/h!");
//    }
//}



#endregion

#region destructor

// Destructor in C#
// A destructor is used to clean up resources before an object is destroyed by the garbage collector.
// In C#, destructors are rarely needed unless your class manages unmanaged resources (like file handles, database connections, etc.).
// For most classes, especially those only using managed resources (like strings, ints), you do not need a destructor.
// Syntax: ~ClassName() { /* cleanup code */ }

// Usage Example for Destructor
// Note: You cannot directly call or force the destructor. It is called by the garbage collector.
// For demonstration, you can create and remove objects in a scope and force garbage collection.

//void DemoDestructor()
//{
//    // Create a Car object in a local scope
//    Car tempCar = new Car("W15", 350, "Mercedes");
//    tempCar.Race(); // Output: W15 from Mercedes is racing at 350 km/h!

//    // tempCar goes out of scope after this function
//}

//// Call the demo function
//DemoDestructor();

//// Force garbage collection (for demonstration only; not recommended in production code)
//GC.Collect();
//GC.WaitForPendingFinalizers();

//// You may see the destructor message in the output, but timing is not guaranteed.
//// Output (eventually): Car object (W15, Mercedes) is being destroyed.

//public class Car
//{
//    public string Model { get; set; }
//    public int MaxSpeed { get; set; }
//    public string Team { get; set; }

//    // Default constructor
//    public Car()
//    {
//        Model = "Unknown";
//        MaxSpeed = 0;
//        Team = "Unknown";
//    }

//    // Parameterized constructor
//    public Car(string model, int maxSpeed, string team)
//    {
//        Model = model;
//        MaxSpeed = maxSpeed;
//        Team = team;
//    }

//    // Copy constructor
//    public Car(Car other)
//    {
//        Model = other.Model;
//        MaxSpeed = other.MaxSpeed;
//        Team = other.Team;
//    }

//    // Destructor
//    // This will be called by the garbage collector before the object is removed from memory.
//    // Use this only if you need to release unmanaged resources.
//    ~Car()
//    {
//        // Example: Clean up code here (if needed)
//        // For demonstration, we'll just print a message.
//        // In real scenarios, you would release unmanaged resources here.
//        Console.WriteLine($"Car object ({Model}, {Team}) is being destroyed.");
//    }

//    public void Race()
//    {
//        Console.WriteLine($"{Model} from {Team} is racing at {MaxSpeed} km/h!");
//    }
//}



#endregion

#region base and this 

// Demonstration of 'this' and 'base' keywords in C#
// 'this' refers to the current instance of the class.
// 'base' refers to members of the base class from a derived class.

//// Usage Example
//Car ferrariCar = new Car("SF-24", 360, "Ferrari");
//ferrariCar.ShowInfo();
//// Output:
//// [Vehicle] Model: SF-24, Max Speed: 360 km/h
//// [Car] Team: Ferrari

//// You can also use 'this' in methods to clarify property access
//ferrariCar.SetCarDetails("RB20", 355, "Red Bull");
//ferrariCar.ShowInfo();
//// Output:
//// [Vehicle] Model: RB20, Max Speed: 355 km/h
//// [Car] Team: Red Bull

//// Base class representing a general vehicle
//public class Vehicle
//{
//    public string Model { get; set; }
//    public int MaxSpeed { get; set; }

//    // Virtual method to show info
//    public virtual void ShowInfo()
//    {
//        Console.WriteLine($"[Vehicle] Model: {Model}, Max Speed: {MaxSpeed} km/h");
//    }
//}

//// Derived class representing a Formula 1 car
//public class Car : Vehicle
//{
//    public string Team { get; set; }

//    // Using 'this' keyword to refer to current instance properties
//    public void SetCarDetails(string model, int maxSpeed, string team)
//    {
//        // 'this' clarifies that we are setting the properties of the current object
//        this.Model = model;
//        this.MaxSpeed = maxSpeed;
//        this.Team = team;
//    }

//    // Using 'base' keyword to call base class method
//    public override void ShowInfo()
//    {
//        // Call the base class version of ShowInfo
//        base.ShowInfo();

//        // Add more details specific to Car
//        Console.WriteLine($"[Car] Team: {this.Team}");
//    }

//    // Example method using 'this' in a constructor
//    public Car(string model, int maxSpeed, string team)
//    {
//        // Use 'this' to assign constructor parameters to properties
//        this.Model = model;
//        this.MaxSpeed = maxSpeed;
//        this.Team = team;
//    }
//}



#endregion
