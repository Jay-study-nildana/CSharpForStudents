# ASP.NET — Introduction, Stylesheets & Master Pages, Database-Driven Web Pages, Migration from Web Forms, ASP.NET Core Razor Pages & Blazor  
Interview Reference Guide for Developers

---

## Table of Contents

1. [Scope & Purpose](#scope--purpose)  
2. [Quick History & Versions (ASP.NET → ASP.NET Core → Blazor)](#quick-history--versions-aspnet--aspnet-core--blazor)  
3. [Web Fundamentals & ASP.NET Hosting Models](#web-fundamentals--aspnet-hosting-models)  
4. [Project Types: Web Forms, MVC, Web API, Razor Pages, Blazor](#project-types-web-forms-mvc-web-api-razor-pages-blazor)  
5. [Working with Stylesheets & Layouts (Master Pages → Layouts)](#working-with-stylesheets--layouts-master-pages--layouts)  
   - Linking stylesheets, bundling/miniﬁcation, and modern tooling  
   - Layouts, Partial Views, View Components, Tag Helpers  
6. [Razor Syntax & View Engine](#razor-syntax--view-engine)  
7. [Database-Driven Web Pages: Data Access Patterns](#database-driven-web-pages-data-access-patterns)  
   - ADO.NET, Dapper, EF Core: examples & when to use each  
   - Connection strings, configuration, migrations, seeding  
   - Data binding to UI (Razor Pages, MVC) and validation  
8. [Transitioning from ASP.NET Web Forms to ASP.NET Core](#transitioning-from-aspnet-web-forms-to-aspnet-core)  
   - Concept mapping: controls → Tag Helpers / Components, ViewState removal, page lifecycle differences  
   - Typical migration strategies and incremental approaches  
9. [Introduction to ASP.NET Core Razor Pages](#introduction-to-aspnet-core-razor-pages)  
   - PageModel, routing, handlers, model binding, validation, file uploads  
   - Example: simple CRUD Razor Page with EF Core  
10. [Blazor — Server & WebAssembly](#blazor--server--webassembly)  
    - Blazor Server vs Blazor WebAssembly (WASM): hosting, latency, resource model, security implications  
    - Component model, lifecycle methods, dependency injection, routing, forms & validation, JS interop, authentication/authorization patterns  
    - State management approaches (Scoped services, local storage, flux-like patterns)  
11. [Building REST APIs & Integrating with Front-Ends](#building-rest-apis--integrating-with-front-ends)  
    - ASP.NET Core Web API, routing, controllers, minimal APIs, versioning, swagger/OpenAPI  
12. [Real-time with SignalR](#real-time-with-signalr)  
13. [Security & Authentication/Authorization](#security--authenticationauthorization)  
    - Cookies, JWT, OAuth2/OIDC, ASP.NET Core Identity, external providers, data protection, XSRF/CSRF mitigation, CORS  
14. [Middleware, Routing, Dependency Injection & Logging](#middleware-routing-dependency-injection--logging)  
15. [Testing, Debugging & Observability](#testing-debugging--observability)  
16. [Deployment & Azure Integration](#deployment--azure-integration)  
    - Publishing options, containerization, App Service, Azure Static Web Apps (WASM), Managed Databases and services  
17. [Performance & Scalability Considerations](#performance--scalability-considerations)  
18. [Best Practices & Architecture Guidelines](#best-practices--architecture-guidelines)  
19. [Common Mistakes & Anti-Patterns](#common-mistakes--anti-patterns)  
20. [Comprehensive Q&A — Developer & Interview Questions (with answers)](#comprehensive-qa--developer--interview-questions-with-answers)  
21. [Practical Exercises & Projects](#practical-exercises--projects)  
22. [Cheat Sheet & Useful Commands / Snippets](#cheat-sheet--useful-commands--snippets)  
23. [References & Further Reading](#references--further-reading)

---

## 1. Scope & Purpose

This guide walks through ASP.NET family technologies: working with styles and layouts (master pages → layouts), building database-driven web pages using EF Core / Dapper / ADO.NET, migrating from legacy Web Forms to modern ASP.NET Core, and building modern UI with Razor Pages and Blazor (Server & WASM). It aims to be an interview and practical engineering reference.

---

## 2. Quick History & Versions (ASP.NET → ASP.NET Core → Blazor)

- ASP.NET Web Forms (2002): event-driven, ViewState, server controls — fast for form-based apps but heavy.
- ASP.NET MVC (2009): clear separation (Model-View-Controller), more control over HTML.
- ASP.NET Web API: RESTful services.
- ASP.NET Core (2016+): cross-platform, modular, high-performance redesign; unified MVC, Razor Pages, Web API.
- Blazor (2019+): client UI using C# — two hosting models: Blazor Server and Blazor WebAssembly.

---

## 3. Web Fundamentals & ASP.NET Hosting Models

- Traditional IIS hosting (in-process/out-of-process) vs cross-platform Kestrel server.
- ASP.NET Core app typically runs Kestrel behind reverse proxy (IIS, Nginx, Apache) in production.
- Request pipeline: HTTP → Server (Kestrel) → Middleware → Endpoint (Razor Page/Controller/Component).

---

## 4. Project Types: Web Forms, MVC, Web API, Razor Pages, Blazor

- Web Forms: page-centric; PostBack; ViewState (legacy).
- MVC: controller-based; views; suitable for complex apps requiring separation of concerns.
- Razor Pages: page-centric but lightweight; PageModel pairs with .cshtml pages — great for simple CRUD pages.
- Web API / Minimal APIs: API endpoints for SPAs/mobile.
- Blazor:
  - Server: components executed on server, UI diffs over SignalR.
  - WASM: runs entirely in browser on WebAssembly.

Choose based on team skills, app needs (SEO, interactivity, offline), hosting model.

---

## 5. Working with Stylesheets & Layouts (Master Pages → Layouts)

Master Pages (Web Forms) → Layouts & Partial Views (MVC/Razor Pages) mapping:
- Web Forms MasterPage (.master) provides placeholders; Razor uses _Layout.cshtml with `@RenderBody()` and `@RenderSection()`.

Linking style sheets:
```html
<!-- _Layout.cshtml -->
<!doctype html>
<html lang="en">
<head>
  <meta charset="utf-8" />
  <meta name="viewport" content="width=device-width, initial-scale=1" />
  <title>@ViewData["Title"] - MyApp</title>
  <link rel="stylesheet" href="~/css/site.css" asp-append-version="true" />
  <environment include="Development">
    <link rel="stylesheet" href="~/lib/bootstrap/dist/css/bootstrap.css" />
  </environment>
  @RenderSection("Styles", required: false)
</head>
<body>
  @RenderBody()
  <script src="~/js/site.js" asp-append-version="true"></script>
  @RenderSection("Scripts", required: false)
</body>
</html>
```

Modern tooling:
- Bundling & minification: built-in static file pipeline + third-party tools. For ASP.NET Core use:
  - Build-time tools: Webpack, Vite, esbuild (recommended), Parcel.
  - Runtime: `asp-append-version` adds cache-busting query string based on file hash.
  - Use `dotnet watch` for development.

Partial Views & View Components:
- Partial: reusable markup (`_LoginPartial.cshtml`).
- View Components: reusable server-side rendering with logic (like mini-controller + view).

Tag Helpers:
- Replace HTML helpers with more readable markup: `<a asp-controller="Home" asp-action="Index">Home</a>`.

---

## 6. Razor Syntax & View Engine

Razor mixes C# with HTML using `@`. Example:
```csharp
@model IEnumerable<Product>
@{
    ViewData["Title"] = "Products";
}
<h1>@ViewData["Title"]</h1>
<ul>
@foreach(var p in Model) {
  <li>@p.Name - @p.Price.ToString("C")</li>
}
</ul>
```
- `@functions` deprecated in favor of PageModel/Controller.
- Use Tag Helpers for form inputs: `<input asp-for="Name" class="form-control" />`
- Anti-forgery token: `@Html.AntiForgeryToken()` or `<form asp-antiforgery="true">`.

---

## 7. Database-Driven Web Pages: Data Access Patterns

Options:
- ADO.NET: low-level, best for high control and performance-sensitive queries.
- Dapper: micro-ORM, lightweight, fast; maps query results to POCOs.
- EF Core: full ORM with change tracking, LINQ queries, migrations.

EF Core basic example (Razor Page or Controller):

DbContext:
```csharp
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
    public DbSet<Product> Products { get; set; }
}
```

Register in Program.cs (ASP.NET Core 6+ minimal host):
```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
```

Razor Page for list:
```csharp
public class IndexModel : PageModel
{
    private readonly AppDbContext _db;
    public IList<Product> Products { get; set; }
    public IndexModel(AppDbContext db) => _db = db;
    public async Task OnGetAsync() => Products = await _db.Products.AsNoTracking().ToListAsync();
}
```

Migrations:
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

Validation:
- Use Data Annotations (`[Required]`, `[Range]`) and client-side unobtrusive validation.

When to use which:
- Use EF Core for rapid development and maintainability.
- Use Dapper for complex/custom queries with high performance needs.
- Use raw ADO.NET for maximum performance or to avoid ORM overhead.

---

## 8. Transitioning from ASP.NET Web Forms to ASP.NET Core

Key differences:
- No ViewState: state must be managed explicitly (hidden fields, session, client-side, or DI-scoped services).
- Page lifecycle vs simplified Razor Page lifecycle: Web Forms had many events (Init, Load, PreRender) — Razor Pages/Controllers are request/response only.
- Server controls → Tag Helpers / Components: server controls generated HTML and maintained state; modern approach uses components (Blazor) or helper libraries.
- PostBack replaced by normal HTTP verbs (GET/POST) and Ajax/fetch calls for partial updates.
- Use progressive migration:
  - Start by building new features in ASP.NET Core and keep legacy Web Forms app running.
  - Extract services and business logic into class libraries shared by both apps.
  - Introduce APIs (Web API) and have the Web Forms front-end call them, enabling gradual porting.

Migration checklist:
- Separate business logic from UI.
- Centralize data access / DI container for reuse.
- Replace server controls with partial pages, Tag Helpers or Blazor components.
- Re-implement authentication/authorization using ASP.NET Core Identity or JWT/OIDC.

---

## 9. Introduction to ASP.NET Core Razor Pages

Razor Pages are page-centric and ideal for CRUD workflows.

File structure:
- Pages/Products/Index.cshtml
- Pages/Products/Index.cshtml.cs (PageModel)

PageModel example:
```csharp
public class CreateModel : PageModel
{
    private readonly AppDbContext _db;
    public CreateModel(AppDbContext db) => _db = db;

    [BindProperty]
    public Product Product { get; set; }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        _db.Products.Add(Product);
        await _db.SaveChangesAsync();
        return RedirectToPage("./Index");
    }
}
```

Routing:
- Page locations map to routes (Pages/Products/Index → /Products).

Handlers:
- OnGet, OnPost, OnPostDeleteAsync, etc.
- Handler methods support parameters via model binding.

File upload:
- Bind `IFormFile` via `[BindProperty]` and save to storage.

Security:
- Use `[Authorize]` on PageModel or per handler.

---

## 10. Blazor — Server & WebAssembly

Blazor basics:
- Components are `.razor` files mixing markup and C#.
- Example:
```razor
@inject HttpClient Http
<h3>Weather</h3>
@if (forecasts == null) { <p>Loading...</p> }
else {
  <ul>
    @foreach(var f in forecasts) { <li>@f.Date: @f.TemperatureC &deg;C</li> }
  </ul>
}
@code {
  private WeatherForecast[] forecasts;
  protected override async Task OnInitializedAsync() {
    forecasts = await Http.GetFromJsonAsync<WeatherForecast[]>("WeatherForecast");
  }
}
```

Blazor Server:
- App executes on server; UI diffs sent over SignalR. Pros: small download size, full .NET on server. Cons: latency, server memory per connection, need sticky sessions in scale-out.

Blazor WebAssembly (WASM):
- App runs in browser on WebAssembly; can be standalone or hosted with ASP.NET Core backend. Pros: runs offline, no server memory per user. Cons: larger initial download, restricted by browser sandbox.

Component lifecycle:
- `OnInitialized[Async]`, `OnParametersSet[Async]`, `OnAfterRender[Async]`, `Dispose`.
- Use `StateHasChanged()` to force re-render.

Forms & validation:
- `<EditForm Model="@model" OnValidSubmit="HandleValid">`
- Use `DataAnnotationsValidator` and custom validators.

JS Interop:
- `IJSRuntime` and `IJSObjectReference` for calling JS from C# and vice versa.

Authentication:
- Blazor Server uses server-side auth (cookies); Blazor WASM typically uses token-based (OIDC/OAuth2): authenticate via ID provider, store token (prefer authorization code + PKCE) in secure storage.

State management:
- Use scoped services for server-side per-connection state.
- For WASM use local storage or Circuit handlers; consider flux or libraries when app grows.

Best practices:
- Avoid long-running synchronous work in Blazor Server to keep responsiveness.
- For Blazor WASM, lazy-load assemblies, use caching, and pre-render critical UI where needed.

---

## 11. Building REST APIs & Integrating with Front-Ends

ASP.NET Core Web API:
- Controllers: `[ApiController]` attribute provides automatic model validation and binding semantics.
- Minimal APIs (ASP.NET Core 6+): compact route-first approach.

Example minimal API:
```csharp
var app = builder.Build();
app.MapGet("/products", async (AppDbContext db) => await db.Products.ToListAsync());
app.MapPost("/products", async (Product p, AppDbContext db) => { db.Products.Add(p); await db.SaveChangesAsync(); return Results.Created($"/products/{p.Id}", p); });
```

OpenAPI/Swagger:
- `builder.Services.AddEndpointsApiExplorer(); builder.Services.AddSwaggerGen();` then `app.UseSwagger(); app.UseSwaggerUI();`.

Versioning:
- Use Microsoft.AspNetCore.Mvc.Versioning for API versioning strategies.

---

## 12. Real-time with SignalR

SignalR provides real-time messaging with automatic fallback transports.

Server Hub:
```csharp
public class ChatHub : Hub
{
    public async Task SendMessage(string user, string message) {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}
```
Client:
- JavaScript: `const connection = new signalR.HubConnectionBuilder().withUrl("/chathub").build(); connection.on("ReceiveMessage", (u,m)=>{ ... }); await connection.start(); connection.invoke("SendMessage", user, msg);`
- Blazor integrates SignalR natively for Blazor Server; also can use HubConnection in Blazor WASM.

Use cases: notifications, collaborative apps, dashboards.

---

## 13. Security & Authentication/Authorization

Authentication:
- Cookie authentication for server apps.
- JWT and OAuth2/OIDC for APIs and SPAs.
- ASP.NET Core Identity for user management (passwords, roles, claims).
- Use external providers (Azure AD, Google, Microsoft Account) via OIDC.

Authorization:
- Policies, roles, claims-based authorization:
  ```csharp
  services.AddAuthorization(options => {
      options.AddPolicy("AdminOnly", p => p.RequireRole("Admin"));
  });
  ```
- Use `[Authorize]` and `[AllowAnonymous]`.

Anti-forgery:
- Use `ValidateAntiForgeryToken` or `@Html.AntiForgeryToken()` for POST forms; for APIs use tokens and CORS properly.

Data protection:
- `IDataProtectionProvider` for protecting cookies and tokens; configure key storage for multiple servers.

Input validation:
- Always validate and sanitize inputs; use parameterized queries to prevent SQL injection.

---

## 14. Middleware, Routing, Dependency Injection & Logging

Middleware:
- Central concept in ASP.NET Core: executed in pipeline order. Example:
```csharp
app.Use(async (context, next) => {
  var sw = Stopwatch.StartNew();
  await next();
  sw.Stop();
  context.Response.Headers["X-Elapsed"] = sw.ElapsedMilliseconds.ToString();
});
```

Routing:
- Endpoint routing maps requests to controllers/pages/components.
- Attribute routing: `[HttpGet("products/{id}")]`.

Dependency Injection:
- Built-in DI container supports Scoped, Transient, Singleton lifetimes.
  - Transient: new instance each resolve.
  - Scoped: per-request in web apps (per connection in Blazor Server).
  - Singleton: shared across app lifetime.

Logging:
- Built-in providers: Console, Debug, EventSource. Integrate Serilog, NLog for structured logging.
- Use structured logging: `_logger.LogInformation("Created {Id} for user {UserId}", id, userId);`

---

## 15. Testing, Debugging & Observability

Testing:
- Unit tests: xUnit/NUnit/MSTest for services. Mock dependencies with Moq.
- Integration tests: `WebApplicationFactory<TEntryPoint>` for end-to-end tests of controllers and pages.
- Blazor testing: bUnit for component unit tests.

Debugging:
- `dotnet watch` for live reload.
- Diagnostics: `dotnet-trace`, `dotnet-counters`, and Application Insights for telemetry.

Health checks:
- `Microsoft.Extensions.Diagnostics.HealthChecks` and health endpoints for readiness/liveness probes.

---

## 16. Deployment & Azure Integration

Publishing:
- `dotnet publish -c Release` creates artifacts. Deploy to:
  - Azure App Service (Windows/Linux), Docker containers, Azure Kubernetes Service (AKS).
  - IIS: publish and configure hosting.
  - Static Web Apps: Blazor WASM + API backend.

Azure specifics:
- App Service: easy deployment with continuous deployment (GitHub Actions).
- Azure Static Web Apps: ideal for Blazor WASM with serverless APIs.
- Azure Functions as backend microservices.
- Azure SQL Database: managed DB; configure firewall rules, connection string in Key Vault.
- Use `Azure App Configuration` or `Key Vault` for secrets and configuration values.

CI/CD:
- Use GitHub Actions or Azure DevOps pipelines for build/test/publish steps.

---

## 17. Performance & Scalability Considerations

- Use response caching and output caching (`ResponseCache` middleware).
- Use distributed caching (Redis) for state across instances.
- Optimize DB queries and use EF Core `.AsNoTracking()` for read-only ops where change tracking not needed.
- For Blazor Server, minimize per-connection memory usage; scale out with sticky sessions (or use Redis-backed CircuitStore).
- Use HTTP/2 and compression, CDN for static assets.
- Use gzip/ Brotli compression via middleware.

---

## 18. Best Practices & Architecture Guidelines

- Keep controllers/pages thin — move business logic to services.
- Prefer asynchronous I/O (`async`/`await`) for scalable throughput.
- Use DTOs for API responses and avoid leaking EF entities directly.
- Centralize configuration and secrets; do not check secrets into source control.
- Automate tests and security scans in CI.
- Document API via OpenAPI/Swagger.

---

## 19. Common Mistakes & Anti-Patterns

- Relying on synchronous DB calls in web request threads.
- Storing large amounts of state in memory on server (especially per-user).
- Mixing concerns: placing data access in controllers or Razor pages.
- Exposing exception details in production responses.
- Overusing sessions or ViewState-like patterns (when migrating from Web Forms).
- Forgetting to validate and sanitize user input.

---

## 20. Comprehensive Q&A — Developer & Interview Questions (with answers)

Q1: What is the difference between Razor Pages and MVC?  
A: Razor Pages are page-focused and ideal for page-based scenarios — each page has a PageModel. MVC separates concerns with Controllers and Views — more flexible for complex apps.

Q2: How does Blazor Server differ from Blazor WebAssembly?  
A: Blazor Server runs components on the server and transmits UI diffs via SignalR; Blazor WASM runs client-side in the browser via WebAssembly.

Q3: How do you prevent CSRF in ASP.NET Core?  
A: Use anti-forgery tokens (ValidateAntiForgeryToken) for form posts; for APIs use tokens (Bearer) and strict CORS policies.

Q4: How to share code between server and client in Blazor?  
A: Create shared class library projects referenced by both server and client (models, validation attributes).

Q5: When migrating from Web Forms, how to handle ViewState-dependent logic?  
A: Refactor stateful logic to services, use persisting stores (DB, Cache), or client-side state (localStorage) and rework pages to be request/response based.

Q6: How is dependency injection configured in ASP.NET Core?  
A: In `Program.cs` via `builder.Services.AddScoped`, `AddSingleton`, and `AddTransient`. Constructor injection is used to receive dependencies.

Q7: What are Tag Helpers and why use them?  
A: Tag Helpers are server-side components that generate HTML with cleaner markup (e.g., `<form asp-action="Create">`) and enable compile-time checks and intellisense.

Q8: How to implement file uploads in Razor Pages?  
A: Bind `IFormFile` via `[BindProperty]` and write file stream to disk or blob storage with proper validation and size limits.

Q9: How to secure secrets in Azure deployments?  
A: Use Azure Key Vault and Managed Identities to access secrets securely; avoid embedding secrets in appsettings.json.

Q10: What are View Components?  
A: Reusable server-side components with encapsulated rendering logic — similar to partials but with richer logic and caching options.

---

## 21. Practical Exercises & Projects

1. Beginner:
   - Build a simple Razor Pages CRUD app with EF Core (Products). Include validation, paging, and sorting.
   - Implement layout and partial views, and add responsive CSS.

2. Intermediate:
   - Create a Blazor WebAssembly app that consumes an ASP.NET Core Web API (hosted) and performs authentication via OIDC.
   - Implement SignalR chat integrated with Blazor Server or JS client.

3. Advanced:
   - Migrate a small Web Forms module to Razor Pages incrementally: extract business logic to a shared library, implement APIs, and replace UI pages one-by-one.
   - Build a multi-tenant app with per-tenant configuration loaded at runtime and tested with unit/integration tests.

4. Testing:
   - Add unit tests for services, integration tests for pages/controllers, and component tests for Blazor with bUnit.

---

## 22. Cheat Sheet & Useful Commands / Snippets

Create new solutions & projects:
```bash
dotnet new sln -n MyApp
dotnet new webapp -o MyApp.Web   # Razor Pages
dotnet new mvc -o MyApp.Mvc     # MVC
dotnet new blazorserver -o MyApp.BlazorServer
dotnet new blazorwasm -o MyApp.BlazorWasm
```

Run project:
```bash
dotnet watch run
```

EF Core migrations:
```bash
dotnet ef migrations add AddProducts
dotnet ef database update
```

Publish:
```bash
dotnet publish -c Release -o ./publish
```

Add services in Program.cs:
```csharp
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(conn));
```

Add middleware:
```csharp
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllers();
app.MapBlazorHub();
```

Common Tag Helpers:
```html
<form asp-page="/Products/Create" method="post" asp-antiforgery="true">
  <input asp-for="Product.Name" />
  <span asp-validation-for="Product.Name"></span>
  <button type="submit">Save</button>
</form>
```

---

## 23. References & Further Reading

- Official docs:
  - ASP.NET Core — https://learn.microsoft.com/aspnet/core
  - Blazor — https://learn.microsoft.com/aspnet/core/blazor
  - Entity Framework Core — https://learn.microsoft.com/ef/core
  - SignalR — https://learn.microsoft.com/aspnet/core/signalr
- Books & guides:
  - "Pro ASP.NET Core" series — Apress
  - "Blazor in Action"
  - Microsoft patterns & practices
- Community:
  - GitHub samples: ASP.NET Core repo, Blazor samples
  - ASP.NET community standups, blogs, and conference talks

---

Prepared as an interview-focused and practical development reference for ASP.NET, Razor Pages, Blazor and migration from legacy Web Forms. Use the sections as a checklist for learning, architecture discussions, and hands-on coding.  