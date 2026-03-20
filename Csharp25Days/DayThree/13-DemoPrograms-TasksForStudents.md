# Day 3 — Control Flow Problems (if/else, switch, for, while, foreach)

Instructions: For each problem below, implement the solution in C#. Comment which control flow constructs you chose and why. Include time and space complexity (Big-O). The provided solution files use the problem titles as filenames (underscores used). You may implement for practice, or produce pseudocode and complexity notes.

Problems:
1. Sum_Array
   - Given an array of integers, return the sum of all elements.
   - Input: int[] numbers
   - Output: int sum
   - Hint: `for` or `foreach`

2. Max_and_Index
   - Find the maximum value in an integer array and its first index.
   - Input: int[] numbers
   - Output: (maxValue, index)
   - Hint: `for` if you need the index.

3. Count_Evens_Odds
   - Given an array of integers, return counts of even and odd numbers.
   - Input: int[] numbers
   - Output: (evenCount, oddCount)
   - Hint: `foreach` is simple and readable.

4. Reverse_Array_In_Place
   - Reverse an array in place (modify the given array).
   - Input: int[] numbers
   - Output: same array reversed
   - Hint: `for` with two indices (start/end); consider off-by-one.

5. Remove_Odd_From_List
   - Given a List<int>, remove all odd numbers and return the modified list.
   - Input: List<int> list
   - Output: List<int> with only even numbers
   - Hint: Avoid modifying a collection while iterating with `foreach`. Use backward `for` or List.RemoveAll.

6. First_Negative_Index
   - Return the index of the first negative number in an array; return -1 if none.
   - Input: int[] numbers
   - Output: int index or -1
   - Hint: `for` with `break` to allow early exit.

7. Is_Prime
   - Determine whether a given integer n (>1) is prime.
   - Input: int n
   - Output: bool isPrime
   - Hint: Use `for` or `while` up to sqrt(n) with early exit.

8. FizzBuzz_Range
   - For integers from 1 to N, print "Fizz" for multiples of 3, "Buzz" for multiples of 5, "FizzBuzz" for multiples of both, otherwise print the number.
   - Input: int N
   - Output: sequence of strings/lines
   - Hint: `for`, and `%` checks with ordered `if`/`else if`.

9. Histogram_Counts
   - Given an array of integers where values are in a small fixed range [0..K], return an array of counts for each value.
   - Input: int[] numbers, int K
   - Output: int[] counts of length K+1
   - Hint: `for` or `foreach`; counts array indexed by value.

10. Grade_Calculator_Switch
    - Given an integer score 0..100, return letter grade: A (90–100), B (80–89), C (70–79), D (60–69), F (<60). Use a `switch` or switch expression.
    - Input: int score
    - Output: string grade
    - Hint: Map to tens place and `switch` on that value, or use `if` ladder.

Each problem should include:
- Correctness (handle edge cases and boundaries).
- Complexity comments: time and space Big-O.
- A short note: Which construct you chose and why (if/else, switch, for, while, foreach).

Good practice: for removal or mutation tasks, explain why you avoid `foreach` if mutating the same collection. For tasks with early exit (search/isPrime), show how `break` or returning early reduces average case work.