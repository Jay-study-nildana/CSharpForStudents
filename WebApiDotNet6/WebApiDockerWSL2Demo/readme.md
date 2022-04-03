# Basic Web Api Docker WSL2 Demo

a simple code that does a simple docker deployed to azure web app.

It has detailed notes, please go through them carefully.

# Detailed Notes

[Excerpt from my work diary. it appears dirty but its a lot of steps]

Okay, installed Docker. restarted computer.

I am on Windows Home. So, I should use 'Docker Linux', NOT Windows.

https://localhost:49153/swagger/index.html

it works fine from visual studio directly. Both Debug and Release.

Note : Remember to do the following, as you do with any, dot net core app. If you dont do this, swagger wont be visible on the deployed app.

```

  // Configure the HTTP request pipeline.
  //if (app.Environment.IsDevelopment())
  //{

  //}

  app.UseSwagger();
  app.UseSwaggerUI();

```

it will look something like this when you run directly from docker. (for this, you must do release build first). But, because of HTTPS, it wont run outside of visual studio. You can run it, but you will keep getting SSL related errors.

https://stackoverflow.com/questions/43708578/how-to-run-docker-image-produced-by-vs-2017

https://127.0.0.1:49166/swagger/index.html

https://127.0.0.1:1234/swagger/index.html

You can set the port number from the Docker Engine GUI.

Or, in command line.

```

  docker run -d -p 1234:80 --name some_name Your.App:latest

```

Your.App

this is the app as listed in docker.

:latest

this is the tag.

Now, you try to push it from Docker GUI, it wont work.

https://stackoverflow.com/questions/43858398/docker-push-error-denied-requested-access-to-the-resource-is-denied

Create a repository on your online docker hub first.

Something like this, will show up. jaydaakarru/dotnecore

Next, tag it.

Then, finally, push it with the tag.

```

  docker image list
  docker login -u jaydaakarru
  [now enter password]
  [Login Succeeded]
  docker tag webapidockerwsl2demo:latest jaydaakarru/dotnetcoreb:firstimagepush
  docker push jaydaakarru/dotnetcoreb:firstimagepush
  The push refers to repository [docker.io/jaydaakarru/dotnetcoreb]
  0097e1b6d252: Pushed
  5f70bf18a086: Pushed
  37252fa6e7df: Pushed
  17aff088b762: Pushed
  9a515fdf7f03: Pushed
  c4d9ca739af5: Pushed
  3f94255da7c2: Pushed
  608f3a074261: Pushed
  firstimagepush: digest: sha256:eb72ff460aea382c7a2795bc3bf2f118910ba0accb3bbc8004ca0e34fe9a0c35 size: 1995

```

Now, deploy it into azure web app

https://docs.microsoft.com/en-us/learn/modules/deploy-run-container-app-service/5-exercise-deploy-web-app?pivots=csharp

https://docs.microsoft.com/en-us/azure/container-registry/

https://azure.github.io/AppService/2020/10/15/Docker-Hub-authenticated-pulls-on-App-Service.html

Here, all you have to do is

1. select Linux Web App
1. select Docker Hub (there are other container options as well)
1. Put the Public name tag of your docker hub. for example, from the above push we did, jaydaakarru/dotnetcoreb . Azure will pull the container during deployment, no problem.
1. You can also use private docker hub by logging in, no problem.

The first time you load the website, it will take a few seconds to load. The docker image is unpacking and stuff. So, wait for a few minutes.

Head over to the deployed site, https://yourazurewebapp.net/swagger/index.html and boom, you are golden.

# References

1. https://stackoverflow.com/questions/43708578/how-to-run-docker-image-produced-by-vs-2017
1. https://stackoverflow.com/questions/43858398/docker-push-error-denied-requested-access-to-the-resource-is-denied
1. https://docs.microsoft.com/en-us/learn/modules/deploy-run-container-app-service/5-exercise-deploy-web-app?pivots=csharp
1. https://docs.microsoft.com/en-us/azure/container-registry/
1. https://azure.github.io/AppService/2020/10/15/Docker-Hub-authenticated-pulls-on-App-Service.html

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
