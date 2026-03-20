using System;
using Moq;
using Xunit;

// Abstractions to avoid DateTime.Now and Environment
public interface IClock { DateTime UtcNow { get; } }
public interface IEnvironment { string Get(string key); }

public class FeatureToggleService
{
    private readonly IClock _clock;
    private readonly IEnvironment _env;

    public FeatureToggleService(IClock clock, IEnvironment env)
    {
        _clock = clock;
        _env = env;
    }

    // Example: feature enabled only after specific date or if override env var present
    public bool IsFeatureActive()
    {
        var overrideVal = _env.Get("FEATURE_X");
        if (!string.IsNullOrEmpty(overrideVal)) return overrideVal == "1";
        return _clock.UtcNow.Date >= new DateTime(2026, 1, 1);
    }
}

// Tests
public class FeatureToggleServiceTests
{
    [Fact]
    public void IsFeatureActive_EnvOverrideTurnsOnFeature()
    {
        // Arrange
        var mockClock = new Mock<IClock>();
        mockClock.Setup(c => c.UtcNow).Returns(new DateTime(2025, 12, 31));
        var mockEnv = new Mock<IEnvironment>();
        mockEnv.Setup(e => e.Get("FEATURE_X")).Returns("1");

        var svc = new FeatureToggleService(mockClock.Object, mockEnv.Object);

        // Act
        var result = svc.IsFeatureActive();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsFeatureActive_DateAfterThreshold_ActivatesFeature()
    {
        // Arrange
        var mockClock = new Mock<IClock>();
        mockClock.Setup(c => c.UtcNow).Returns(new DateTime(2026, 2, 1));
        var mockEnv = new Mock<IEnvironment>();
        mockEnv.Setup(e => e.Get("FEATURE_X")).Returns((string)null);

        var svc = new FeatureToggleService(mockClock.Object, mockEnv.Object);

        // Act
        var result = svc.IsFeatureActive();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsFeatureActive_BeforeThresholdAndNoOverride_ReturnsFalse()
    {
        // Arrange
        var mockClock = new Mock<IClock>();
        mockClock.Setup(c => c.UtcNow).Returns(new DateTime(2025, 6, 1));
        var mockEnv = new Mock<IEnvironment>();
        mockEnv.Setup(e => e.Get("FEATURE_X")).Returns((string)null);

        var svc = new FeatureToggleService(mockClock.Object, mockEnv.Object);

        // Act
        var result = svc.IsFeatureActive();

        // Assert
        Assert.False(result);
    }
}