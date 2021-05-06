# Token Testing

Before coming here, make sure you have completed [SequenceOfSteps.md](SequenceOfSteps.md).

1. On Auth0 dashboard, you have a 'Test' section where you can get testing client_id and client_secret.
1. Use those two things to get a test token to test against your API server.
1. Use the following code to get your test token. 
    ```
        curl --request POST \
        --url https://csharpforstudents.us.auth0.com/oauth/token \
        --header 'content-type: application/json' \
        --data '{"client_id":"PUTYOURIDHERE","client_secret":"PUTYOURCLIENTSECRETHERE","audience":"PUTYOURAUDIENCEHERE","grant_type":"client_credentials"}'
    ```
1. Now, you can call your API like this.
    ```
    curl --request GET \
    --url http://path_to_your_api/ \
    --header 'authorization: Bearer INSERTYOURTOKEN'
    ```
1. Please note that you can curl yourself. Or, you can just copy the token that is generated for you by the 'Test' section.
1. Now, use Postman to test the endpoints. I have included a Postman Collection in the project - [TempCSharpForStudents.postman_collection.json](TempCSharpForStudents.postman_collection.json)

# References

1. https://stackoverflow.com/questions/65793225/postman-error-unable-to-verify-the-first-certificate-when-try-to-get-from-my

# Hire Me

I work as a full time freelance software developer and coding tutor. Hire me at [UpWork](https://www.upwork.com/fl/vijayasimhabr) or [Fiverr](https://www.fiverr.com/jay_codeguy). 

# important note 

This code is provided as is without any warranties. It's primarily meant for my own personal use, and to make it easy for me share code with my students. Feel free to use this code as it pleases you.

I can be reached through my website - [Jay's Developer Profile](https://jay-study-nildana.github.io/developerprofile)