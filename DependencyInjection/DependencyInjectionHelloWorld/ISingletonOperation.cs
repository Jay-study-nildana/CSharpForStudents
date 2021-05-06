namespace DependencyInjectionHelloWorld
{
    //All of the subinterfaces of IOperation name their intended service lifetime. 
    //For example, "Transient" or "Singleton".
    public interface ISingletonOperation : IOperation
    {
    }
}