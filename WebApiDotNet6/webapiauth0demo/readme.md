# webapiauth0demo

A quick and simple demo for web api with Auth0 plus scopes and roles.

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

# Hire Me

I work as a full time freelance software developer and coding tutor. Hire me at [UpWork](https://www.upwork.com/fl/vijayasimhabr) or [Fiverr](https://www.fiverr.com/jay_codeguy).

# Hobbies

I try to maintain a few hobbies.

1. Podcasting. You can listen to my [podcast here](https://stories.thechalakas.com/listen-to-podcast/).
1. Photography. You can see my photography on [Unsplash here](https://unsplash.com/@jay_neeruhaaku).
1. Digital Photorealism 3D Art and Arch Viz. You can see my work on this on [Adobe Behance](https://www.behance.net/vijayasimhabr).
1. Writing and Blogging. You can read my blogs. I have many medium Publications. [Read them here](https://medium.com/@vijayasimhabr).

# important note

This code is provided as is without any warranties. It's primarily meant for my own personal use, and to make it easy for me share code with my students. Feel free to use this code as it pleases you.

I can be reached through my website - [Jay's Developer Profile](https://jay-study-nildana.github.io/developerprofile)
