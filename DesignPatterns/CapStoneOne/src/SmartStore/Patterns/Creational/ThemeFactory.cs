namespace SmartStore.Patterns.Creational;

// ================================================================
// ABSTRACT FACTORY PATTERN
// ================================================================
// Creates families of related UI rendering objects (a "theme").
// Switching the factory swaps the entire visual family atomically.
//
// Intent   : Provide an interface for creating families of related
//            or dependent objects without specifying concrete classes.
// Problem  : The app needs switchable Light/Dark console themes where
//            all components (header, table rows, status) must match.
// Solution : IThemeFactory declares creation methods for each element.
//            LightThemeFactory and DarkThemeFactory produce coherent families.
// ================================================================
public interface IThemeFactory
{
    IHeaderRenderer  CreateHeaderRenderer();
    ITableRenderer   CreateTableRenderer();
    IStatusRenderer  CreateStatusRenderer();
}

// ---- Product interfaces (what the factory produces) ----
public interface IHeaderRenderer { void Render(string title); }
public interface ITableRenderer  { void RenderRow(string label, string value); }
public interface IStatusRenderer { void RenderStatus(string status, bool success); }

// ================================================================
// LIGHT THEME  — concrete factory
// ================================================================
public class LightThemeFactory : IThemeFactory
{
    public IHeaderRenderer  CreateHeaderRenderer()  => new LightHeaderRenderer();
    public ITableRenderer   CreateTableRenderer()   => new LightTableRenderer();
    public IStatusRenderer  CreateStatusRenderer()  => new LightStatusRenderer();
}

public class LightHeaderRenderer : IHeaderRenderer
{
    public void Render(string title)
    {
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine($"\n{new string('═', 60)}");
        Console.WriteLine($"  {title}");
        Console.WriteLine(new string('═', 60));
        Console.ResetColor();
    }
}

public class LightTableRenderer : ITableRenderer
{
    public void RenderRow(string label, string value)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write($"  {label,-28}");
        Console.ResetColor();
        Console.WriteLine(value);
    }
}

public class LightStatusRenderer : IStatusRenderer
{
    public void RenderStatus(string status, bool success)
    {
        Console.ForegroundColor = success ? ConsoleColor.DarkGreen : ConsoleColor.DarkRed;
        Console.WriteLine($"  [{(success ? " OK " : "FAIL")}]  {status}");
        Console.ResetColor();
    }
}

// ================================================================
// DARK THEME  — concrete factory
// ================================================================
public class DarkThemeFactory : IThemeFactory
{
    public IHeaderRenderer  CreateHeaderRenderer()  => new DarkHeaderRenderer();
    public ITableRenderer   CreateTableRenderer()   => new DarkTableRenderer();
    public IStatusRenderer  CreateStatusRenderer()  => new DarkStatusRenderer();
}

public class DarkHeaderRenderer : IHeaderRenderer
{
    public void Render(string title)
    {
        Console.ResetColor();
        Console.BackgroundColor = ConsoleColor.DarkBlue;
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"\n  ▌ {title.ToUpperInvariant()} ▐  ");
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine(new string('─', 60));
        Console.ResetColor();
    }
}

public class DarkTableRenderer : ITableRenderer
{
    public void RenderRow(string label, string value)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write($"  ► {label,-25}: ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(value);
        Console.ResetColor();
    }
}

public class DarkStatusRenderer : IStatusRenderer
{
    public void RenderStatus(string status, bool success)
    {
        Console.ForegroundColor = success ? ConsoleColor.Green : ConsoleColor.Red;
        Console.WriteLine($"  {(success ? "✔" : "✘")}  {status}");
        Console.ResetColor();
    }
}
