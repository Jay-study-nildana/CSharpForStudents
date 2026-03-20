# Day 6 — OOP Practice Problems (Classes, Properties, Constructors, Encapsulation)

Instructions: For each problem below, implement the class(es) in C#. Provide:
- Class declaration(s) with fields, properties, and constructors.
- Example initialization code (how a caller creates/uses the class).
- A brief note explaining encapsulation choices (why fields are private, why properties are read-only, etc.).
- For collection-returning members, prefer returning read-only views or copies.

Problems:

1. Design_Book_Class  
   - Create a Book class with Title, Author, Pages. Validate inputs in the constructor. Provide a ToString override. Show example creation.

2. Student_With_Borrowing  
   - Create a Student class that stores an internal list of borrowed Book objects. Provide Borrow(Book) and Return(Book) methods and expose BorrowedBooks as an IReadOnlyList<Book>. Demonstrate how encapsulation prevents external mutation.

3. BankAccount_Encapsulation  
   - Build a BankAccount class with private balance field, Deposit(decimal) and Withdraw(decimal) methods, and a public read-only Balance property. Enforce business rules (no negative deposits/withdrawals).

4. Immutable_Point  
   - Implement an immutable 2D Point class with X and Y set only at construction (use readonly fields or get-only properties). Show how to create a translated copy (e.g., Translate(dx, dy) returns a new Point).

5. Library_With_Private_Collections  
   - Implement a Library class that manages a collection of Book objects internally. Provide AddBook(Book), LendBook(string title, Student student), and GetAvailableBooks() that returns IReadOnlyCollection<Book>. Demonstrate safe usage.

6. Movie_With_Validation  
   - Create a Movie class with Title, ReleaseYear, Rating (0.0–10.0). Validate ranges in constructor and property setters (if any). Show invalid input handling.

7. Rectangle_With_Area_Property  
   - Create Rectangle with Width and Height (init or get-only) and a computed Area property. Demonstrate initialization and usage of Area.

8. Person_Constructor_Overloads  
   - Implement a Person class with overloaded constructors: (name), (name, age), (name, dateOfBirth). Show constructor chaining and guard clauses.

9. Product_With_Static_IdGenerator  
   - Build Product class with auto-generated unique Id using a static counter. Use thread-safe increment (Interlocked). Show product creation assigns sequential Ids.

10. Car_Defensive_Copying  
    - Implement Car with a collection (e.g., string[] features or List<string>). Ensure constructor and property accessors perform defensive copies to avoid exposing internal arrays/lists. Demonstrate that external modifications don't change internal state.

Deliverable:
- Implement each problem as a small, self-contained C# example program, with a Main method demonstrating typical usage and the encapsulation choices.