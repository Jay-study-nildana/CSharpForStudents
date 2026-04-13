# Self-Reference in C# Classes

## What is Self-Reference?
Self-reference is when a class refers to itself within its own definition. This is common in patterns like Singleton, recursive methods, or self-referential data structures.

## Example: Singleton Pattern
Consider the following code:

```csharp
public class SingletonLogger
{
    private static SingletonLogger _instance; // self-reference
    private SingletonLogger() { }

    public static SingletonLogger Instance => _instance ??= new SingletonLogger();

    public void Log(string message)
    {
        Console.WriteLine($"[SingletonLogger] {message}");
    }
}
```

### Explanation
- `private static SingletonLogger _instance;` declares a static field of the same type as the class being defined.
- `public static SingletonLogger Instance` uses the class type to return the singleton instance.
- The compiler allows this because it knows the class type during parsing.

## Why is This Allowed?
The C# compiler recognizes the class type as it parses the class body, so you can use the class name inside its own definition. This enables patterns like Singleton and recursive structures.

## When is Self-Reference Useful?
- Singleton pattern
- Recursive methods (e.g., trees, linked lists)
- Factory methods returning the same type

---
**Summary:** Self-reference is a powerful feature in C# that enables advanced design patterns and data structures.
