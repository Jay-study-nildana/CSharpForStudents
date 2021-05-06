using static System.Guid;

//The DefaultOperation implements all of the named marker interfaces 
// and initializes the OperationId property to the last four characters of a new globally unique identifier (GUID).
namespace DependencyInjectionHelloWorld
{
    public class DefaultOperation :
        ITransientOperation,
        IScopedOperation,
        ISingletonOperation
    {
        public string OperationId { get; } = NewGuid().ToString()[^4..];
    }
}