using System;

class Adapter_For_LegacyApi
{
    // Problem: adapt LegacyNotifier to INotifier
    public interface INotifier { void Notify(string msg); }

    // Legacy code we cannot change
    public class LegacyNotifier
    {
        public void SendMsg(string text) => Console.WriteLine($"Legacy send: {text}");
    }

    // Adapter implements INotifier and delegates to LegacyNotifier
    public class LegacyNotifierAdapter : INotifier
    {
        private readonly LegacyNotifier _legacy;
        public LegacyNotifierAdapter(LegacyNotifier legacy) => _legacy = legacy;
        public void Notify(string msg) => _legacy.SendMsg(msg);
    }

    static void Main()
    {
        INotifier notifier = new LegacyNotifierAdapter(new LegacyNotifier());
        notifier.Notify("Hello via adapter");

        // Adapter decouples client code from legacy API and fits it into modern interface usage.
    }
}