namespace ComicBookShop.Core.Exceptions;

/// <summary>
/// Thrown when a requested entity cannot be found.
/// Demonstrates custom exceptions (Day 14).
/// </summary>
public class EntityNotFoundException : Exception
{
    public string EntityName { get; }
    public Guid EntityId { get; }

    public EntityNotFoundException(string entityName, Guid entityId)
        : base($"{entityName} with Id '{entityId}' was not found.")
    {
        EntityName = entityName;
        EntityId = entityId;
    }
}
