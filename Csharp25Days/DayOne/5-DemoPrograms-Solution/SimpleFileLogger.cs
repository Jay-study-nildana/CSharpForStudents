using System;
using System.IO;

class SimpleFileLogger
{
    static void Main()
    {
        // Initial settings
        string logPath = Path.Combine(Environment.CurrentDirectory, "log.txt");
        bool dailyRotation = false;

        while (true)
        {
            ShowHeader(logPath, dailyRotation);
            ShowMenu();
            Console.Write("Choose an option: ");
            var choice = Console.ReadLine()?.Trim();

            switch (choice)
            {
                case "1":
                    AppendMessage(logPath, dailyRotation);
                    break;
                case "2":
                    ListMessages(logPath, dailyRotation);
                    break;
                case "3":
                    logPath = ChangeLogPath(logPath);
                    break;
                case "4":
                    dailyRotation = !dailyRotation;
                    Console.WriteLine($"Daily rotation {(dailyRotation ? "enabled" : "disabled")}.");
                    break;
                case "0":
                    Console.WriteLine("Exiting. Goodbye!");
                    return;
                default:
                    Console.WriteLine("Unknown option. Enter a number from the menu.");
                    break;
            }

            Console.WriteLine(); // spacing between iterations
        }
    }

    static void ShowHeader(string logPath, bool dailyRotation)
    {
        Console.WriteLine("=== Simple File Logger ===");
        Console.WriteLine($"Current log file: {logPath}");
        Console.WriteLine($"Daily rotation: {(dailyRotation ? "On (per-day files)" : "Off (single file)")}");
    }

    static void ShowMenu()
    {
        Console.WriteLine("1) Append message to log");
        Console.WriteLine("2) List saved messages");
        Console.WriteLine("3) Change log file path (absolute or relative)");
        Console.WriteLine("4) Toggle daily rotation on/off");
        Console.WriteLine("0) Exit");
    }

    static string ResolveLogPath(string baseLogPath, bool dailyRotation)
    {
        if (!dailyRotation) return baseLogPath;

        var dir = Path.GetDirectoryName(baseLogPath);
        if (string.IsNullOrEmpty(dir)) dir = Environment.CurrentDirectory;

        var baseName = Path.GetFileNameWithoutExtension(baseLogPath);
        var ext = Path.GetExtension(baseLogPath);
        if (string.IsNullOrEmpty(ext)) ext = ".txt";

        var fileName = $"{baseName}-{DateTime.Now:yyyy-MM-dd}{ext}";
        return Path.Combine(dir, fileName);
    }

    static void AppendMessage(string baseLogPath, bool dailyRotation)
    {
        Console.Write("Enter message (empty to cancel): ");
        var message = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(message))
        {
            Console.WriteLine("No message entered. Cancelled.");
            return;
        }

        var logFile = ResolveLogPath(baseLogPath, dailyRotation);
        var entry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {message}{Environment.NewLine}";

        try
        {
            // Ensure directory exists
            var dir = Path.GetDirectoryName(logFile);
            if (!string.IsNullOrEmpty(dir))
                Directory.CreateDirectory(dir);

            File.AppendAllText(logFile, entry);
            Console.WriteLine($"Appended to {logFile}");
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("Error: Access denied. Check file permissions.");
        }
        catch (DirectoryNotFoundException)
        {
            Console.WriteLine("Error: Directory not found. Check the configured path.");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"I/O error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    static void ListMessages(string baseLogPath, bool dailyRotation)
    {
        var logFile = ResolveLogPath(baseLogPath, dailyRotation);

        try
        {
            if (!File.Exists(logFile))
            {
                Console.WriteLine($"No log file found at {logFile}");
                return;
            }

            var lines = File.ReadAllLines(logFile);
            if (lines.Length == 0)
            {
                Console.WriteLine("(Log file is empty)");
                return;
            }

            Console.WriteLine($"--- Contents of {logFile} ---");
            for (int i = 0; i < lines.Length; i++)
            {
                Console.WriteLine($"{i + 1:000}: {lines[i]}");
            }
            Console.WriteLine($"--- End ({lines.Length} entries) ---");
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("Error: Access denied. Check file permissions.");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"I/O error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    static string ChangeLogPath(string currentPath)
    {
        Console.Write("Enter new log file path (absolute or relative): ");
        var input = Console.ReadLine()?.Trim();
        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("Path not changed.");
            return currentPath;
        }

        try
        {
            // Try to get full path (will throw for invalid paths)
            var full = Path.GetFullPath(input);
            Console.WriteLine($"Log path changed to: {full}");
            return full;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Invalid path: {ex.Message}. Path not changed.");
            return currentPath;
        }
    }
}
