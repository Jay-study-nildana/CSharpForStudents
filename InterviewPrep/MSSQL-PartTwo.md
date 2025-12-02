# SQL Server — Functions, Subqueries, Views, Stored Procedures & Triggers  
Detailed Reference with T‑SQL Examples

---

## Table of Contents

1. [Scope & Purpose](#scope--purpose)  
2. [Functions in SQL — Overview (SQL Server)](#functions-in-sql---overview-sql-server)  
3. [String Functions — Common Usage & Examples](#string-functions---common-usage--examples)  
4. [Numeric Functions — Common Usage & Examples](#numeric-functions---common-usage--examples)  
5. [Date & Time Functions — Common Usage & Examples](#date--time-functions---common-usage--examples)  
6. [Aggregate Functions & Grouping Extensions](#aggregate-functions--grouping-extensions)  
7. [Generate Groups / Sequences (Tally / GENERATE_SERIES / CTE)](#generate-groups--sequences-tally--generateseries--cte)  
8. [SQL Subqueries — Non‑Correlated & Correlated](#sql-subqueries---non-correlated--correlated)  
9. [Views — Simple, Complex, Creating & Altering (SQL Server)](#views---simple-complex-creating--altering-sql-server)  
10. [Indexed Views & Schema Binding (Materialized-like)](#indexed-views--schema-binding-materialized-like)  
11. [Stored Procedures — Concepts & Examples](#stored-procedures---concepts--examples)  
    - System vs User-defined  
    - Input / OUTPUT parameters and optional params  
    - RETURN code conventions  
12. [User-defined Functions — Scalar & Table-valued (inline & multi-statement)](#user-defined-functions---scalar--table-valued-inline--multi-statement)  
13. [CTE & Recursive CTE (within queries, functions and procs)](#cte--recursive-cte-within-queries-functions-and-procs)  
14. [Triggers — Types, Magic Tables & Examples (T‑SQL)](#triggers---types-magic-tables--examples-t-sql)  
    - DML Triggers (AFTER / INSTEAD OF)  
    - DDL Triggers (Database / Server)  
    - Triggers on Views (INSTEAD OF)  
    - Recursion, nesting, safety controls  
15. [Best Practices & Performance Considerations](#best-practices--performance-considerations)  
16. [Common Pitfalls & How to Avoid Them](#common-pitfalls--how-to-avoid-them)  
17. [Short Q&A — Key Concepts](#short-qa---key-concepts)  
18. [References & Further Reading](#references--further-reading)

---

## 1. Scope & Purpose

This reference targets Microsoft SQL Server (T‑SQL). It covers built-in functions (string, numeric, date/time), aggregates, generating sequences, subqueries (correlated/non‑correlated), views (including indexed views), stored procedures, user-defined functions (scalar and table-valued), CTEs and recursive CTEs, and triggers (DML/DDL/INSTEAD OF). Each section contains practical T‑SQL examples and notes about behavior, restrictions and performance.

---

## 2. Functions in SQL — Overview (SQL Server)

- Built-in scalar functions return a single value per input row (e.g., UPPER, SUBSTRING).
- Aggregate functions operate over groups of rows (SUM, AVG, COUNT).
- Window functions (OVER()) compute aggregates for each row over a partition.
- User-defined functions (UDFs) can be scalar or table-valued (inline or multi-statement).
- Stored procedures are routines that can perform actions (DML/DDL) and return result sets or output parameters; procedures cannot be used in expressions but can call functions.

Important T‑SQL notes:
- Some built-ins are nondeterministic (GETDATE(), NEWID(), RAND()), which affects indexed view eligibility and replication.
- UDFs have restrictions: scalar UDFs run in row context and can cause performance penalties; inline TVFs are preferred for set-based logic.

---

## 3. String Functions — Common Usage & Examples

Common functions:
- LEN(), DATALENGTH(), LEFT(), RIGHT(), SUBSTRING(), CHARINDEX(), PATINDEX(), REPLACE(), LOWER()/UPPER(), LTRIM()/RTRIM(), TRIM(), CONCAT(), FORMAT(), STUFF(), STRING_AGG (for group concat, SQL Server 2017+), QUOTENAME()

Examples:

Concatenate safely:
```sql
SELECT CONCAT(FirstName, ' ', ISNULL(MiddleName + ' ', ''), LastName) AS FullName
FROM dbo.Person;
```

Substring and search:
```sql
SELECT SUBSTRING(Notes, 1, 200) AS Excerpt,
       CHARINDEX('error', LOWER(Notes)) AS ErrorPos
FROM dbo.Log;
```

Trim & length:
```sql
SELECT TRIM(Both ' ' FROM Name) AS TrimmedName,
       LEN(TRIM(Name)) AS CharCount
FROM dbo.Company;
```

Replace and remove non-digits (using TRANSLATE + REPLACE or the regex-like approach with CLR or 2017+ STRING_AGG tricks). A simple pattern to keep digits:
```sql
-- SQL Server 2017+ using TRANSLATE to remove common punctuation (example)
SELECT TRANSLATE(Phone, '()- .', '') AS DigitsOnly FROM dbo.Contacts;
```

Aggregating strings per group:
```sql
-- SQL Server 2017+: STRING_AGG
SELECT CustomerId,
       STRING_AGG(ProductName, ', ') WITHIN GROUP (ORDER BY ProductName) AS Products
FROM dbo.Orders o
JOIN dbo.OrderLines l ON o.Id = l.OrderId
GROUP BY CustomerId;
```

Historic approach (pre-2017) — FOR XML PATH:
```sql
SELECT c.CustomerId,
  STUFF((
    SELECT ', ' + p.Name
    FROM dbo.OrderLines ol JOIN dbo.Products p ON ol.ProductId = p.Id
    WHERE ol.OrderId = o.Id
    FOR XML PATH(''), TYPE).value('.', 'nvarchar(max)'), 1, 2, '') AS ProductList
FROM dbo.Orders o;
```

Notes:
- Be careful with NVARCHAR vs VARCHAR and using COLLATION when comparing or concatenating.

---

## 4. Numeric Functions — Common Usage & Examples

Common functions:
- ABS(), ROUND(), CEILING(), FLOOR(), POWER(), SQRT(), SIGN(), RAND(), LOG(), LOG10(), EXP(), MOD (use %), TRY_CAST/TRY_CONVERT.

Rounding examples:
```sql
SELECT Amount,
       ROUND(Amount, 2) AS Rounded2,
       CEILING(Amount) AS CeilingVal,
       FLOOR(Amount) AS FloorVal
FROM dbo.Transactions;
```

Random numbers:
```sql
-- RAND() without seed returns same value per batch; NEWID() trick for per-row randomness
SELECT TOP(10) NEWID() AS rnd_guid, RAND(CHECKSUM(NEWID())) AS rnd_val
FROM sys.objects; -- not deterministic distribution control
```

Safe conversions:
```sql
SELECT TRY_CAST('123.45' AS DECIMAL(10,2)) AS ValueNumeric,
       TRY_CAST('abc' AS INT) AS InvalidIntNull; -- returns NULL instead of error
```

Bucketizing with CASE:
```sql
SELECT Score,
       CASE
         WHEN Score >= 90 THEN 'A'
         WHEN Score >= 80 THEN 'B'
         WHEN Score >= 70 THEN 'C'
         ELSE 'F'
       END AS Grade
FROM dbo.Results;
```

Notes:
- Use DECIMAL/NUMERIC for currency to avoid floating point issues.
- Use TRY_CAST to avoid conversion errors when working with dirty data.

---

## 5. Date & Time Functions — Common Usage & Examples

Common functions:
- GETDATE(), SYSDATETIME(), SYSUTCDATETIME(), GETUTCDATE()
- DATEADD(), DATEDIFF(), DATENAME(), DATEPART(), EOMONTH(), FORMAT(), CONVERT(), SWITCHOFFSET/AT TIME ZONE
- DATEFROMPARTS, DATETIMEFROMPARTS

Examples:

Current date/time and parts:
```sql
SELECT GETDATE() AS NowLocal, SYSUTCDATETIME() AS NowUTC,
       YEAR(OrderDate) AS Yr, MONTH(OrderDate) AS Mo, DAY(OrderDate) AS Day
FROM dbo.Orders;
```

Date arithmetic:
```sql
SELECT OrderId, OrderDate,
       DATEADD(day, 30, OrderDate) AS DueDate,
       DATEDIFF(day, OrderDate, GETDATE()) AS DaysSinceOrder
FROM dbo.Orders;
```

End of month:
```sql
SELECT OrderDate, EOMONTH(OrderDate) AS EndOfMonth
FROM dbo.Orders;
```

Time zone handling (2016+):
```sql
-- Convert from UTC to Pacific Standard Time
SELECT OrderId,
       OrderDate AT TIME ZONE 'UTC' AT TIME ZONE 'Pacific Standard Time' AS OrderDatePST
FROM dbo.Orders;
```

Formatting for display:
```sql
SELECT FORMAT(OrderDate, 'yyyy-MM-dd HH:mm') FROM dbo.Orders; -- slower; prefer CONVERT for fixed formats
```

Range queries (SARGable):
```sql
-- SARGable range: avoid wrapping column in functions
WHERE OrderDate >= '2025-01-01' AND OrderDate < '2025-02-01'
```

Notes:
- Prefer SARGable patterns; avoid applying functions to the column in WHERE clause (e.g., avoid CONVERT on column).
- Store times in UTC (DATETIME2 + time zone conversion at presentation) when possible.

---

## 6. Aggregate Functions & Grouping Extensions

Basic aggregates:
- COUNT(), COUNT_BIG(), SUM(), AVG(), MIN(), MAX(), STDEV(), VAR()

GROUP BY and HAVING:
```sql
SELECT CustomerId, COUNT(*) AS Orders, SUM(Total) AS TotalSpent
FROM dbo.Orders
GROUP BY CustomerId
HAVING SUM(Total) > 1000;
```

Grouping sets, ROLLUP, CUBE (T‑SQL):
```sql
-- ROLLUP for hierarchical subtotals
SELECT Region, ProductCategory, SUM(Sales) AS TotalSales
FROM dbo.Sales
GROUP BY ROLLUP (Region, ProductCategory);

-- CUBE for all combinations
SELECT Region, ProductCategory, SUM(Sales) AS TotalSales
FROM dbo.Sales
GROUP BY CUBE (Region, ProductCategory);
```

GROUPING & GROUPING_ID to identify subtotal rows:
```sql
SELECT Region, ProductCategory, SUM(Sales) AS TotalSales,
       GROUPING(Region) AS IsRegionSubtotal, GROUPING(ProductCategory) AS IsCategorySubtotal
FROM dbo.Sales
GROUP BY CUBE (Region, ProductCategory);
```

STRING_AGG (for string aggregation):
```sql
SELECT CustomerId, STRING_AGG(ProductName, ', ') AS Products
FROM dbo.OrderLines ol
JOIN dbo.Products p ON ol.ProductId = p.Id
GROUP BY CustomerId;
```

Windowed aggregates (OVER):
```sql
SELECT OrderId, CustomerId, OrderDate, Total,
       SUM(Total) OVER (PARTITION BY CustomerId ORDER BY OrderDate ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW) AS RunningTotal
FROM dbo.Orders;
```

Notes:
- COUNT_BIG returns bigint — use for very large tables or indexed views (indexed views require COUNT_BIG).
- Use window functions to avoid subqueries and allow per-row context.

---

## 7. Generate Groups / Sequences (Tally / GENERATE_SERIES / CTE)

Generating a sequence of numbers is a common building block for gap-filling, date ranges, test data.

Options in SQL Server:

A. GENERATE_SERIES (SQL Server 2022 / Azure SQL)
```sql
-- Generate numbers 1..10
SELECT value AS n FROM GENERATE_SERIES(1,10,1);

-- Generate dates
SELECT DATEADD(day, value, '2025-01-01') AS d
FROM GENERATE_SERIES(0, 30, 1);
```

B. Recursive CTE (works in older versions)
```sql
WITH numbers AS (
    SELECT 1 AS n
    UNION ALL
    SELECT n + 1 FROM numbers WHERE n < 10
)
SELECT n FROM numbers
OPTION (MAXRECURSION 0); -- 0 = unlimited (use with caution)
```

C. Tally table pattern (fast)
```sql
-- Create an ad-hoc tally using system tables
SELECT TOP (1000) ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS n
INTO #Tally
FROM sys.all_objects a CROSS JOIN sys.all_objects b;

-- Use #Tally for joins to generate sequences or expand rows
SELECT DATEADD(day, n-1, '2025-01-01') AS dt FROM #Tally WHERE n <= 31;
```

Example: fill missing dates for sales
```sql
DECLARE @Start DATE = '2025-01-01', @End DATE = '2025-01-10';

;WITH seq AS (
  SELECT DATEADD(day, value, @Start) AS dt
  FROM GENERATE_SERIES(0, DATEDIFF(day, @Start, @End))
)
SELECT s.dt, ISNULL(SUM(t.Sales),0) AS TotalSales
FROM seq s
LEFT JOIN dbo.Sales t ON CAST(t.SaleDate AS DATE) = s.dt
GROUP BY s.dt
ORDER BY s.dt;
```

Notes:
- Prefer GENERATE_SERIES where available for readability and performance.
- Tally tables are extremely efficient for larger sequences.

---

## 8. SQL Subqueries — Non‑Correlated & Correlated

Subqueries are queries nested inside other queries. Two main forms:

### Non‑correlated subquery
Independent of the outer query; executed once.
Example — scalar subquery:
```sql
SELECT Name, Salary,
       (SELECT AVG(Salary) FROM dbo.Employees) AS AvgSalary
FROM dbo.Employees;
```

Example — IN with list:
```sql
SELECT OrderId FROM dbo.Orders
WHERE CustomerId IN (SELECT CustomerId FROM dbo.Customers WHERE Status = 'Active');
```

Performance:
- Non-correlated subqueries are often optimized to be computed once.

### Correlated subquery
Refers to column(s) of outer query; evaluated per outer row.

Example — find orders greater than customer's average:
```sql
SELECT o.OrderId, o.CustomerId, o.Total
FROM dbo.Orders o
WHERE o.Total > (
    SELECT AVG(o2.Total)
    FROM dbo.Orders o2
    WHERE o2.CustomerId = o.CustomerId
);
```

Alternative with APPLY:
- CROSS APPLY and OUTER APPLY often express per-row subqueries more clearly and can be optimized.

Example using OUTER APPLY:
```sql
-- most recent order per customer
SELECT c.CustomerId, c.Name, lastOrder.LastOrderDate, lastOrder.LastTotal
FROM dbo.Customers c
OUTER APPLY (
   SELECT TOP (1) o.OrderDate AS LastOrderDate, o.Total AS LastTotal
   FROM dbo.Orders o
   WHERE o.CustomerId = c.CustomerId
   ORDER BY o.OrderDate DESC
) lastOrder;
```

Exists vs IN vs JOIN:
- Use EXISTS when you only need existence; it short-circuits effectively.
- IN can be problematic with NULLs — prefer EXISTS or INNER JOIN depending on semantics.
- JOIN returns duplicates if many matches; use DISTINCT or aggregation if needed.

Example using EXISTS:
```sql
SELECT c.CustomerId, c.Name
FROM dbo.Customers c
WHERE EXISTS (
  SELECT 1 FROM dbo.Orders o WHERE o.CustomerId = c.CustomerId AND o.Total > 1000
);
```

Notes:
- Correlated subqueries can be expensive; try to rewrite using joins, APPLY, or window functions if appropriate.
- Check execution plans (IN/EXISTS/joins may have different plans depending on optimizer).

---

## 9. Views — Simple, Complex, Creating & Altering (SQL Server)

Views are named queries stored in the database metadata and can simplify reuse and security.

Create a simple view:
```sql
CREATE VIEW dbo.vActiveCustomers
AS
SELECT CustomerId, Name, Email
FROM dbo.Customers
WHERE IsActive = 1;
GO
```

Use view:
```sql
SELECT * FROM dbo.vActiveCustomers WHERE Email LIKE '%@example.com';
```

Create complex view (joins, computed columns):
```sql
CREATE VIEW dbo.vOrderSummary
AS
SELECT o.OrderId, o.CustomerId, c.Name AS CustomerName,
       o.OrderDate, o.Total,
       (SELECT COUNT(*) FROM dbo.OrderLines l WHERE l.OrderId = o.OrderId) AS LineCount
FROM dbo.Orders o
JOIN dbo.Customers c ON o.CustomerId = c.CustomerId;
GO
```

Alter view:
```sql
ALTER VIEW dbo.vActiveCustomers
AS
SELECT CustomerId, Name, Email, CreatedAt
FROM dbo.Customers
WHERE IsActive = 1;
GO
```

Drop view:
```sql
DROP VIEW IF EXISTS dbo.vActiveCustomers;
```

Security & ownership chaining:
- Views can simplify permissions: GRANT SELECT on the view and not the underlying tables.
- Ownership chaining means if view owner and table owner are same, underlying permissions may not be checked; be careful with cross-schema ownership.

Updating data through views:
- Updatable views have restrictions: simple SELECT from a base table with primary key is typically updatable. Views with aggregates, joins or DISTINCT are not directly updatable.
- Use INSTEAD OF triggers on views to implement update/insert/delete behavior.

Notes:
- Avoid SELECT * in view definitions (schema changes may break dependent objects).
- Be mindful of performance—views do not store data (unless indexed).

---

## 10. Indexed Views & Schema Binding (Materialized-like)

SQL Server supports creating an index on a view (materialized view behavior) with requirements:

- View must be created WITH SCHEMABINDING.
- All referenced objects must be qualified with schema names.
- Deterministic functions only; no nondeterministic functions like GETDATE(), RAND() in the indexed portion.
- Use COUNT_BIG(*) in aggregate indexed views.
- Create unique clustered index first.

Example:
```sql
-- Example: sales per product per day (aggregated) as an indexed view
CREATE VIEW dbo.vProductDailySales
WITH SCHEMABINDING
AS
SELECT
  p.ProductId,
  CONVERT(date, o.OrderDate) AS OrderDate,
  SUM(ol.Quantity) AS TotalQty,
  SUM(ol.Quantity * ol.UnitPrice) AS TotalSales,
  COUNT_BIG(*) AS RowCount
FROM dbo.OrderLines ol
JOIN dbo.Orders o ON ol.OrderId = o.OrderId
JOIN dbo.Products p ON ol.ProductId = p.ProductId
GROUP BY p.ProductId, CONVERT(date, o.OrderDate);
GO

CREATE UNIQUE CLUSTERED INDEX IX_vProductDailySales ON dbo.vProductDailySales(ProductId, OrderDate);
```

Benefits:
- Querying the view can use the persisted index for very fast reads.
- Beware of increased storage and maintenance overhead on writes.

Limitations:
- Many restrictions on view definition (no outer joins in some cases, no TOP, no DISTINCT if indexed, etc).
- Updating base tables causes index maintenance cost.

---

## 11. Stored Procedures — Concepts & Examples

Stored procedures encapsulate logic, can modify data and schema, manage transactions, and return result sets.

Create a procedure (input and output):
```sql
CREATE PROCEDURE dbo.usp_AddCustomer
  @Name NVARCHAR(200),
  @Email NVARCHAR(255),
  @NewCustomerId INT OUTPUT
AS
BEGIN
  SET NOCOUNT ON;
  INSERT INTO dbo.Customers (Name, Email) VALUES (@Name, @Email);
  SET @NewCustomerId = SCOPE_IDENTITY(); -- safe identity retrieval
END;
GO
```

Call procedure and read OUTPUT:
```sql
DECLARE @Id INT;
EXEC dbo.usp_AddCustomer @Name = 'Alice', @Email = 'alice@example.com', @NewCustomerId = @Id OUTPUT;
SELECT @Id AS CreatedId;
```

Optional parameters (defaults):
```sql
CREATE PROCEDURE dbo.usp_GetOrders
  @CustomerId INT = NULL, -- optional
  @StartDate DATE = NULL,
  @EndDate DATE = NULL
AS
BEGIN
  SELECT * FROM dbo.Orders
  WHERE (@CustomerId IS NULL OR CustomerId = @CustomerId)
    AND (@StartDate IS NULL OR OrderDate >= @StartDate)
    AND (@EndDate IS NULL OR OrderDate < DATEADD(day,1,@EndDate));
END;
```

RETURN value:
- Procedures can return an integer via RETURN n (often used for status codes).
- Use OUTPUT parameters for richer data.

Transactions in procs:
```sql
CREATE PROCEDURE dbo.usp_TransferFunds
  @FromAccount INT, @ToAccount INT, @Amount DECIMAL(18,2)
AS
BEGIN
  SET NOCOUNT ON;
  BEGIN TRY
    BEGIN TRANSACTION;
      UPDATE dbo.Accounts SET Balance = Balance - @Amount WHERE AccountId = @FromAccount;
      UPDATE dbo.Accounts SET Balance = Balance + @Amount WHERE AccountId = @ToAccount;
    COMMIT TRANSACTION;
  END TRY
  BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    THROW; -- preserves error info
  END CATCH
END;
```

System stored procedures exist (sp_help, sp_who, sp_configure) — avoid naming custom procs with sp_ prefix (they create cross-database lookups and performance quirks).

---

## 12. User-defined Functions — Scalar & Table-valued (inline & multi-statement)

UDFs return values and can be used in SELECT, WHERE, etc. They differ from procs: functions cannot perform permanent side-effects (no DML to permanent tables) and have other restrictions.

Scalar function:
```sql
CREATE FUNCTION dbo.ufn_FormatPhone(@phone NVARCHAR(50))
RETURNS NVARCHAR(20)
AS
BEGIN
  DECLARE @digits NVARCHAR(20) = TRANSLATE(@phone, '()- .', '');
  RETURN STUFF(STUFF(@digits, 7, 0, '-'), 4, 0, '-'); -- naive formatting
END;
GO

SELECT dbo.ufn_FormatPhone(Phone) FROM dbo.Contacts;
```

Inline Table-Valued Function (recommended for performance):
```sql
CREATE FUNCTION dbo.ufn_GetRecentOrders(@CustomerId INT)
RETURNS TABLE
AS
RETURN
(
  SELECT TOP (10) OrderId, OrderDate, Total
  FROM dbo.Orders
  WHERE CustomerId = @CustomerId
  ORDER BY OrderDate DESC
);
GO

SELECT * FROM dbo.ufn_GetRecentOrders(42);
```

Multi-statement Table-Valued Function:
```sql
CREATE FUNCTION dbo.ufn_GetOrdersAggregate(@CustomerId INT)
RETURNS @Result TABLE (OrderId INT, OrderDate DATETIME, Total DECIMAL(18,2))
AS
BEGIN
  INSERT INTO @Result
  SELECT OrderId, OrderDate, Total FROM dbo.Orders WHERE CustomerId = @CustomerId;
  RETURN;
END;
GO
```

Performance notes:
- Inline TVFs are essentially parameterized views — the optimizer can inline them for good plans.
- Scalar UDFs historically executed for each row with context switch penalties — SQL Server 2019 introduced Scalar UDF Inlining to mitigate this for eligible UDFs.
- UDFs cannot modify database state (no INSERT/UPDATE/DELETE on permanent tables), cannot call stored procedures.

Usage example in queries:
```sql
SELECT o.OrderId, o.Total, dbo.ufn_FormatPhone(c.Phone) AS CustomerPhone
FROM dbo.Orders o
JOIN dbo.Customers c ON o.CustomerId = c.CustomerId;
```

---

## 13. CTE & Recursive CTE (within queries, functions and procs)

CTE (Common Table Expression) provides a readable named result set and can be recursive.

Non-recursive CTE:
```sql
WITH RecentOrders AS (
  SELECT OrderId, CustomerId, OrderDate FROM dbo.Orders WHERE OrderDate >= DATEADD(day, -30, GETDATE())
)
SELECT CustomerId, COUNT(*) AS CountOrders FROM RecentOrders GROUP BY CustomerId;
```

Recursive CTE (hierarchy traversal):
Example — organization chart:
```sql
WITH Org AS (
  SELECT EmployeeId, ManagerId, 0 AS Level, CAST(EmployeeId AS VARCHAR(MAX)) AS Path
  FROM dbo.Employees
  WHERE ManagerId IS NULL

  UNION ALL

  SELECT e.EmployeeId, e.ManagerId, o.Level + 1, o.Path + '>' + CAST(e.EmployeeId AS VARCHAR(MAX))
  FROM dbo.Employees e
  JOIN Org o ON e.ManagerId = o.EmployeeId
)
SELECT * FROM Org OPTION (MAXRECURSION 1000); -- set limit if needed
```

Recursive CTE for factorial or sequences:
```sql
WITH nums(n) AS (
  SELECT 1
  UNION ALL
  SELECT n+1 FROM nums WHERE n < 10
)
SELECT n, EXP(SUM(LOG(n)) OVER (ORDER BY n)) AS factorial_approx FROM nums
OPTION (MAXRECURSION 0);
```

Using CTE within functions and procedures:
- CTEs can be used inside stored procedures and functions (both scalar and TVFs), subject to function restrictions (e.g., inline TVF may return CTE result directly).

Notes:
- Recursive CTE default recursion depth is limited by MAXRECURSION (default 100). Use OPTION (MAXRECURSION n) or 0 for unlimited (risky).
- Recursive CTEs should have a clear termination condition.

---

## 14. Triggers — Types, Magic Tables & Examples (T‑SQL)

Triggers are special procedures invoked automatically in response to DML or DDL events.

### Magic tables: inserted and deleted
- In DML triggers, SQL Server provides two logical tables:
  - inserted: contains the new rows for INSERT and UPDATE.
  - deleted: contains the old rows for DELETE and UPDATE.

Examples:

#### AFTER (FOR) trigger example — audit INSERT/UPDATE/DELETE
```sql
CREATE TABLE dbo.CustomerAudit (
  AuditId INT IDENTITY PRIMARY KEY,
  CustomerId INT,
  AuditType NVARCHAR(10),
  ChangedAt DATETIME2 DEFAULT SYSUTCDATETIME(),
  ChangedBy SYSNAME DEFAULT SUSER_SNAME(),
  ChangedData NVARCHAR(MAX)
);

CREATE TRIGGER dbo.trg_Customers_Audit
ON dbo.Customers
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
  SET NOCOUNT ON;

  -- INSERTS
  INSERT INTO dbo.CustomerAudit (CustomerId, AuditType, ChangedData)
  SELECT i.CustomerId, 'INSERT', (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)
  FROM inserted i
  WHERE NOT EXISTS (SELECT 1 FROM deleted d WHERE d.CustomerId = i.CustomerId);

  -- UPDATES
  INSERT INTO dbo.CustomerAudit (CustomerId, AuditType, ChangedData)
  SELECT i.CustomerId, 'UPDATE',
         (SELECT d.*, i.* FOR JSON PATH)
  FROM inserted i
  JOIN deleted d ON d.CustomerId = i.CustomerId;

  -- DELETES
  INSERT INTO dbo.CustomerAudit (CustomerId, AuditType, ChangedData)
  SELECT d.CustomerId, 'DELETE', (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)
  FROM deleted d
  WHERE NOT EXISTS (SELECT 1 FROM inserted i WHERE i.CustomerId = d.CustomerId);
END;
GO
```

Notes:
- Triggers must be set-based — consider that inserted/deleted may contain multiple rows.
- Avoid RBAR (row-by-row) operations in triggers; prefer set-based logic.

#### INSTEAD OF trigger on a view — supporting updates through complex view
```sql
CREATE VIEW dbo.vCustomerBalance
AS
SELECT c.CustomerId, c.Name, SUM(o.Total) AS Balance
FROM dbo.Customers c
LEFT JOIN dbo.Orders o ON o.CustomerId = c.CustomerId
GROUP BY c.CustomerId, c.Name;
GO

CREATE TRIGGER dbo.tr_vCustomerBalance_InsteadOfUpdate
ON dbo.vCustomerBalance
INSTEAD OF UPDATE
AS
BEGIN
  SET NOCOUNT ON;
  -- Example: prevent direct updates; route to base table updates
  RAISERROR('Updates not allowed on vCustomerBalance. Update the Customers table directly.', 16, 1);
END;
GO
```

#### Cascading changes & automating updates
Example — maintain denormalized total in parent table when lines change:
```sql
CREATE TRIGGER dbo.tr_OrderLines_AfterChange
ON dbo.OrderLines
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @ChangedOrders TABLE (OrderId INT PRIMARY KEY);

  INSERT INTO @ChangedOrders (OrderId)
  SELECT DISTINCT OrderId FROM inserted
  UNION
  SELECT DISTINCT OrderId FROM deleted;

  UPDATE o
  SET Total = s.SumTotal
  FROM dbo.Orders o
  JOIN (
    SELECT ol.OrderId, SUM(ol.Quantity * ol.UnitPrice) AS SumTotal
    FROM dbo.OrderLines ol
    WHERE ol.OrderId IN (SELECT OrderId FROM @ChangedOrders)
    GROUP BY ol.OrderId
  ) s ON o.OrderId = s.OrderId;
END;
GO
```

#### DDL triggers — auditing CREATE/ALTER/DROP
Database-level DDL trigger:
```sql
CREATE TABLE dbo.SchemaChanges (
  EventData XML,
  EventTime DATETIME2 DEFAULT SYSUTCDATETIME()
);

CREATE TRIGGER trg_DDL_Changes
ON DATABASE
FOR CREATE_TABLE, ALTER_TABLE, DROP_TABLE
AS
BEGIN
  SET NOCOUNT ON;
  INSERT INTO dbo.SchemaChanges (EventData) VALUES (EVENTDATA());
END;
GO
```

Server-level DDL triggers can monitor logins and server-scoped events (use ON ALL SERVER).

### Recursion, nesting and limits

- Triggers can cause nested trigger execution: one triggered action causes another trigger to fire.
- Server configuration option: `sp_configure 'nested triggers', 1` controls nested triggers at server level (historically related to server config).
- Database option `RECURSIVE_TRIGGERS` controls whether triggers on a table can fire recursively (i.e., a trigger on table A updating table A causing the trigger to fire again).
- Set MAXRECURSION for CTEs (default 100); triggers do not have a built-in MAXRECURSION — you must design logic to prevent infinite loops.
- Always code to avoid infinite recursion (e.g., check inserted vs deleted, use CONTEXT_INFO or session context to detect and short-circuit).

### Preventing trigger recursion example:
```sql
-- Example pattern: only update totals when a flag is not set
CREATE TRIGGER dbo.tr_OrderLines_AfterChange_NoRecurse
ON dbo.OrderLines
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
  SET NOCOUNT ON;
  IF EXISTS (SELECT 1 FROM inserted) OR EXISTS (SELECT 1 FROM deleted)
  BEGIN
    -- Avoid recursion by checking a session context key
    IF (SESSION_CONTEXT(N'TriggerRunning') IS NULL)
    BEGIN
      EXEC sp_set_session_context @key = N'TriggerRunning', @value = 1;
      BEGIN TRY
        -- perform updates
      END TRY
      BEGIN FINALLY
        EXEC sp_set_session_context @key = N'TriggerRunning', @value = NULL;
      END FINALLY
    END
  END
END;
```

Notes:
- Use TRY/CATCH to ensure session context is cleared in error cases.
- Excessive trigger nesting can lead to complex, hard-to-debug behavior; prefer explicit stored proc calls for complex business logic where possible.

---

## 15. Best Practices & Performance Considerations

- Favor set‑based operations; avoid row-by-row processing inside triggers, procs, or functions.
- Use inline TVFs instead of scalar UDFs where possible (inline TVFs compose better into execution plans).
- Keep stored procedure transactions short; avoid user interaction inside a transaction.
- For auditing, consider using Change Data Capture (CDC), Change Tracking, or SQL Server Audit instead of heavy triggers for large OLTP systems.
- Use TRY/CATCH and THROW for robust error propagation and preserving stack information.
- Use SCOPE_IDENTITY() for retrieval of last identity in same scope (avoid @@IDENTITY).
- When creating indexed views: ensure the view meets all requirements and consider write penalty on base tables.
- Test performance with realistic data volumes and examine execution plans; enable SET STATISTICS IO/TIME for diagnostics.

---

## 16. Common Pitfalls & How to Avoid Them

- Assuming triggers fire per row — they are statement-level; always handle multiple rows in inserted/deleted.
- Using non-deterministic functions in indexed views (disallowed) or in UDFs you expect to be inlined.
- Using scalar UDFs in large result sets without considering UDF inlining limits (historically a performance trap).
- Applying functions to columns in WHERE clauses (non-SARGable) causing index scans.
- Heavy logic in triggers leading to long transaction durations and lock escalation.
- Using SELECT * in views or client code — schema changes can break dependent code.
- Relying on implicit conversions — use TRY_CAST/TRY_CONVERT or validate inputs.

---

## 17. Short Q&A — Key Concepts

Q: When to use a stored procedure vs a function?  
A: Use stored procedures for actions (DML/DDL, complex transactions, multiple result sets). Use functions when you need to compute a value usable in SQL expressions (scalar) or return a tabular result set (table-valued) that composes well with queries.

Q: What is the advantage of an inline TVF?  
A: The optimizer can inline the definition into the calling query, producing efficient execution plans similar to views or parameterized queries.

Q: How do you prevent infinite recursion with triggers?  
A: Design triggers to be idempotent, use session context or flags, set RECURSIVE_TRIGGERS appropriately, and avoid fire chains that update the same table without stopping condition.

Q: How to implement a materialized result in SQL Server?  
A: Use an indexed view (with SCHEMABINDING and a unique clustered index) or maintain a physical aggregate table populated by scheduled ETL or triggered updates.

---

## 18. References & Further Reading

- Microsoft Docs — T‑SQL Reference: https://learn.microsoft.com/sql/t-sql  
- CREATE VIEW (Transact-SQL) — indexed view requirements: https://learn.microsoft.com/sql/t-sql/statements/create-view-transact-sql  
- CREATE FUNCTION (Transact-SQL): https://learn.microsoft.com/sql/t-sql/functions/create-function-transact-sql  
- CREATE PROCEDURE (Transact-SQL): https://learn.microsoft.com/sql/t-sql/statements/create-procedure-transact-sql  
- CREATE TRIGGER (Transact-SQL): https://learn.microsoft.com/sql/t-sql/statements/create-trigger-transact-sql  
- GENERATE_SERIES in SQL Server (2022+) docs  
- "SQL Server Best Practices" blogs and Whitepapers (Microsoft/industry sources)

---

Prepared as a detailed T‑SQL reference for functions, subqueries, views, stored procedures and triggers. Use the examples as starting points; adapt patterns to your schema and performance needs; always test on non-production data before deploying to production.  