namespace SolidDc.Services
{
    // Abstraction used to print objects — allows SRP and DIP demos.
    public interface IPrinter<T>
    {
        void Print(T item);
    }
}
