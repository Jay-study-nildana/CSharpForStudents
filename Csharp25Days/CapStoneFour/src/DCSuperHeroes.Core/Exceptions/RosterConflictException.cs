namespace DCSuperHeroes.Core.Exceptions;

public sealed class RosterConflictException : Exception
{
    public RosterConflictException(string message)
        : base(message)
    {
    }
}