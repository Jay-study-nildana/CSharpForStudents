// 10-UnitTestableBuilderAndFactory.cs
// Demonstration of test doubles: FakeProviderFactory and TestBuilder for unit tests.
// Test snippet included in comments showing how to assert behavior.

using System;
using System.Collections.Generic;

// Fake Provider Factory for tests
public class FakeConnection : IConnection
{
    public void Open() => Console.WriteLine("FakeConnection.Open");
    public void Dispose() => Console.WriteLine("FakeConnection.Dispose");
}
public class FakeCommand : ICommand
{
    public void Execute(string sql) => Console.WriteLine($"FakeCommand.Execute: {sql}");
}
public class FakeProviderFactory : IProviderFactory
{
    public IConnection CreateConnection() => new FakeConnection();
    public ICommand CreateCommand(IConnection conn) => new FakeCommand();
}

// Test builder for creating user objects easily
public class TestUserBuilder
{
    private string _username = "test-user";
    private string _email = "test@example.com";

    public TestUserBuilder WithUsername(string username) { _username = username; return this; }
    public TestUserBuilder WithEmail(string email) { _email = email; return this; }

    public User Build() => new UserBuilder().WithUsername(_username).WithEmail(_email).Build();
}

/*
Conceptual test snippet (pseudo-code, not a test framework):

// Arrange
var fakeFactory = new FakeProviderFactory();
var client = new DataAccess(fakeFactory); // DataAccess from problem 02

// Act
client.Run("SELECT 1");

// Assert
// For a real unit test, use assertions on a spy or captured calls. Here we rely on fake implementations to run deterministically.

For builder tests:
var user = new TestUserBuilder().WithUsername("alice").Build();
Assert.Equal("alice", user.Username);
*/