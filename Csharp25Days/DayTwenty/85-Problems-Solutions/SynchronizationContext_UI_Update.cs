// SynchronizationContext_UI_Update.cs
// Simulates a UI update that must occur on the UI context. Shows how ConfigureAwait(false)
// prevents resuming on the UI context and what that means for UI updates.

using System;
using System.Threading;
using System.Threading.Tasks;

class UIShim
{
    // Very small fake "UI" object
    class UI
    {
        public void Update(string text) => Console.WriteLine($"UI updated: {text}");
    }

    static async Task WorkerAsync(UI ui)
    {
        // Simulate an I/O-bound operation
        string data = await Task.Run(async () => { await Task.Delay(100); return "data"; });
        // Because await captured the SynchronizationContext, this will run on UI context in a real UI app.
        ui.Update(data);
    }

    static async Task Worker_NoCaptureAsync(UI ui)
    {
        // This avoids resuming on the UI context
        string data = await Task.Run(async () => { await Task.Delay(100); return "data"; }).ConfigureAwait(false);
        // We are now *not* on the UI context; updating UI here would be invalid in real apps:
        try
        {
            ui.Update(data); // may throw in real UI frameworks
            Console.WriteLine("UI update from non-UI context succeeded (console).");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to update UI: {ex.Message}");
        }
    }

    public static async Task Main()
    {
        var ui = new UI();
        Console.WriteLine("WorkerAsync (captures context) — safe to update UI after await:");
        await WorkerAsync(ui);

        Console.WriteLine("Worker_NoCaptureAsync (ConfigureAwait(false)) — not on UI context after await:");
        await Worker_NoCaptureAsync(ui);
    }
}