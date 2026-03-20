using System;
using Moq;
using Xunit;

// Interfaces
public interface IPasswordHasher { string Hash(string plain); }
public interface IUserRepository { void Save(User user); }
public class User { public string Username { get; set; } public string PasswordHash { get; set; } }

// Service to test
public class AuthenticationService
{
    private readonly IPasswordHasher _hasher;
    private readonly IUserRepository _repo;
    public AuthenticationService(IPasswordHasher hasher, IUserRepository repo)
    {
        _hasher = hasher;
        _repo = repo;
    }

    public void Register(string username, string password)
    {
        var hash = _hasher.Hash(password);
        var user = new User { Username = username, PasswordHash = hash };
        _repo.Save(user);
    }
}

// Test
public class AuthenticationServiceTests
{
    [Fact]
    public void Register_UsesHasherAndSavesHashedPassword()
    {
        // Arrange
        var mockHasher = new Mock<IPasswordHasher>();
        var mockRepo = new Mock<IUserRepository>();

        mockHasher.Setup(h => h.Hash("secret")).Returns("HASHED-secret");

        User savedUser = null;
        mockRepo.Setup(r => r.Save(It.IsAny<User>()))
            .Callback<User>(u => savedUser = u);

        var svc = new AuthenticationService(mockHasher.Object, mockRepo.Object);

        // Act
        svc.Register("alice", "secret");

        // Assert
        mockHasher.Verify(h => h.Hash("secret"), Times.Once);
        mockRepo.Verify(r => r.Save(It.IsAny<User>()), Times.Once);
        Assert.NotNull(savedUser);
        Assert.Equal("alice", savedUser.Username);
        Assert.Equal("HASHED-secret", savedUser.PasswordHash);
    }
}