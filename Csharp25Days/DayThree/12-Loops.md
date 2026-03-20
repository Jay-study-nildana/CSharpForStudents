# For, While, Foreach — Choosing the Right Loop (C# / .NET)

Objectives: understand syntax and common patterns for `for`, `while`, and `foreach`; choose the appropriate loop for a task; watch for common pitfalls; add complexity comments.

## Quick summary
- `for` — use when you know or can compute the number of iterations (index-based iteration).
- `while` — use when you continue until a condition changes (unknown count up front).
- `foreach` — use to iterate collections/arrays safely and readably (no index needed).

---

## Syntax & simple examples

for (index-based, typical when you need the index)
```csharp
int[] numbers = { 1, 2, 3, 4, 5 };
int sum = 0;
for (int i = 0; i < numbers.Length; i++)
{
    sum += numbers[i];
}
// Time: O(n), Space: O(1)
```

while (condition-driven, good for loops that stop on a condition)
```csharp
int i = 0;
int sum = 0;
while (i < numbers.Length)
{
    sum += numbers[i];
    i++;
}
// Equivalent to the for loop above; Time: O(n), Space: O(1)
```

foreach (iterate collections, simple and safe; cannot change collection structure)
```csharp
int sum = 0;
foreach (int n in numbers)
{
    sum += n;
}
// Time: O(n), Space: O(1)
```

---

## When to use which loop

- Use `for`:
  - You need the index (e.g., writing to another array, reversing, skipping by step).
  - You need to iterate a precise number of times (e.g., repeat N times).
  - Example: iterate backward to remove items safely from a list.

- Use `while`:
  - You don't know how many iterations will be needed (e.g., read until a sentinel, retry until success).
  - The termination condition is based on changing state checked each iteration.

- Use `foreach`:
  - Read-only traversal of a collection for clarity and safety.
  - Best when you don't need the index and won't modify the collection.

---

## Common patterns and tips

- Counting/accumulation: `for` or `foreach` are both fine; `foreach` is cleaner if you only read values.
- Searching for first match: `foreach` with `break` or `for` (if index is needed).
- Removing items from a List<T>: do NOT modify a collection in a `foreach` — either:
  - iterate backwards with `for` and call `RemoveAt(i)`, or
  - build a new collection or use `List<T>.RemoveAll(predicate)`.
```csharp
// Remove even numbers safely using backward for-loop
List<int> list = new List<int> { 1, 2, 3, 4, 5, 6 };
for (int i = list.Count - 1; i >= 0; i--)
{
    if (list[i] % 2 == 0) list.RemoveAt(i);
}
// Time: O(n^2) worst-case removing many items (each RemoveAt shifts elements),
// consider using RemoveAll for O(n).
```

- Off-by-one errors: pay attention to `<` vs `<=` in `for` bounds.
- Infinite loops: always ensure loop variables or conditions will eventually change.

---

## Example: Find first negative number (showing break)
```csharp
int[] arr = { 3, 7, -1, 2 };
int index = -1;
for (int i = 0; i < arr.Length; i++)
{
    if (arr[i] < 0)
    {
        index = i;
        break; // stop early when found
    }
}
// Time: O(n) worst-case, O(1) early-exit when found
```

---

## Homework (4 small tasks — implement or pseudocode, include complexity)
Pick one loop for each task and comment why; include time/space complexity.

1. Sum an array of integers (choose `foreach` or `for`) — Time: O(n).
2. Find maximum value and its index (use `for` if you need index) — Time: O(n).
3. Remove all odd numbers from a List<int> (show safe removal strategy; prefer `RemoveAll` or backward `for`) — Time: O(n) with `RemoveAll`.
4. Check whether a number is prime (use `for` or `while` up to sqrt(n)`; stop early when divisor found) — Time: O(sqrt(n)).

Example pseudocode for task 4:
```text
i = 2
while i * i <= n:
    if n % i == 0:
        return false
    i = i + 1
return n > 1
// Time: O(sqrt(n)), Space: O(1)
```

---

## Final notes
- Prefer clarity: `foreach` when you only read; `for` when you need indexing or controlled steps; `while` for condition-driven loops.
- Always comment complexity (Big-O) for homework.
- Watch for modifying collections during iteration and off-by-one/infinite-loop mistakes.

Good luck — bring one sample of each loop to class so we can compare choices and their flowcharts.