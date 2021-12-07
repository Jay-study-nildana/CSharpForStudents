# EnableCORS

CORS allow you to decide which 'outside' the API server running location apps can talk to your server. 

Check Startup.cs for exact code usage.

# Part One - ConfigureServices

    services.AddCors(options =>
    {
        options.AddDefaultPolicy(builder =>
        {
            builder.WithOrigins(Configuration["CorsOriginLocalHost"],
                                Configuration["CorsOriginStaging"],
                                Configuration["CorsOriginProduction"]);
            builder.AllowAnyMethod();
            builder.AllowAnyHeader();
            builder.AllowCredentials();
        });
    });

# Part Two - Configure

    app.UseCors();

# Hire Me

I work as a full time freelance software developer and coding tutor. Hire me at [UpWork](https://www.upwork.com/fl/vijayasimhabr) or [Fiverr](https://www.fiverr.com/jay_codeguy). 

# important note 

This code is provided as is without any warranties. It's primarily meant for my own personal use, and to make it easy for me share code with my students. Feel free to use this code as it pleases you.

I can be reached through my website - [Jay's Developer Profile](https://jay-study-nildana.github.io/developerprofile)