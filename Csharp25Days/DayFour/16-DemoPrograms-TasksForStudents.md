# Day 4 — Methods & Parameters: 10 Practice Problems

Context: Methods, parameter passing (value/ref/out), return values (single value, tuples), overloading, default parameters, params, scope and lifetime (locals, fields, closures), and decomposition into smaller methods.

Instructions: For each problem below implement the solution in C#. Provide:
- Method-level signatures and implementations.
- A brief note explaining which parameter passing style you used (value/ref/out) and why.
- Time and space complexity (Big-O).
- Any scope/lifetime considerations or edge cases.

Problems:

1. Sum_And_Average
   - Given an int[] input, implement two methods:
     - int Sum(int[] values)
     - (int sum, double average) SumAndAverage(int[] values)
   - Return average as 0.0 for empty arrays.
   - Hint: return tuple for multiple values.

2. Swap_With_Ref
   - Implement void Swap(ref int a, ref int b) to swap caller variables.
   - Demonstrate that passing by value does not swap the caller’s variables.
   - Hint: use `ref`.

3. TryParsePositive_Out
   - Implement bool TryParsePositive(string s, out int value) that returns true and sets value only if s parses to a positive integer (>0).
   - Demonstrate usage and caller-side checking.

4. Overloaded_Print
   - Create overloaded methods Print(string), Print(string, int times), Print(string, ConsoleColor).
   - Show compile-time overload resolution and usage.
   - Comment on ambiguity and how default parameters affect overloads.

5. Params_VarArgs_Sum
   - Implement int SumAll(params int[] numbers) which accepts a variable number of ints (0..n) and returns their sum.
   - Demonstrate calling with an array and with individual ints.

6. Recursive_Factorial
   - Implement long Factorial(int n) recursively (n >= 0).
   - Comment on local-variable scope and recursion depth limits.
   - Include iterative alternative as a second method.

7. ReferenceType_Mutation_vs_Reassignment
   - Demonstrate difference between mutating an object passed into a method (e.g., List<T>.Add) and reassigning the parameter to a new object.
   - Implement void Mutate(List<int> list) and void Reassign(List<int> list).
   - Show how `ref` can be used to allow reassignment to affect caller.

8. Overload_And_Defaults
   - Implement PrintReport(string title, bool detailed = false) and an overloaded PrintReport(string title, int copies).
   - Show how defaults and overloads interact, and best practices.

9. Text_Decomposition_Methods
   - Decompose a text-processing task into small methods:
     - string Normalize(string s)
     - string[] Tokenize(string s)
     - Dictionary<string,int> CountWords(string[] tokens)
     - (Use these to implement) Dictionary<string,int> WordFrequency(string text)
   - Show method signatures and implementations, and explain single-responsibility in each helper.

10. Password_Validation_Methods
    - Create small methods that validate components of a password:
      - bool HasLength(string p, int min)
      - bool HasUpperLowerDigit(string p)
      - bool HasSpecialChar(string p)
      - (Compose) int PasswordStrength(string p) // returns score 0..4
    - Return value is an int score; discuss returning boolean vs integer and side effects (logging).

Notes for students:
- Include guard clauses and argument validation for each method.
- Prefer returning values/tuples over `out`/`ref` unless necessary.
- Document scope/lifetime effects (e.g., captured variables in lambdas, instance field lifetimes).
- For each problem provide sample inputs and expected outputs.