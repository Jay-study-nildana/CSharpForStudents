// UnitTestWithFakeRepository.cs
using System;
using Xunit;

namespace DI.Exercises.Tests
{
    // Service under test
    public interface ISampleRepository { string GetById(int id); }
    public class SampleService
    {
        private readonly ISampleRepository _repo;
        public SampleService(ISampleRepository repo) => _repo = repo;
        public string GetGreeting(int id) => $"Hello {_repo.GetById(id)}";
    }

    // Hand-rolled fake repository for tests
    public class FakeSampleRepository : ISampleRepository
    {
        public string GetById(int id) => id == 1 ? "Alice" : "Unknown";
    }

    public class SampleServiceTests
    {
        [Fact]
        public void GetGreeting_ReturnsExpectedGreeting()
        {
            // Arrange
            var fakeRepo = new FakeSampleRepository();
            var svc = new SampleService(fakeRepo);

            // Act
            var result = svc.GetGreeting(1);

            // Assert
            Assert.Equal("Hello Alice", result);
        }
    }

    // Note: Using a hand-rolled fake removes DB dependencies and gives a fast, deterministic unit test.
}