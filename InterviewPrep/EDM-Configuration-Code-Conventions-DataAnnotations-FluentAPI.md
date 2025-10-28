# EDM Configuration in C# — Code Conventions, Data Annotations & Fluent API  
Interview Reference Guide for Developers (Entity Framework / EF Core)

---

## Table of Contents

1. [Scope & Targets](#scope--targets)  
2. [Quick Version Note & Terminology](#quick-version-note--terminology)  
3. [Why Configure the EDM?](#why-configure-the-edm)  
4. [Conventions — “Convention over Configuration”](#conventions--convention-over-configuration)  
   - Basic convention rules  
   - Navigation / relationship discovery by convention  
   - Keys, foreign keys and shadow properties by convention  
   - Cascade delete conventions  
   - When conventions are insufficient  
5. [Data Annotations (Attributes)](#data-annotations-attributes)  
   - Common attributes and examples  
   - Limitations of Data Annotations  
   - Attribute precedence & interactions with Fluent API  
6. [Fluent API (ModelBuilder)](#fluent-api-modelbuilder)  
   - Where to configure the model (OnModelCreating, IEntityTypeConfiguration)  
   - Property configuration (types, nullability, precision, defaults)  
   - Keys, composite keys, alternate keys  
   - Indexes & uniqueness  
   - Relationships: HasOne/HasMany/WithOne/WithMany, foreign/principal keys, cascade behaviors  
   - Owned/Value Objects (OwnsOne / OwnsMany)  
   - Table and column mapping, schemas, views  
   - Concurrency tokens, rowversion/timestamp  
   - Value conversions (ValueConverter) and custom mappings  
   - Inheritance mapping (TPH, TPT, TPC)  
   - Mapping many-to-many with join entity or skip-navigations  
   - Ignoring properties, shadow properties, and backing fields  
   - Provider-specific annotations and raw SQL mapped constructs  
7. [Configuration patterns & organization](#configuration-patterns--organization)  
   - Separate configuration classes (IEntityTypeConfiguration<T>)  
   - Applying configurations from assembly  
   - Conventions and model-level configuration patterns  
8. [Common mapping scenarios & examples](#common-mapping-scenarios--examples)  
   - Simple entity example: Code conventions vs annotations vs fluent mapping  
   - Composite key example  
   - One-to-many & many-to-many examples  
   - Owned type (value object) example  
   - Column type, default value, computed column example  
   - Enum mapping example  
   - Shadow property and query filter example (soft delete)  
   - Mapping for JSON/Provider-specific types (Postgres jsonb)  
9. [Migrations and schema evolution considerations](#migrations-and-schema-evolution-considerations)  
10. [Performance & runtime considerations](#performance--runtime-considerations)  
11. [Testing, diagnostics & debugging model problems](#testing-diagnostics--debugging-model-problems)  
12. [Best practices & guidelines](#best-practices--guidelines)  
13. [Common pitfalls & anti-patterns](#common-pitfalls--anti-patterns)  
14. [Interview Q&A — Developer & Design Questions (with answers)](#interview-qa--developer--design-questions-with-answers)  
15. [Practical Exercises & Projects](#practical-exercises--projects)  
16. [References & Further Reading](#references--further-reading)

---

## 1. Scope & Targets

This guide focuses on configuring the Entity Data Model (EDM) for Entity Framework / EF Core using:

- Code conventions (the default behavior EF uses to infer the model),
- Data Annotations (attributes applied to entity classes/properties),
- Fluent API (the ModelBuilder and IEntityTypeConfiguration-based configuration API).

Examples target EF Core (3.x / 5 / 6 / 7 style APIs). Concepts apply to EF6 (classic) with some API differences — the code is oriented to EF Core.

---

## 2. Quick Version Note & Terminology

- "EDM" = Entity Data Model (conceptual model + mapping).
- "DbContext" = unit-of-work / entry point for EF Core.
- "ModelBuilder" = fluent configuration API within `OnModelCreating(ModelBuilder modelBuilder)`.
- "Conventions" = implicit rules EF uses to infer model shape from classes and properties.
- "Data Annotations" = attributes from `System.ComponentModel.DataAnnotations` (and EF-specific attributes) applied to POCOs.
- "Fluent API" = programmatic configuration using ModelBuilder.

EF Core evolves; some features (e.g., TPT/TPC mapping, `[Index]` attribute, default value behaviors, JSON mapping) were introduced or changed across versions — prefer checking provider docs for specifics.

---

## 3. Why Configure the EDM?

- Conventions are helpful but insufficient for many real-world requirements:
  - Custom column names, computed columns, column types, precision/scale.
  - Composite keys, alternate keys.
  - Complex relationships (composite FKs, many-to-many join table with payload).
  - Owned/value objects and mapping to same table or separate tables.
  - Provider-specific types (Postgres JSONB, SQL Server geography).
  - Performance tuning: indexes, query filters, compiled queries.

You can configure using Data Annotations for simple cases and Fluent API for full control. Fluent API always overrides annotations.

---

## 4. Conventions — “Convention over Configuration”

EF Core discovers the model from your classes using conventions. Understanding conventions helps you write less configuration.

Basic convention rules:
- Class -> entity: Every public class included as `DbSet<T>` or referenced by other entity types becomes an entity type.
- Property -> column: Public read/write properties are mapped as columns.
- Primary key by convention:
  - Property named `Id` or `<EntityTypeName>Id` is recognized as the primary key.
  - For example: `public int Id { get; set; }` or `public int OrderId { get; set; }`.
- Foreign keys by convention:
  - A property named `<NavigationPropertyName>Id` or `<PrincipalTypeName>Id` is recognized as a foreign key.
- Navigation discovery:
  - Reference navigation: property of another entity type -> relationship.
  - Collection navigation: `ICollection<T>` or `List<T>` -> collection navigation.
- Many-to-many:
  - If two entities contain collection navigations to each other and no join entity is declared, EF Core will create a join table (skip navigation) automatically (EF Core 5+).
- Concurrency token:
  - A property named `RowVersion` or `Timestamp` with `byte[]` type is conventionally used with `[Timestamp]`, but explicit annotation recommended.
- Column names:
  - By default, property names are column names; table name inferred from `DbSet<T>` property name or entity class name.

Shadow properties:
- EF can create properties in the model that don't have a CLR property — used for FKs or internal tracking.

Cascade delete conventions:
- EF uses cascade behavior conventions:
  - Required relationships default to Cascade (may vary by EF Core version & relational provider).
  - Optional relationships default to ClientSetNull (or Restrict) — check provider defaults.

When conventions are insufficient:
- Use Data Annotations for common tweaks (length, required, key).
- Use Fluent API for complex mapping.

---

## 5. Data Annotations (Attributes)

Data Annotations are quick and readable. They are limited to what attributes expose.

Common attributes and usage:

```csharp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Product
{
    [Key] // optional if named Id
    public int ProductId { get; set; }

    [Required] // non-nullable
    [MaxLength(100)]
    public string Name { get; set; }

    [Column("unit_price", TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [Timestamp] // concurrency token / rowversion (SQL Server)
    public byte[] RowVersion { get; set; }

    [NotMapped] // ignored property
    public string TempComputed { get; set; }

    [ForeignKey("Category")] // specify FK property name
    public int CategoryForeignKey { get; set; }

    public Category Category { get; set; } // navigation
}
```

Useful attributes:
- [Key], [Required], [StringLength], [MaxLength], [MinLength]
- [Column], [Table], [NotMapped], [ForeignKey], [InverseProperty]
- [ConcurrencyCheck], [Timestamp]
- [DatabaseGenerated(DatabaseGeneratedOption.Identity | Computed | None)]
- [Owned] (EF Core-specific for Owned types in some versions or use Fluent API)
- [Index] — introduced in later EF Core versions as an attribute to apply an index (check your EF Core version)

Limitations of Data Annotations:
- Not all mappings are supported (e.g., composite keys require Fluent API).
- More complex relationship configuration (alternate keys, split tables) often needs Fluent API.
- Less expressive for provider-specific features (HasComputedColumnSql, HasConversion, HasDefaultValueSql).
- Annotations are applied directly on the class; not suitable when you cannot modify the POCO (e.g., generated classes). For those, use Fluent API or mapping classes.

Attribute precedence:
- EF respects both. Fluent API wins: configuration applied in `OnModelCreating` overrides DataAnnotations.

---

## 6. Fluent API (ModelBuilder)

Fluent API is the full configuration surface. Put configuration in `OnModelCreating` in your `DbContext`, or better: in separate configuration classes that implement `IEntityTypeConfiguration<TEntity>`.

Where:
```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyContext).Assembly);
    // or
    modelBuilder.Entity<Product>(entity =>
    {
        entity.HasKey(e => e.ProductId);
        entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
        entity.HasOne(e => e.Category).WithMany(c => c.Products).HasForeignKey(e => e.CategoryForeignKey);
    });
}
```

### Property configuration
Common API surface:

```csharp
entity.Property(e => e.Price)
      .HasColumnName("unit_price")
      .HasColumnType("decimal(18,2)")
      .IsRequired()
      .HasDefaultValue(0m)
      .HasDefaultValueSql("GETDATE()") // provider-specific
      .ValueGeneratedOnAdd() // for identity-like behavior
      .ValueGeneratedOnAddOrUpdate() // for computed columns
      .IsConcurrencyToken(); // treat as concurrency token
```

- `IsRequired()` vs C# nullable types: For reference types EF infers nullability from C# nullable annotations (nullable reference types) or the presence of `[Required]`. For value types, `T?` => optional.
- `HasMaxLength(int)` sets string/array length.
- `HasPrecision(precision, scale)` for decimal in EF Core 6+ (or `HasColumnType("decimal(18,2)")` as fallback).
- `HasDefaultValue` vs `HasDefaultValueSql` – use SQL for computed defaults.

### Keys, composite keys, alternate keys
- Primary key:
  ```csharp
  entity.HasKey(e => e.Id);
  ```
- Composite key:
  ```csharp
  entity.HasKey(e => new { e.OrderId, e.LineNumber });
  ```
- Alternate key (unique constraint):
  ```csharp
  entity.HasAlternateKey(e => e.Sku); // creates a unique constraint
  ```

### Indexes & uniqueness
```csharp
entity.HasIndex(e => e.Email).IsUnique();
entity.HasIndex(e => new { e.LastName, e.FirstName }).HasDatabaseName("IX_Person_Name");
```
In EF Core 5+ you may also use `[Index]` attribute.

### Relationships
Fluent API expresses relationships explicitly:

- One-to-many:
  ```csharp
  modelBuilder.Entity<Order>()
      .HasOne(o => o.Customer)         // principal
      .WithMany(c => c.Orders)         // collection
      .HasForeignKey(o => o.CustomerId)
      .OnDelete(DeleteBehavior.Cascade); // cascade rule
  ```

- One-to-one:
  ```csharp
  modelBuilder.Entity<User>()
      .HasOne(u => u.Profile)
      .WithOne(p => p.User)
      .HasForeignKey<UserProfile>(p => p.UserId);
  ```

- Many-to-many (skip navigation):
  ```csharp
  modelBuilder.Entity<Student>()
      .HasMany(s => s.Courses)
      .WithMany(c => c.Students)
      .UsingEntity<Dictionary<string, object>>(
          "Enrollments", // join table name
          j => j.HasOne<Course>().WithMany().HasForeignKey("CourseId"),
          j => j.HasOne<Student>().WithMany().HasForeignKey("StudentId"));
  ```

- Many-to-many with payload (explicit join entity):
  ```csharp
  modelBuilder.Entity<Enrollment>()
      .HasKey(e => new { e.StudentId, e.CourseId });

  modelBuilder.Entity<Enrollment>()
      .HasOne(e => e.Student).WithMany(s => s.Enrollments).HasForeignKey(e => e.StudentId);

  modelBuilder.Entity<Enrollment>()
      .HasOne(e => e.Course).WithMany(c => c.Enrollments).HasForeignKey(e => e.CourseId);
  ```

FK and principal key:
- `HasForeignKey` sets the FK property (on dependent).
- `HasPrincipalKey` sets which property on principal is referenced (not necessarily the PK).

Cascade behaviors:
- `OnDelete(DeleteBehavior.Cascade | Restrict | SetNull | NoAction | ClientSetNull)`.
- Provider defaults and convention choices may differ; configure explicitly where semantics matter.

### Owned / Value Objects
Owned types model value objects stored in the same table or their own table depending on configuration:
```csharp
modelBuilder.Entity<Order>().OwnsOne(o => o.ShippingAddress, a =>
{
    a.Property(p => p.Street).HasColumnName("ShippingStreet");
    a.Property(p => p.Zip).HasColumnName("ShippingZip");
});
```
For collections:
```csharp
modelBuilder.Entity<Customer>().OwnsMany(c => c.Addresses);
```
Owned types do not have identity independent of owner by default. You can configure the owned entity's table with `.ToTable("...")` to map to a separate table.

### Table / Column mapping, schemas, views
```csharp
entity.ToTable("orders", "sales"); // table name and schema
entity.Property(e => e.Name).HasColumnName("customer_name");
modelBuilder.Entity<MyView>().ToView("vwSummary");
```

### Concurrency tokens and rowversion
```csharp
entity.Property(e => e.RowVersion)
      .IsRowVersion()
      .IsConcurrencyToken();
```
For SQL Server, `IsRowVersion()` maps to `rowversion` and is updated by the database.

### Value conversions
Map CLR types to provider types with `HasConversion`:
```csharp
entity.Property(e => e.Status)
      .HasConversion<string>(); // enum to string
// Custom converter
entity.Property(e => e.Metadata)
      .HasConversion(
          v => JsonSerializer.Serialize(v, options),
          v => JsonSerializer.Deserialize<Dictionary<string,string>>(v, options))
      .HasColumnType("jsonb"); // Postgres example
```

### Inheritance mapping (TPH/TPT/TPC)
- TPH (Table-per-hierarchy) — default: single table with discriminator
  ```csharp
  modelBuilder.Entity<Base>().HasDiscriminator<string>("Discriminator")
      .HasValue<DerivedA>("A")
      .HasValue<DerivedB>("B");
  ```
- TPT (Table-per-type) — map each type to separate table (EF Core supports TPT with `ToTable` on derived types).
  ```csharp
  modelBuilder.Entity<Derived>().ToTable("DerivedTable");
  ```
- TPC (Table-per-concrete type) — supported in later EF Core versions; check provider/version support.

### Ignoring properties, shadow properties & backing fields
- Ignore property:
  ```csharp
  modelBuilder.Entity<Product>().Ignore(p => p.TempComputed);
  ```
- Shadow property:
  ```csharp
  modelBuilder.Entity<Product>().Property<int>("CreatedBy");
  ```
  Access via `entry.Property("CreatedBy").CurrentValue` — useful for auditing or when you can't modify CLR type.
- Backing field:
  ```csharp
  modelBuilder.Entity<Product>()
      .Property<int>("_stock")
      .HasField("_stock")
      .UsePropertyAccessMode(PropertyAccessMode.Field);
  ```

### Provider-specific annotations, raw SQL & computed columns
- Computed column:
  ```csharp
  entity.Property(e => e.FullName)
        .HasComputedColumnSql("[FirstName] + ' ' + [LastName]", stored: false);
  ```
- Raw SQL for default values:
  ```csharp
  entity.Property(e => e.CreatedAt).HasDefaultValueSql("NOW()"); // postgres
  ```
- Use `.HasAnnotation("SqlServer:FillFactor", 90)` for provider-specific metadata.

---

## 7. Configuration patterns & organization

Good patterns help manage complexity:

- Use `IEntityTypeConfiguration<T>`:

```csharp
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
         builder.HasKey(p => p.ProductId);
         builder.Property(p => p.Name).IsRequired().HasMaxLength(200);
         // ...
    }
}
```

- Then apply:
```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyContext).Assembly);
}
```

- Keep Fluent API and DataAnnotations separate: use annotations for simple constraints; move complex mapping to configuration classes to keep entity POCO clean.

- Use extension methods for shared patterns:
```csharp
public static class ModelBuilderExtensions
{
    public static void ApplyAuditProperties(this ModelBuilder modelBuilder)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(IAuditable).IsAssignableFrom(entity.ClrType))
            {
                modelBuilder.Entity(entity.ClrType)
                    .Property(nameof(IAuditable.CreatedAt))
                    .HasDefaultValueSql("GETDATE()");
            }
        }
    }
}
```

- Centralize naming conventions or snake_case mapping in a single place.

---

## 8. Common mapping scenarios & examples

### Simple entity — convention vs annotation vs fluent

Conventions (no config):
```csharp
public class Category
{
    public int Id { get; set; }         // PK by convention
    public string Name { get; set; }    // column by convention
    public ICollection<Product> Products { get; set; }
}
```

Data Annotation:
```csharp
[Table("product")]
public class Product
{
    [Key]
    public int ProductId { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; }

    [Column("unit_price", TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
}
```

Fluent:
```csharp
modelBuilder.Entity<Product>(builder =>
{
    builder.ToTable("product");
    builder.HasKey(p => p.ProductId);
    builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
    builder.Property(p => p.Price).HasColumnName("unit_price").HasColumnType("decimal(18,2)");
});
```

### Composite key
```csharp
public class OrderLine
{
  public int OrderId { get; set; }
  public int LineNumber { get; set; }
  public int ProductId { get; set; }
  public int Quantity { get; set; }
}
modelBuilder.Entity<OrderLine>().HasKey(ol => new { ol.OrderId, ol.LineNumber });
```

### Owned type (value object)
```csharp
public class Address { public string Street {get;set;} public string City {get;set;} }
public class Customer { public int Id {get;set;} public Address BillingAddress {get;set;} }

modelBuilder.Entity<Customer>().OwnsOne(c => c.BillingAddress, a =>
{
    a.Property(p => p.Street).HasColumnName("BillingStreet");
    a.Property(p => p.City).HasColumnName("BillingCity");
});
```

### Enum mapping
```csharp
public enum OrderStatus { New = 0, Processing = 1, Shipped = 2 }

modelBuilder.Entity<Order>().Property(o => o.Status).HasConversion<string>(); // store as text
// or
modelBuilder.Entity<Order>().Property(o => o.Status).HasConversion<int>(); // store as int (default)
```

### Soft delete using shadow property + global query filter
```csharp
modelBuilder.Entity<MyEntity>().Property<bool>("IsDeleted");
modelBuilder.Entity<MyEntity>().HasQueryFilter(e => EF.Property<bool>(e, "IsDeleted") == false);
```

### Mapping JSON (Postgres jsonb) using value conversion
```csharp
entity.Property(e => e.Metadata)
      .HasColumnType("jsonb")
      .HasConversion(
         v => JsonSerializer.Serialize(v, null),
         v => JsonSerializer.Deserialize<Dictionary<string,string>>(v, null));
```

### Computed column & default value
```csharp
entity.Property(e => e.Total).HasComputedColumnSql("[Price] * [Quantity]");
entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
```

---

## 9. Migrations and schema evolution considerations

- Model changes translate to migrations via `dotnet ef migrations add`.
- Migrations depend on provider. For provider-specific column types or SQL, verify generated SQL using `Script-Migration` or `dotnet ef migrations script`.
- Renaming columns/tables requires `RenameColumn` or manual migration operations to avoid data loss.
- Composite key changes are disruptive — plan schema updates carefully.
- Rolling out non-null columns: when adding a non-nullable column to an existing table, provide a default or perform multi-step migration (add nullable, populate data, make not-nullable).
- Use `HasDefaultValueSql` for provider-side defaults and avoid setting defaults only in client code.

---

## 10. Performance & runtime considerations

- Database indexes are key for performance - create them on FK columns and frequently searched columns.
- Avoid eager loading everything — use projections and `Include` only when needed.
- Materialize results (`ToList`) only when necessary. Repeated enumeration causes multiple DB calls if query not materialized.
- Avoid N+1: use `Include` or projection to fetch related data in single query.
- Prefer server-side evaluation in IQueryable queries — pushing work to DB can be faster for large sets.
- Value conversion and computed columns may affect query translation and indexes — test generated SQL.
- Global query filters (e.g., soft delete) are applied in translation — verify indexes accommodate filtered queries.

---

## 11. Testing, diagnostics & debugging model problems

- Inspect model metadata:
  ```csharp
  var model = context.Model;
  foreach (var entityType in model.GetEntityTypes())
  {
      Console.WriteLine(entityType.Name);
      foreach (var property in entityType.GetProperties())
          Console.WriteLine($"  {property.Name} -> {property.GetColumnName()}");
  }
  ```
- Use `DbContext.Database.GetDbConnection()` and SQL logging to see executed SQL.
- EF Core: enable detailed errors and sensitive data logging in development:
  ```csharp
  optionsBuilder.EnableSensitiveDataLogging().EnableDetailedErrors();
  ```
- For EF Core & EF: `ToQueryString()` on queryable to inspect SQL (EF Core 5+).
- Validate migrations and scaffolded SQL before deploying.
- Unit test mapping: create in-memory SQLite with schema created from migrations and run basic CRUD tests to verify mappings.

---

## 12. Best practices & guidelines

- Prefer Fluent API for complex mapping; use Data Annotations for simple, obvious constraints.
- Keep domain entities POCO and free from persistence-specific concerns where possible (use Fluent API in separate classes).
- Always consider nullability: leverage C# nullable reference types together with EF Core nullability inference.
- Use `IEntityTypeConfiguration<T>` to separate concerns and keep `DbContext` clean.
- Document model assumptions (e.g., cascade rules, uniqueness constraints) in code or README.
- Add indexes for search and FK columns; review query plans for heavy queries.
- Prefer compile-time safe entity types over `dynamic` or reflection for mapping logic.
- Use migration testing environments to validate schema changes.

---

## 13. Common pitfalls & anti-patterns

- Relying on conventions for critical constraints (e.g., expecting PK by some name that differs).
- Changing key properties frequently — keys should be stable.
- Mapping huge, complex object graphs without projections — causes heavy queries/over-fetching.
- Using `ToList()` too early causing memory pressure.
- Using stringly-typed queries or concatenated SQL — use parameters or `HasDefaultValueSql`.
- Forgetting to configure indexes on columns used for filtering and joins.
- Not specifying delete behavior — default cascades might delete more than intended.
- Mixing attributes and fluent API carelessly and not verifying final model.

---

## 14. Interview Q&A — Developer & Design Questions (with answers)

Q1: What are the three ways to configure the EF Core model?  
A: Conventions (automatic), Data Annotations (attributes in class), Fluent API (ModelBuilder / IEntityTypeConfiguration). Fluent API overrides Data Annotations.

Q2: When would you prefer Fluent API over Data Annotations?  
A: For complex mappings (composite keys, owned types, alternate keys), when POCOs are generated or read-only, to keep domain classes clean, or when configuration requires runtime logic.

Q3: How does EF determine the primary key by convention?  
A: EF looks for a property named `Id` or `<TypeName>Id`. If none found, key must be configured via Data Annotation `[Key]` or Fluent API `HasKey(...)`.

Q4: How do you map value objects (owned types)?  
A: Use `OwnsOne`/`OwnsMany` in Fluent API to map the owned type properties into the owner’s table (or map owned to separate table). Owned types don’t have independent identity by default.

Q5: How to configure a composite key?  
A: Using Fluent API: `builder.HasKey(e => new { e.Part1, e.Part2 });` — composite keys are not supported via Data Annotations.

Q6: How do EF Core global query filters work and when to use them?  
A: Use `HasQueryFilter(...)` on entity type to apply a filter to all queries (e.g., soft-delete). They are applied at translation time and can reference EF.Property for shadow properties. Be careful with query filters and `Include`.

Q7: What’s the difference between `HasDefaultValue` and `HasDefaultValueSql`?  
A: `HasDefaultValue` sets a constant default value used in migrations; `HasDefaultValueSql` sets SQL expression executed by DB (e.g., `GETDATE()`, `now()`).

Q8: How do you map enums?  
A: Default store as integers. Use `HasConversion<string>()` to store string names. Choose based on readability and expected future changes.

Q9: How does EF support many-to-many relationships?  
A: EF Core 5+ supports skip navigations (implicit join table). For join entity with payload, define explicit join entity and configure relationships. Use `.UsingEntity` for customizing join table.

Q10: What is a shadow property? When is it useful?  
A: A model property not present on the CLR type. Useful for FK properties you don’t want in the domain class or for audit fields you prefer to keep out of the domain.

Q11: How do you control cascade delete behavior?  
A: Configure with `.OnDelete(DeleteBehavior.Cascade | Restrict | SetNull | NoAction | ClientSetNull)`. Conventions may vary — explicitly define in model where semantics are important.

Q12: How to debug a translation error when LINQ to Entities fails?  
A: Inspect the expression used (e.g., the predicate), check `ToQueryString()` to see SQL, and avoid client-side methods that are not translatable. Rewrite predicate using supported constructs or use AsEnumerable()/ToList() to force client evaluation where appropriate (but beware performance).

Q13: How to prevent accidental data loss when renaming columns in migrations?  
A: Use `RenameColumn` migration operation, or generate manual migration that uses SQL to rename rather than drop/create. Verify migration script before applying to production.

---

## 15. Practical Exercises & Projects

1. Starter:
   - Create a model with `Customer`, `Order`, `OrderLine`, and map relationships using conventions; then add DataAnnotations for simple constraints.
   - Run migrations and verify schema.

2. Intermediate:
   - Replace DataAnnotations with Fluent API in `IEntityTypeConfiguration` classes. Add composite key for `OrderLine`. Add index on `Order.OrderDate`.
   - Implement Owned type `Address` for `Customer` and map its fields to the same table.

3. Advanced:
   - Implement soft-delete via shadow property and global query filter. Implement a service that can query including soft-deleted items.
   - Map a many-to-many where the join entity has additional columns (e.g., `Enrollment` with `EnrolledAt`), then write queries to fetch students and courses with enrollment metadata.
   - Create a migration that renames a column safely (manual migration), and write tests that validate no data loss.

4. Performance:
   - Benchmark a query that causes N+1; fix it with `Include` or projection into DTOs. Use `ToQueryString()` to inspect SQL before and after.
   - Test a JSON `Dictionary<string,string>` property mapped to Postgres `jsonb` with `HasConversion` and run queries using `EF.Functions.JsonContains` (provider-specific).

---

## 16. References & Further Reading

- EF Core Docs — Model building & configuration: https://learn.microsoft.com/ef/core/modeling/  
- EF Core Docs — Data annotations: https://learn.microsoft.com/ef/core/modeling/entity-properties?tabs=data-annotations  
- EF Core Docs — Fluent API: https://learn.microsoft.com/ef/core/modeling/  
- EF Core Docs — Migrations: https://learn.microsoft.com/ef/core/managing-schemas/migrations/  
- "Entity Framework Core in Action" — books/articles (for deeper reading)  
- Provider docs (SQL Server, PostgreSQL/Npgsql, MySQL) for provider-specific mappings and SQL functions

---

Prepared as a comprehensive reference for configuring the EDM in EF Core using code conventions, Data Annotations and the Fluent API. Use this guide for interview preparation, code reviews, or as a checklist when designing or refactoring persistence models.
