# Day 10 — Structs, Records & Immutability: 10 Practice Problems

Context: Decide between structs vs classes, design immutable records/record-structs, use tuples for small multi-value returns, and apply defensive copying and equality rules for immutable DTOs.

Instructions: For each problem below implement a short C# console program or type that demonstrates the requested concept. For each solution include:
- The implemented type(s) (struct/class/record/record struct/tuple).
- A short Main that demonstrates behavior (copy semantics, equality, defensive copy, boxing, with-expressions, etc.).
- A brief comment explaining why you chose struct vs class and any immutability or performance notes.

Problems:

1. Choose_Struct_vs_Class  
   - Given two domain types (2D Point used heavily in math vs Customer entity with identity and lifecycle), implement one as a struct and one as a class. Show copy vs reference semantics and explain rationale.

2. Immutable_Record_Money  
   - Implement an immutable `Money` as a `record` (or `record struct`) with Currency and Amount. Demonstrate equality, `with` expression, and why immutability is useful for money values.

3. Tuple_Returns_Summary  
   - Write a method that processes an int array and returns a named tuple `(int Sum, double Average, int Count)` and demonstrate deconstruction and named access.

4. Record_Struct_Point  
   - Implement a `readonly record struct Point(int X, int Y)` and show value semantics, deconstruction, and usage in a small array of points.

5. Defensive_Copying_DTO  
   - Create a `UserDto` that accepts a `List<string>` of roles in its constructor and returns roles via a read-only collection or immutable collection to avoid external mutation. Demonstrate that external changes to the original list do not affect the DTO.

6. Equality_Record_vs_Class  
   - Implement a `PersonClass` (class) and `PersonRecord` (record) with same properties, show how `Equals` and `==` behave differently by default, and comment on which to use for value semantics.

7. Boxing_Unboxing_Example  
   - Show a small example where using a struct in a boxed context (e.g., `object` or non-generic `ArrayList` / `IList`) causes boxing. Demonstrate the allocation behavior conceptually and suggest how to avoid it.

8. Readonly_Struct_Performance  
   - Implement a `readonly struct Vector3` with small fields and show that methods cannot mutate state. Explain benefits for thread-safety and performance for small value types.

9. Convert_Mutable_To_Immutable  
   - Start with a mutable `OrderDto` (public setters) and provide an immutable replacement (`record` or class with get-only properties). Show example code that constructs and uses the immutable variant and explain migration tips.

10. DTOs_Should_Be_Immutable  
    - Provide a small sample `ApiResponse` DTO (status code, message, payload) implemented as an immutable record and write a short justification comment listing which DTOs in a typical app should be immutable and why.

Use these practice problems to solidify decisions about value vs reference, immutable vs mutable, and the role of tuples and records in your designs.