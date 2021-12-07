# Rest Sharp Usage in Dot Net 6

Quick demo that consumes an API server. 

Some Details

1. For the purpose of running this demo, I am using one of my own projects, the [Random Stuff Generator](https://jay-study-nildana.github.io/RandomStuffDocs/). However, the code should work with any API server of your choice and with any OAuth2 authentication system.
1. Unfortunately, there is no way to automatically collect a token to use in all the authenticated calls that we are making in this code. [More details here](CollectingToken.md).
1. If you want test the API server directly, you can copy paste the token to the "Authorize" box. Just the token, and nothing else or it wont work. 

# Projects

This is a solution which feeds off many class libraries.

1. [AllTheGeneralHelpers](AllTheGeneralHelpers) - Contains helpers.
1. [AllTheInterfaces](AllTheInterfaces) - Contains interfaces.
1. [APIConsumerHelper](APIConsumerHelper) - The final API response returning stuff.
1. [RandomStuffGenerator](RandomStuffGenerator) - JSON POCOS and Converters.
1. [RestSharpDotNet6](RestSharpDotNet6) - Driver Program. For testing and seeing how the other projects are used.

# Server and Client

1. API Server - https://randomstuffapizeropoint4.azurewebsites.net/index.html
1. Client App - https://randomstuffreactjsappzeropoint4.azurewebsites.net/
1. API Server Code - https://jay-study-nildana.github.io/RandomStuffDocs/APIServer/
1. Client App Code - https://jay-study-nildana.github.io/RandomStuffDocs/ReactJSApp/

# References

1. https://jay-study-nildana.github.io/RandomStuffDocs/
1. https://restsharp.dev/

# Hire Me

I work as a full time freelance software developer and coding tutor. Hire me at [UpWork](https://www.upwork.com/fl/vijayasimhabr) or [Fiverr](https://www.fiverr.com/jay_codeguy). 

# important note 

This code is provided as is without any warranties. It's primarily meant for my own personal use, and to make it easy for me share code with my students. Feel free to use this code as it pleases you.

I can be reached through my website - [Jay's Developer Profile](https://jay-study-nildana.github.io/developerprofile)
