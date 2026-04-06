try
{
    DoWork();
}
catch (Exception)
{
    // maybe log details, then rethrow preserving stack
    Log("error happened");
    throw; // preserves original stack trace
}

// Example work method
void DoWork()
{
    Console.WriteLine("Doing work...");
    // Simulate an error
    throw new InvalidOperationException("Something went wrong!");
}

// Example log method
void Log(string message)
{
    Console.WriteLine($"LOG: {message}");
}