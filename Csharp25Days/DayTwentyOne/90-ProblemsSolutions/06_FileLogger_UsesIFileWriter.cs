using System;
using Moq;
using Xunit;

// Abstraction for file writes
public interface IFileWriter { void WriteAllText(string path, string contents); }

// Logger that was refactored to use IFileWriter
public class FileLogger
{
    private readonly IFileWriter _writer;
    private readonly string _path;
    public FileLogger(IFileWriter writer, string path) { _writer = writer; _path = path; }

    public void Log(string message)
    {
        var line = $"{DateTime.UtcNow:o} {message}";
        _writer.WriteAllText(_path, line);
    }
}

// Tests
public class FileLoggerTests
{
    [Fact]
    public void Log_WritesFormattedLineToFileWriter()
    {
        // Arrange
        var mockWriter = new Mock<IFileWriter>();
        string capturedPath = null;
        string capturedContents = null;
        mockWriter.Setup(w => w.WriteAllText(It.IsAny<string>(), It.IsAny<string>()))
            .Callback<string, string>((p, c) => { capturedPath = p; capturedContents = c; });

        var logger = new FileLogger(mockWriter.Object, "/tmp/log.txt");

        // Act
        logger.Log("hello");

        // Assert
        Assert.Equal("/tmp/log.txt", capturedPath);
        Assert.Contains("hello", capturedContents);
        mockWriter.Verify(w => w.WriteAllText("/tmp/log.txt", It.IsAny<string>()), Times.Once);
    }
}