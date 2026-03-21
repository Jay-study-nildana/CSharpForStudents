// 10-RepositoryVsDbContextNotes.cs
// Purpose: Design notes summarizing when to use Repository+UnitOfWork vs direct DbContext usage,
// plus a short checklist to guide decisions and DI registration guidance.

using System;

/// <summary>
/// Summary: Repository + UnitOfWork vs Direct DbContext
///
/// When to prefer Repository + UnitOfWork:
/// - You want a clear domain/persistence boundary and easier substitution of storage implementations.
/// - You need to mock/fake persistence for unit tests without using EF in-memory provider.
/// - You want to control and centralize transaction boundaries and coordinate multiple repositories.
///
/// When direct DbContext usage is acceptable:
/// - Small projects where the team accepts coupling to EF APIs and prefers fewer abstractions.
/// - When you need direct access to advanced ORM features (raw SQL, complex LINQ mapping) and don't need portability.
///
/// Checklist (ask these before choosing):
/// 1) Do we need to swap the persistence implementation (EF -> other) or support multiple stores?
/// 2) Do we require simple unit tests without a database? (If yes, prefer Repository+UoW)
/// 3) Do our queries rely heavily on EF-specific behavior/optimizations? (If yes, direct DbContext may be pragmatic)
/// 4) Do we need centralized transaction orchestration across multiple aggregates? (If yes, use UoW)
/// 5) Will the extra abstraction complexity be maintained and understood by the team? (If no, keep it simple)
///
/// DI & lifetime recommendations:
/// - Register DbContext as Scoped (per web request) when using EF Core:
///     services.AddDbContext<AppDbContext>(options => ...); // scoped by default
/// - If wrapping DbContext in EfUnitOfWork, register IUnitOfWork as Scoped:
///     services.AddScoped<IUnitOfWork, EfUnitOfWork>();
/// - Register repository interfaces as Scoped or resolve them from IUnitOfWork to keep lifetime consistent.
///
/// Final note:
/// - Prefer pragmatic balance: small apps can use DbContext directly; larger or evolving domains benefit from repository/UoW for testability and separation.
///
/// </summary>
public static class NotesHolder { }