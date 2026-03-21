# Day 3 — Exercises: Abstract Factory & Builder (10 Problems)

Instructions
- Solve the 10 problems below. Each problem focuses on Abstract Factory, Builder, or their integration with DI, immutability, plugin loading, and testability.
- Implement each problem in a separate C# file. Name the solution file exactly as the problem title (prefix with the two-digit ID).
- In your solution file include a top comment summarizing the pattern intent, lifetime/D.I. recommendations, and one testability note.
- Aim for clear interfaces, minimal boilerplate, and small, focused examples.

Problems

1) 01-AbstractFactory_UI.cs  
Problem: Implement IButton and IWindow interfaces and an IUiFactory abstract factory. Provide two concrete factories (WindowsFactory, MacFactory) and an Application client that uses IUiFactory only. Add a short comment about DI registration and recommended lifetime.

2) 02-AbstractFactory_DataProvider.cs  
Problem: Implement an abstract provider factory that creates IConnection and ICommand. Provide two families: SqlProviderFactory and InMemoryProviderFactory. Show a DataAccess client that obtains a connection and command from the factory.

3) 03-ConfigurableFactoryRegistration.cs  
Problem: Show how to register a concrete Abstract Factory based on configuration with IServiceCollection (conceptual snippet). Implement a small FactoryProvider that reads a config string and returns the chosen factory; show how the consumer receives IProviderFactory via DI.

4) 04-Builder_Order.cs  
Problem: Implement an immutable Order class and an OrderBuilder with fluent methods (WithCustomer, AddLineItem, WithDiscount) and Build(). Ensure Build() returns an immutable snapshot and add a comment about invariants.

5) 05-Builder_DirectorTemplates.cs  
Problem: Implement an InvoiceBuilder and an InvoiceDirector that produces two templates (StandardInvoice and CreditMemo). Show usage where Director orchestrates the builder to produce preset documents.

6) 06-Builder_Validation.cs  
Problem: Create a ProductBuilder that validates required fields (Name, Price) before Build(). If invalid, Build() should throw a descriptive InvalidOperationException.

7) 07-AbstractFactory_PluginRegistry.cs  
Problem: Implement a PluginFactoryRegistry that can register multiple IProviderFactory instances by key and return a factory by name. Provide a PluginClient that requests a factory by key and uses it.

8) 08-FactoryDelegateAndDI.cs  
Problem: Implement a factory-delegate approach for INotifier using Func<string, INotifier>. Provide a NotifierConsumer that depends on the delegate. Include a commented IServiceCollection registration example showing how to wire the delegate.

9) 09-RefactorLegacyToFactoryMethod.cs  
Problem: Given a legacy class that instantiates concrete Notifiers directly, provide a refactor that introduces a NotifierCreator base class (Factory Method) and two ConcreteCreators. Show before-and-after comments explaining testability gains.

10) 10-UnitTestableBuilderAndFactory.cs  
Problem: Provide two test doubles: FakeProviderFactory and TestBuilder to show how code that uses factories and builders can be unit-tested. Include a short conceptual test snippet in comments that asserts expected behavior.

Deliverables
- This Day3_Exercises.md file (above).
- Ten solution files named exactly as the problem titles (01–10).  