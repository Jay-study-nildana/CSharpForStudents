# Below are 12 simple console program ideas

You can use as short in-class demonstrations. For each I give a short description, the core concepts it illustrates, suggested demo time (for a 2‑hour session), and one or two easy extensions you can assign as homework.

1) Greeting app
- Description: Ask the user for their name and print a personalized greeting.
- Concepts: Console input/output, string handling, simple flow.
- Demo time: 5–8 minutes.
- Extension: Ask for age and respond differently for minors/adults.

2) Simple calculator
- Description: Prompt for two numbers and an operation (+, -, *, /), then print the result.
- Concepts: Parsing input, arithmetic operators, basic validation.
- Demo time: 8–12 minutes.
- Extension: Support multiple operations in a loop until the user chooses to exit.

3) Temperature converter
- Description: Convert temperatures between Celsius and Fahrenheit based on user choice.
- Concepts: Functions/method calls (conceptual), numeric conversions, formatting output.
- Demo time: 6–10 minutes.
- Extension: Add Kelvin and input validation for numeric ranges.

4) Even/odd and divisibility checker
- Description: Read an integer and report whether it’s even/odd and whether divisible by 3 or 5.
- Concepts: Modulo operator, conditional branching, combining boolean conditions.
- Demo time: 5–8 minutes.
- Extension: Report all divisors or prime-check the number.

5) Number guessing game
- Description: Program picks a random number; user guesses until correct; program gives higher/lower hints and counts attempts.
- Concepts: Random numbers, loops, conditionals, basic program state.
- Demo time: 10–15 minutes.
- Extension: Add difficulty levels that change the number range; limit attempts.

6) FizzBuzz
- Description: For numbers 1..N, print “Fizz” for multiples of 3, “Buzz” for multiples of 5, “FizzBuzz” for both, otherwise the number.
- Concepts: Looping, conditional precedence, concise logic.
- Demo time: 6–10 minutes.
- Extension: Make it functional-style with small helper methods (conceptually).

7) Multiplication table
- Description: Print a multiplication table for a given number or an NxN table.
- Concepts: Nested loops, formatted output, iteration.
- Demo time: 6–10 minutes.
- Extension: Align columns nicely or print a range of tables.

8) Palindrome checker
- Description: Read a string and report whether it reads the same forwards and backwards (ignore spaces/case).
- Concepts: String processing, normalization, indexing.
- Demo time: 8–12 minutes.
- Extension: Check palindromic phrases (ignore punctuation) or find the longest palindromic substring (advanced).

9) Word/character counter
- Description: Read a line or paragraph and count words and characters.
- Concepts: Splitting strings, trimming, simple aggregation.
- Demo time: 6–10 minutes.
- Extension: Count frequency of each word and display the top 5 most common words.

10) Sum and average from a list
- Description: Ask the user to enter several numbers (or read until blank), then compute sum and average.
- Concepts: Collections (conceptual), parsing, accumulation, guarding against divide-by-zero.
- Demo time: 6–10 minutes.
- Extension: Track min/max and present a summary (count, min, max, average).

11) To‑Do list (in‑memory)
- Description: A simple menu-driven app to add, list, and remove tasks held in memory for the session.
- Concepts: Menus/loops, lists/collections (conceptual), basic CRUD operations.
- Demo time: 12–18 minutes (good for showing program structure and loops).
- Extension: Save/load tasks to a file (introduces file I/O on later days).

12) Simple file logger (intro to I/O)
- Description: Prompt the user for messages and append them to a text file; list saved messages on command.
- Concepts: File I/O (append/read), error handling (file not found/permission), basic persistence.
- Demo time: 10–15 minutes (if you want to show how programs can persist data).
- Extension: Date-stamp entries and implement simple rotation (new file per day).

How to use these in a Day‑1 demo
- Start with short demos (1–3, 4) to illustrate input/output and program flow.
- Use one or two slightly longer examples (guessing game, to‑do list) to show loops and state.
- Emphasize the conceptual building blocks: input → processing → output, where the entry point is, and how you’d break behavior into methods/classes later.
- End with an extension idea as homework (e.g., convert the calculator to loop until exit, or add file saving to the to‑do list).

Timing tips
- Keep each demonstration focused on one or two concepts; don’t overload with too many language details.
- Show the program running first, then walk through the structure and explain where you’d change behavior.
- Use failing inputs on purpose to demonstrate validation and how the program should handle errors.