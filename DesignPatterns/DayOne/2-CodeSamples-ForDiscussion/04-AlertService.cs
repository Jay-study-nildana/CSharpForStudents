// 04-AlertService.cs
// Uses constructor injection to receive its IMessageSender dependency (good DI example).

public class AlertService
{
    private readonly IMessageSender _sender;

    // Dependency provided via constructor (constructor injection)
    public AlertService(IMessageSender sender)
    {
        _sender = sender;
    }

    public void Alert(string text)
    {
        _sender.Send(text);
    }
}