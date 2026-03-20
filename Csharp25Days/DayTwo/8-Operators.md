## Basic operators

### Arithmetic
- `+`, `-`, `*`, `/`, `%`  
  Integer division truncates; use floating types for fractional results.
```csharp
int a = 7 / 2;     // 3
double b = 7.0 / 2; // 3.5
```

### Unary
- `+`, `-`, logical negation `!`, increment/decrement `++`, `--`
```csharp
int x = 1;
x++; // x becomes 2
bool ok = !false; // true
```

### Assignment and compound assignment
- `=`, `+=`, `-=`, `*=`, `/=`, `%=` — compound operators perform an operation then assign:
```csharp
int n = 5;
n += 3; // n = 8
```

### Comparison
- `==`, `!=`, `<`, `<=`, `>`, `>=`  
Used in conditionals; comparing reference types with `==` depends on operator overloads (strings compare by value).

### Logical (boolean) operators
- `&&` (AND), `||` (OR) — short-circuiting
- `&`, `|`, `^` — bitwise (and logical non-short-circuiting for bool)
```csharp
if (a > 0 && ComputeHeavy()) { ... } // ComputeHeavy runs only if a > 0
```

### Bitwise (integers)
- `&`, `|`, `^`, `~`, `<<`, `>>` for integer bit manipulation.

### Ternary conditional
- `condition ? exprIfTrue : exprIfFalse` — concise conditional expression.
```csharp
string s = (n % 2 == 0) ? "even" : "odd";
```

### Null-coalescing
- `??` returns left operand if not null, otherwise right.
```csharp
string name = input ?? "Unknown";
```
- Null-coalescing assignment `??=` assigns only if left is null:
```csharp
name ??= "Unknown";
```

### Precedence
Operators have precedence; use parentheses to clarify:
```csharp
int result = (a + b) * c;
```

## Type conversions and casts
- Implicit conversions exist when safe (e.g., `int` → `long`).
- Explicit casts required when narrowing (e.g., `double` → `int`):
```csharp
double d = 3.7;
int i = (int)d; // 3
```
- Use `checked` to detect overflow on casts or arithmetic:
```csharp
checked { int x = (int)big; } // throws if overflow
```

## Best practices & teaching tips
- Encourage clear code: use `var` for brevity when the type is obvious; otherwise prefer explicit types.
- Teach difference between `const` and `readonly` with examples.
- Demonstrate integer division vs floating-point division.
- Show short-circuiting with side-effect methods to reinforce evaluation order.
- Provide quick exercises: refactor a block using `var`, replace repeated string concatenation with `StringBuilder`, and predict results of mixed-type expressions.

Summary: Choosing implicit vs explicit typing balances brevity and clarity. Use `const` for compile-time constants and `readonly` for runtime-one-time values. Mastering basic operators and conversion rules prevents common bugs like unintended integer division or silent data loss during casts.