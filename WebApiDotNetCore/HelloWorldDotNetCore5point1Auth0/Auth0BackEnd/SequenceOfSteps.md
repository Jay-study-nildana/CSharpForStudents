# Sequence Of Steps

Making OAuth2 work is not a task that is taken lightly. It has a lot of moving parts and everything has to be configured in a specific way. 

I have outlined the steps, in detail, below.

1. Start by creating an account in Auth0.com. My suggestion to use your existing GitHub account. 
1. Create a tenant. Pick a region. I usually pick USA.
1. Create API. Pick Identifier. Pick Signing Algorithm.
1. Identifier is recommended to be a URL. I own many domains. i just use any of them. If you dont own a domain, just put whatever you want. For example, somethingsomething.com
1. Add some permissions. At least one is required.
1. Now, update appsettings.json.
    ```json
        "Auth0": {
            "Domain": "{DOMAIN}",
            "Audience": "{API_IDENTIFIER}"
        }
    ```
1. The values must match what you used earlier when creating the tenant.
1. Now, add the neccessary library.
    ```
    Install-Package Microsoft.AspNetCore.Authentication.JwtBearer --version 5.0.5
    OR
    dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 5.0.5
    ```
1. This package is needed in order to validate Access Tokens.
1. Now, Configure the middleware in ConfigureServices
    ```
    // Startup.cs

    public void ConfigureServices(IServiceCollection services)
    {
        // Some code omitted for brevity...

        string domain = $"https://{Configuration["Auth0:Domain"]}/";
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = domain;
                options.Audience = Configuration["Auth0:Audience"];
            });
    }
    ```    
1. Now, complete the middleware configuration in Configure
    ```
    app.UseAuthentication();
    app.UseAuthorization();
    ```
1. Now, we need to create some classes for handling policy/scope/authorization. For this, we use 'Policy-based authorization'
1. Check the classes HasScopeRequirement and HasScopeHandler for more details.
1. Now, include the Policy in ConfigureServices
    ```
        services.AddAuthorization(options =>
        {
            options.AddPolicy("read:messages", policy => policy.Requirements.Add(new HasScopeRequirement("read:messages", domain)));
        });

        services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
    ```
1. Now, go and check if the authentication is working. More details at Auth0TestController.
1. Please check [TokenTestingAPI.md](TokenTestingAPI.md) after you have configured your API and test it.

# References

1. https://auth0.com/docs/quickstart/backend/aspnet-core-webapi
1. https://github.com/auth0-samples/auth0-aspnetcore-webapi-samples/tree/master/Quickstart/01-Authorization
1. https://docs.microsoft.com/en-us/aspnet/core/security/authorization/policies?view=aspnetcore-5.0

# Hire Me

I work as a full time freelance software developer and coding tutor. Hire me at [UpWork](https://www.upwork.com/fl/vijayasimhabr) or [Fiverr](https://www.fiverr.com/jay_codeguy). 

# important note 

This code is provided as is without any warranties. It's primarily meant for my own personal use, and to make it easy for me share code with my students. Feel free to use this code as it pleases you.

I can be reached through my website - [Jay's Developer Profile](https://jay-study-nildana.github.io/developerprofile)