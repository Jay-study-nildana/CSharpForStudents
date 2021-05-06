# JavaScript Auth0 Demo App

This is a quick and easy javascript single page application SPA. 

# auth_config.json file

1. rename the 'auth_config.json.sample' to 'auth_config.json'
1. Update it with your domain and clientId and scope and audience values.
1. Update this part in app.js to decide what kind of token you want. 
    ```
    auth0 = await createAuth0Client({
        domain: config.domain,
        client_id: config.clientId,
        audience: config.audience,
        scope: config.scope
    });
    ```
1. Update the many buttons and the button linked functions as per your API endpoint.

# Original Source

1. https://github.com/auth0-samples/auth0-javascript-samples

# References

1. Check QuickStart in your Auth0 dashboard.
1. https://auth0.com/docs/quickstart/spa/vanillajs/02-calling-an-api

# Hire Me

I work as a full time freelance software developer and coding tutor. Hire me at [UpWork](https://www.upwork.com/fl/vijayasimhabr) or [Fiverr](https://www.fiverr.com/jay_codeguy). 

# important note 

This code is provided as is without any warranties. It's primarily meant for my own personal use, and to make it easy for me share code with my students. Feel free to use this code as it pleases you.

I can be reached through my website - [Jay's Developer Profile](https://jay-study-nildana.github.io/developerprofile)