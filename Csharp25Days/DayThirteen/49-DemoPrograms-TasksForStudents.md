# Day 13 — LINQ Practice Problems

Instructions
- Solve each problem using LINQ (method or query syntax) over in-memory collections.
- For each problem provide a short comment describing the pipeline stages in plain English and the expected complexity (high-level).
- Each solution should include a small example dataset and a Main() demonstrating the result.
- Aim for idiomatic, readable C# (.NET 6+).

Problems

1) ProjectProductNames  
Given a list of `Product{Id,Name,Price}`, return the list of product names sorted alphabetically. (Projection + ordering)

2) FilterRecentOrders  
Given a list of `Order{Id,CustomerId,Date}`, return orders placed in the last 30 days. (Filtering + date logic)

3) TopNByPrice  
Given products, return the top N most expensive products (name and price). If tie, order by name. (Ordering + Take)

4) TotalQuantityPerProduct  
Given a list of `OrderItem{ProductId,Quantity}` across multiple orders, compute total quantity sold per product as `{ProductId, TotalQty}`. (Flattening + GroupBy + aggregation)

5) JoinProductsAndTotals  
Given `products` and totals per product (from problem 4), join them to produce `{ProductName, TotalQty, Revenue}` where Revenue = TotalQty * Price. (Join + projection)

6) FlattenOrdersToItems  
Given `orders` with nested `Items` lists, produce a flat sequence of `{OrderId, ProductId, Quantity}` for all items. (SelectMany + projection)

7) GroupCustomersByCountry  
Given a list of `Customer{Id, Name, Country}`, group customers by country into `Dictionary<string, List<Customer>>` sorted by customer name within each group. (GroupBy + grouping materialization + sorting)

8) BuildInvertedIndex  
Given `Document{Id, Content}` (Content is string), build an inverted index mapping lowercased word → distinct list of document ids containing that word. (Split words, SelectMany, GroupBy/ToLookup + distinct)

9) PaginateProducts  
Implement pagination over products: given pageNumber and pageSize, return the correct page (1-based pages), plus total count. Use LINQ operators to produce the page. (Ordering + Skip/Take + Count)

10) DeferredExecutionPitfall  
Demonstrate deferred execution vs materialization: create an `IEnumerable<int>` pipeline with `Where` that uses a changing external variable, show difference between enumerating directly and materializing with `ToList()` before changing the external variable. (Deferred execution concept)