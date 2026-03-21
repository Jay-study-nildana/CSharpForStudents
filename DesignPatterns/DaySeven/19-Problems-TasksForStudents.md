# Day 7 — Structural Patterns: Composite & Bridge — Problems

Instructions: For each problem below implement a C# console program or library file. Each problem title is also the filename of the provided solution (see files below). Solutions should compile on .NET 6+ and demonstrate the requested behavior. Keep implementations clear and well commented.

1. 01_SimpleMenuComposite.cs  
   - Implement a basic Composite pattern for a menu/tree UI: `MenuComponent` (abstract), `MenuItem` (leaf), and `MenuGroup` (composite). Provide `Add`, `Remove`, and `Render(int depth)` methods. Demonstrate building a menu tree and rendering it to the console.

2. 02_MenuCompositeTraversal.cs  
   - Extend the menu composite to provide traversal helpers: `DepthFirstTraversal(Action<MenuComponent>)` and `BreadthFirstTraversal(Action<MenuComponent>)`. Demonstrate both traversals and print the visited nodes' titles in order.

3. 03_Composite_With_Iterator.cs  
   - Modify `MenuGroup` to implement `IEnumerable<MenuComponent>` so that clients can use `foreach` to iterate over all child components (recursive depth-first). Demonstrate `foreach` usage and a LINQ query (e.g., count leaves).

4. 04_MenuCompositeSerialization.cs  
   - Add JSON serialization/deserialization for the composite tree. Ensure both `MenuGroup` and `MenuItem` are round-trip serializable (include a small type discriminator in JSON). Provide Save/Load examples.

5. 05_Bridge_Renderer_Abstract.cs  
   - Implement the Bridge pattern: `IRenderer` interface and two `ConcreteImplementor`s (e.g., `ConsoleRenderer` and `HtmlRenderer`). Implement an abstraction `Widget` and a `Button` that uses an `IRenderer` to draw itself. Demonstrate swapping renderers.

6. 06_Composite_With_Bridge_Rendering.cs  
   - Combine Composite and Bridge: let `MenuComponent.Render(IRenderer renderer, int depth)` draw using the renderer primitives (`DrawText`, `DrawBox`, etc.). Build the menu tree and show rendering using two different renderers.

7. 07_Extending_With_New_Leaf.cs  
   - Add a new leaf type `MenuToggle` (a menu entry with on/off) to the composite. Show how adding this new leaf affects (or doesn't affect) existing code and provide a short note in comments about required changes.

8. 08_Adding_New_Renderer.cs  
   - Implement a new `SvgRenderer` (or `SvgLikeRenderer`) that implements `IRenderer`. Show rendering the same `Button` and the same `MenuComponent` tree with the new renderer, demonstrating the Bridge advantage.

9. 09_UML_PlantUML_Generator.cs  
   - Write a small utility that prints PlantUML text for both patterns (Composite and Bridge) describing classes and relationships used in your solutions. The output should be valid PlantUML text that a student can paste into an online PlantUML renderer.

10. 10_UnitTests_For_Composite_Bridge.cs  
    - Provide a small set of console-drive unit-style checks (simple `Assert` helper that throws on failure) to verify:
      - Composite `Add`/`Remove` and `Render` visited counts
      - Traversal order for at least one traversal
      - Bridge: `Button` draws both with `ConsoleRenderer` and `HtmlRenderer`
      - Serialization round-trip from problem 4

Deliverable: Each problem should be implemented in the matching .cs file provided below. Include comments explaining key design choices and a short demonstration in `Main()` or an equivalent entry point.