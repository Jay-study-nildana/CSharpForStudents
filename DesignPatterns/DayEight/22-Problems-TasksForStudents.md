# Day 8 — Behavioral Patterns: Strategy & Template Method — Problems

Instructions: For each problem implement a C# console program or library file. Each problem title is also the filename of the provided solution. Solutions should compile on .NET 6+ and demonstrate the requested behavior. Keep implementations clear and well commented. Demonstrate each feature in Main().

1. 01_SimpleStrategyPricing.cs  
   - Implement Strategy pattern for pricing/discounts. Define `IPriceStrategy` and at least three strategies (NoDiscount, PercentageDiscount, TieredDiscount). Provide `PricingService` that can swap strategies at runtime. Demonstrate different results.

2. 02_Strategy_A_B_Test_Harness.cs  
   - Build a simple A/B testing harness that routes simulated orders/users to two different pricing strategies (A and B) for a fixed number of trials, records conversion-like metrics (e.g., purchase boolean based on price threshold), and reports which strategy performed better.

3. 03_TemplateMethod_OrderProcessing.cs  
   - Implement Template Method `OrderProcessor` with `ProcessOrder()` (sealed) and overridable steps (`Validate`, `CalculateSubtotal`, `ApplyDiscounts`, `ApplyTaxes`, `Save`, `Notify`). Provide at least two concrete processors (Domestic, International) and demonstrate.

4. 04_Template_With_StrategyStep.cs  
   - Combine Template Method and Strategy: make `OrderProcessor` accept an `IPriceStrategy` used in its discount step so you can change discount logic at runtime without altering the pipeline.

5. 05_Strategy_DI_and_Config.cs  
   - Simulate configuration-driven strategy selection. Implement a small factory that maps config keys (string) to strategies, and demonstrate swapping strategy by a config value. Show how to plug the factory into `PricingService`.

6. 06_Strategy_Shipping.cs  
   - Implement shipping cost strategies (`FlatRateShipping`, `WeightBasedShipping`, `FreeOverThreshold`) that implement `IShippingStrategy`. Show `CheckoutService` using a shipping strategy and swapping strategies at runtime.

7. 07_UnitTests_For_Strategy_Template.cs  
   - Provide lightweight console assertions to verify:
     - Strategy outputs for sample inputs.
     - Template Method calls expected steps (use logs or counters).
     - A/B harness yields reproducible metrics for deterministic RNG seed.

8. 08_Serializable_Strategy_Config.cs  
   - Demonstrate serializing a simple strategy configuration (strategy key and parameters) to JSON and restoring the corresponding strategy via factory to reproduce pricing behavior.

9. 09_PlantUML_Generator_Strategy_Template.cs  
   - Print PlantUML text for the Strategy and Template Method participants used in this exercise (interfaces/classes and relationships). The output should be valid PlantUML.

10. 10_Extend_With_New_Strategy.cs  
    - Add a new strategy (e.g., LoyaltyTierDiscount) and show the minimal changes required to use it in `PricingService` and the A/B harness. Explain in comments why changes are minimal.

Deliverable: Each problem is implemented in the matching .cs file below. Each Main() demonstrates the required behavior.