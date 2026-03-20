# Day 6 — Introduction to Object‑Oriented Programming (C# / .NET)

Objectives: understand classes & objects, fields vs properties, constructors, encapsulation and information hiding. This guide uses C# examples and design notes to help you model real-world entities and write robust classes.

---

## What is a class and an object?

- A *class* is a blueprint that defines data and behavior (fields, properties, methods, events).
- An *object* (instance) is a concrete value created from a class at runtime.

Think: class = recipe, object = baked cake.

Example: declare a class and create an object
```csharp
public class Book
{
    // members (fields, properties, methods) go here
}

var book = new Book(); // create an instance
```

---

## Fields vs Properties

- Field: a variable declared inside a class. Usually private.
  - Example: `private int _pages;`
- Property: exposes data with `get`/`set` accessors. Prefer properties for public data.
  - Example: `public int Pages { get; private set; }`

Why use properties instead of public fields?
- Properties define a stable contract — you can add validation or computation later without changing callers.
- Properties can be read-only, write-only, computed, or init-only.

Examples

Private field with validation via property:
```csharp
public class Book
{
    private int _pages;

    public int Pages
    {
        get => _pages;
        private set
        {
            if (value < 0) throw new ArgumentOutOfRangeException(nameof(Pages));
            _pages = value;
        }
    }

    public string Title { get; init; } // init-only (C# 9+)
}
```

Auto-properties (concise):
```csharp
public string Title { get; set; }    // read/write
public string Author { get; init; }  // settable only during initialization
```

Avoid public fields:
```csharp
// Bad: public fields expose internal representation
public int pages;
```

---

## Constructors and object initialization

A constructor initializes an object's state. You can overload constructors for different initialization styles.

Primary constructor with validation:
```csharp
public class Book
{
    public string Title { get; }
    public string Author { get; }
    public int Pages { get; private set; }

    public Book(string title, string author, int pages)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Author = author ?? "Unknown";
        if (pages < 0) throw new ArgumentOutOfRangeException(nameof(pages));
        Pages = pages;
    }

    // convenience constructor
    public Book(string title, string author) : this(title, author, 0) { }
}
```

Object initializer syntax (works with public set or `init`):
```csharp
var b = new Book("1984", "Orwell", 328);
var b2 = new Book("Notes", "Anon") { /* init-only properties could be set here */ };
```

Constructor best practices:
- Validate arguments early (guard clauses).
- Keep constructors small; delegate complex initialization to private methods.
- If many optional settings exist, consider the Builder pattern or factory methods to keep constructors readable.

---

## Encapsulation and information hiding

Encapsulation = hiding internal details and exposing a minimal, safe public surface.

Example: BankAccount protects its balance
```csharp
public class BankAccount
{
    private decimal _balance;

    public string Owner { get; }
    public decimal Balance => _balance; // read-only property

    public BankAccount(string owner, decimal initialDeposit = 0)
    {
        Owner = owner ?? throw new ArgumentNullException(nameof(owner));
        if (initialDeposit < 0) throw new ArgumentOutOfRangeException(nameof(initialDeposit));
        _balance = initialDeposit;
    }

    public void Deposit(decimal amount)
    {
        if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount));
        _balance += amount;
    }

    public bool Withdraw(decimal amount)
    {
        if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount));
        if (amount > _balance) return false;
        _balance -= amount;
        return true;
    }
}
```

Why this is good:
- The `_balance` field cannot be set arbitrarily by callers.
- All changes happen through methods that enforce business rules.

---

## Access modifiers and immutability

- Common access modifiers: `public`, `private`, `protected`, `internal`, `protected internal`.
- Principle: prefer the least permissive modifier that still meets the need (minimize public surface).
- Make objects immutable when possible — simpler reasoning, thread-safety benefits.

Immutability examples:
```csharp
public class Student
{
    public int Id { get; }
    public string Name { get; }  // read-only

    public Student(int id, string name)
    {
        Id = id;
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }
}
```

`readonly` fields vs `init` properties:
```csharp
public readonly int Id;        // set in constructor only
public string Course { get; init; } // set during initialization only (object initializer)
```

---

## Computed properties and expression-bodied members

Properties can compute values instead of storing them:
```csharp
public class Rectangle
{
    public double Width { get; init; }
    public double Height { get; init; }
    public double Area => Width * Height; // computed
}
```

Expression-bodied methods:
```csharp
public override string ToString() => $"{Title} by {Author} ({Pages} pages)";
```

---

## Collections & exposing internal state safely

If you hold collections, avoid exposing mutable lists directly. Use `IReadOnlyList<T>` or return copies.

Bad:
```csharp
public List<Book> Borrowed = new List<Book>(); // public and mutable
```

Good:
```csharp
private readonly List<Book> _borrowed = new();
public IReadOnlyList<Book> BorrowedBooks => _borrowed.AsReadOnly();

public void Borrow(Book book)
{
    if (book == null) throw new ArgumentNullException(nameof(book));
    _borrowed.Add(book);
}
```

This preserves encapsulation and allows you to change internal implementation without breaking callers.

---

## Methods vs property setters

- Use properties for state access.
- Use methods for actions that have behavior/semantics (e.g., `Withdraw`, `Borrow`, `Enroll`).
- Prefer methods when validation, side-effects, or domain logic is involved.

Example: prefer `account.Withdraw(amount)` over `account.Balance -= amount`.

---

## Small modeling example: Book & Student

Putting principles together:
```csharp
public class Book
{
    public string Title { get; }
    public string Author { get; }
    public int Pages { get; }

    public Book(string title, string author, int pages)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Author = author ?? "Unknown";
        Pages = pages >= 0 ? pages : throw new ArgumentOutOfRangeException(nameof(pages));
    }

    public override string ToString() => $"{Title} by {Author}";
}

public class Student
{
    public int Id { get; }
    public string Name { get; private set; }
    private readonly List<Book> _borrowed = new();
    public IReadOnlyList<Book> BorrowedBooks => _borrowed.AsReadOnly();

    public Student(int id, string name) { Id = id; Name = name; }

    public void Borrow(Book book)
    {
        if (book == null) throw new ArgumentNullException(nameof(book));
        _borrowed.Add(book);
    }
}
```

---

## Common pitfalls & tips

- Don’t expose mutable internals (lists, arrays) directly.
- Avoid public fields — use properties.
- Validate inputs in constructors and methods to maintain class invariants.
- Keep methods short and focused (single responsibility).
- Use exceptions for exceptional conditions, not for ordinary control flow.
- Favor composition over inheritance when modeling relationships.

---

## Classroom exercises / Homework suggestions

1. Design a `Movie` class: decide which fields/properties, write constructors, and show two different ways to create instances (constructor + object initializer). Explain encapsulation choices.
2. Model a `Library` that holds `Book` objects. Provide methods to `AddBook`, `LendBook`, and `ReturnBook`. Explain how you prevent external code from removing books from the library without using your methods.
3. Write a short paragraph explaining why properties are preferred over public fields, with an example where changing a field to a property later would prevent breaking callers.

For each task include:
- Class signatures and constructors.
- A brief explanation of access modifiers and why you chose them.
- Example initialization code showing typical usage.

---

Final note
- Classes let you model real-world entities; properties and constructors control initialization; encapsulation keeps objects valid. Use small, focused methods for behavior, and choose the narrowest access level that satisfies your design.

Bring one real-world object (e.g., Phone, Course, Order) to class and we will design its class diagram and constructors together.