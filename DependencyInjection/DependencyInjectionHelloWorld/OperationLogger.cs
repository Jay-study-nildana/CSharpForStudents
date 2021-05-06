using System;

namespace DependencyInjectionHelloWorld
{
    //The OperationLogger defines a constructor that requires each of the aforementioned marker interfaces, 
    //that is; ITransientOperation, IScopedOperation, and ISingletonOperation. 
    //The object exposes a single method that allows the consumer to 
    //log the operations with a given scope parameter. 
    //When invoked, the LogOperations method logs each operation's unique identifier 
    //with the scope string and message.

    public class OperationLogger
    {
        private readonly ITransientOperation _transientOperation;
        private readonly IScopedOperation _scopedOperation;
        private readonly ISingletonOperation _singletonOperation;

        public OperationLogger(
            ITransientOperation transientOperation,
            IScopedOperation scopedOperation,
            ISingletonOperation singletonOperation) =>
            (_transientOperation, _scopedOperation, _singletonOperation) =
                (transientOperation, scopedOperation, singletonOperation);

        public void LogOperations(string scope)
        {
            LogOperation(_transientOperation, scope, "Always different");
            LogOperation(_scopedOperation, scope, "Changes only with scope");
            LogOperation(_singletonOperation, scope, "Always the same");
        }


        private static void LogOperation<T>(T operation, string scope, string message)
            where T : IOperation =>
            Console.WriteLine(
                $"{scope}: {typeof(T).Name,-19} [ {operation.OperationId}...{message,-23} ]");
    }
}