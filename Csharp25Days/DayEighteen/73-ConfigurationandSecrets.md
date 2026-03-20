# Configuration Sources and Secure Secrets (C# / .NET)

Purpose
- Explain common configuration sources in .NET and how they combine (precedence).
- Show secure patterns for secrets in development and production.
- Provide short, practical C# examples: IConfiguration, Options pattern, user-secrets, Key Vault, and container secrets.

Principles
- Keep configuration as data (not code): behavior by config makes apps portable.
- Secrets must never be checked into source control. Use dedicated secret stores or environment-backed secrets.
- Configuration providers are composed; the last provider wins for the same key (order matters).
- Centralize wiring in the composition root (Program.cs).

Common configuration sources (typical precedence, later overrides earlier)
1. appsettings.json (base settings)
2. appsettings.{Environment}.json (environment overrides)
3. User Secrets (development only)
4. Environment variables
5. Command-line arguments
6. Secret stores (Key Vault, AWS Secrets Manager) — treat these as override sources in production
7. In containers: Docker secrets / mounted files (read into env or file)

appsettings example
```json
// appsettings.json
{
  "Logging": { "LogLevel": { "Default": "Information" } },
  "ConnectionStrings": {
    "Default": "Server=localhost;Database=capstone;User Id=dev;Password=dev;"
  },
  "FeatureX": { "Enabled": false }
}
```

Program.cs: composing providers and binding options
```csharp
var builder = WebApplication.CreateBuilder(args);

// Base config from appsettings, then environment-specific, then env vars and command-line
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables()
    .AddCommandLine(args);

// In development, add user secrets (dotnet user-secrets tool)
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

// Bind strongly-typed options
builder.Services.Configure<MyOptions>(builder.Configuration.GetSection("MyOptions"));
```

Options pattern usage
```csharp
public class MyOptions
{
    public string ApiBaseUrl { get; set; } = "";
    public int RetryCount { get; set; } = 3;
}

public class SomeService
{
    private readonly MyOptions _opts;
    public SomeService(IOptions<MyOptions> opts) => _opts = opts.Value;
}
```

Development secrets: dotnet user-secrets
- Initialize in project:
  dotnet user-secrets init
- Set secret:
  dotnet user-secrets set "ConnectionStrings:Default" "Server=dev;User Id=...;Password=..."
- User secrets store is per-user and not checked into source control. Use for local development only.

Production secrets: Azure Key Vault (example)
- Add packages:
  dotnet add package Azure.Extensions.AspNetCore.Configuration.Secrets
  dotnet add package Azure.Identity
- Use managed identity / service principal and wire Key Vault as a configuration provider:

```csharp
using Azure.Identity;

var keyVaultUrl = builder.Configuration["KeyVault:Url"];
if (!string.IsNullOrEmpty(keyVaultUrl))
{
    builder.Configuration.AddAzureKeyVault(new Uri(keyVaultUrl), new DefaultAzureCredential());
}
```
- DefaultAzureCredential picks the best available authentication (Managed Identity in Azure, VS/Azure CLI for dev).

AWS Secrets Manager / HashiCorp Vault
- Use corresponding provider libraries or fetch secrets at startup via the SDK and add them into IConfiguration or a secure secret cache.
- Prefer provider libraries that integrate with IConfiguration so keys bind naturally.

Container secrets and Docker
- Docker secrets are mounted as files (e.g., /run/secrets/DB_PASSWORD). Read them at runtime:
```csharp
var dbPassword = File.ReadAllText("/run/secrets/DB_PASSWORD");
```
- Or map Docker secret files into environment variables in the container entrypoint and use IConfiguration.AddEnvironmentVariables().

CI/CD and orchestration
- Store pipeline secrets in the CI system (GitHub Actions secrets, Azure DevOps variable groups) and inject them into the runtime environment at deploy time.
- Do not echo or log secrets in pipeline logs.

Best practices for secrets
- Never check secrets into git.
- Use principle of least privilege: give apps only the access they need to fetch required secrets (use RBAC).
- Prefer managed identities (Azure AD, AWS IAM roles) instead of long-lived credentials.
- Rotate secrets regularly and enable auditing.
- Use Key Vault / Secrets Manager for production; user-secrets only for local dev.
- Avoid embedding secrets in container images or build artifacts.

Security around configuration data
- Treat config files as public (do not put secrets in them).
- When needed, store non-sensitive defaults in appsettings and override in secure secret stores.
- Ensure components that log configuration do not print secret values. Example:
  - Bad: _logger.LogInformation("Using connection string: {Conn}", conn);
  - Better: _logger.LogInformation("Using database {DbName} with user {User}", dbName, dbUser);

Mapping and naming considerations
- Key names with `:` map to hierarchical IConfiguration keys (e.g., ConnectionStrings:Default).
- Some secret stores disallow `:` in secret names—use naming conventions or a mapping provider to translate names, or prefix keys (ConnectionStrings--Default) and map them in code.

Rotation and caching
- Avoid fetching secrets on every request. Cache secrets securely and refresh on rotation events or with TTL.
- Many secret providers have event or polling mechanisms for rotation; consider secure local caches (encrypted if necessary).

Auditing and monitoring
- Audit accesses to the secret store (who read which secret and when).
- Monitor failed access attempts and unexpected permission changes.
- Use alerts for suspicious activity.

Operational checklist (capstone)
- Development: use appsettings.json + user-secrets + env vars.
- Staging/Production: appsettings (non-sensitive) + Key Vault / Secrets Manager + environment variables + CI-injected secrets.
- Containers: use secrets mounted as files or environment variables; do not bake secrets into images.
- CI/CD: store secrets in CI secrets vault and inject at deploy time.

Homework (short)
- Create a one-page config plan for your capstone:
  1. List 5 configuration keys and where they will be stored in production (Key Vault, env var, appsettings).
  2. Identify which keys are secrets and which are safe to store in appsettings.
  3. Describe the authentication method your app will use to access the secret store (managed identity, service principal, IAM role).
  4. Write the Program.cs lines that add the chosen secret provider.

Summary
- Use layered configuration: local files + environment + secret store, with last-wins precedence.
- Keep secrets out of source control; prefer managed secret stores and managed identities.
- Bind to options for strong typing, prevent logging secrets, and plan for rotation, auditing, and least privilege.

Further reading
- Microsoft docs: Configuration in .NET
- Azure Key Vault + DefaultAzureCredential docs
- AWS Secrets Manager best practices