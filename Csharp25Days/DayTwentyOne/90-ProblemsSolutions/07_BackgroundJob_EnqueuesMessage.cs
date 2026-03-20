using System;
using Moq;
using Xunit;

// Queue client abstraction
public interface IQueueClient { void Enqueue(string queueName, string payload); }

// Background job that enqueues a message when processing is complete
public class BackgroundJob
{
    private readonly IQueueClient _queue;
    public BackgroundJob(IQueueClient queue) { _queue = queue; }

    public void OnOrderProcessed(int orderId)
    {
        var payload = $"{{\"orderId\":{orderId}}}";
        _queue.Enqueue("orders", payload);
    }
}

// Tests
public class BackgroundJobTests
{
    [Fact]
    public void OnOrderProcessed_EnqueuesCorrectPayload()
    {
        // Arrange
        var mockQueue = new Mock<IQueueClient>();
        string capturedQueue = null;
        string capturedPayload = null;
        mockQueue.Setup(q => q.Enqueue(It.IsAny<string>(), It.IsAny<string>()))
            .Callback<string, string>((qName, p) => { capturedQueue = qName; capturedPayload = p; });

        var job = new BackgroundJob(mockQueue.Object);

        // Act
        job.OnOrderProcessed(42);

        // Assert
        Assert.Equal("orders", capturedQueue);
        Assert.Equal("{\"orderId\":42}", capturedPayload);
        mockQueue.Verify(q => q.Enqueue("orders", "{\"orderId\":42}"), Times.Once);
    }
}