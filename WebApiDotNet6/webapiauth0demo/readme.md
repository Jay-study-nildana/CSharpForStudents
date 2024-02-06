# webapiauth0demo

A quick and simple demo for web api with Auth0 plus scopes and roles.

TODO: updated to the latest dot net version. also test with the latest version of the auth0 server. 

# Things to change

Replace these with your own values.

```

    options.Authority = "https://webapiauth0demo.us.auth0.com/";
    options.Audience = "https://ameeshapatelishot.com/";

```

Here is where you can put your own roles and permissions.

```

    options.AddPolicy("RoleThatOnlyReads", policy =>
    {
        policy.Requirements.Add(new HasScopeRequirement("read:stuff", "https://webapiauth0demo.us.auth0.com/"));
    });
    //policy related to Moderator Role
    options.AddPolicy("RoleThatDeletesUpdates", policy =>
    {
        policy.Requirements.Add(new HasScopeRequirement("read:stuff", "https://webapiauth0demo.us.auth0.com/"));
        policy.Requirements.Add(new HasScopeRequirement("write:stuff", "https://webapiauth0demo.us.auth0.com/"));
        policy.Requirements.Add(new HasScopeRequirement("delete:stuff", "https://webapiauth0demo.us.auth0.com/"));
    });


```

# related blog posts

1. https://medium.com/projectwt/configuring-auth0-sample-with-scopes-april-2022-update-4f990f893656 - Use this link to get a quick and simple Auth0 React JS sample working. You will need this to collect the token to test any web api built using this code.

# hire and get to know me

find ways to hire me, follow me and stay in touch with me.

https://jay-study-nildana.github.io/developerprofile/