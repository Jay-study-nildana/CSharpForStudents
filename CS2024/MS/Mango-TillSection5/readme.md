# Mango Till Section 5

This project has code that is mostly written by me and covers the code till Section 5 Auth. Till Chapter 68, Roles Demo of the C# Course from Udemy, "C# 10 | Ultimate Guide - Beginner to Advanced | Master class" 

The following topics are covered in this example project.

1. Introduction to Micro Service Architecture
1. .Net MVC Web App that consumes Authentication and 3rd Party API
1. .Net Identity Auth API with OAuth, JWT Tokesn and Roles
1. .Net API CRUD with SQLite, Entity Framework and Authentication and Authorization
1. .Net API with Swagger UI with Auth Token
1. .Net API that connects with 3rd Party API

note: a improved version is available here, [Comic Book Shop](https://github.com/Jay-study-nildana/comicbookshop)

# Projects

1. [Mango](Mango) - An empty project, just for the sake of running and making sure your computer works fine. 
1. Three Project MicroServices That work with each other.
    1. [Mango.Web](Mango.Web) - the web app built using razor pages
    1. [Mango.Services.AuthAPI](Mango.Services.AuthAPI) - authenticaion API server
    1. [Mango.Services.CouponAPI](Mango.Services.CouponAPI) - the main back end API which is the busines logic/business idea

# Coding Challenge

1. Adding Coupon and Deleting Coupon UI is working in the Web App. However, I have left the 'Edit' UI for you to solve.
    1. The 'Edit'/'PUT'/Update endpoint is working. So, you only need to focus on building the UI
1. Right now, the project works with SQLite. Try switching it to some other database that supports EF, like Microsoft SQL Server.    

# running on visual studio (windows)

1. Tested on Visual Studio 2022 Community Edition.
1. Create an empty project (or a console project) in visual studio, and add all three projects mentioned, Mango.Web, Mango.Services.AuthAPI, Mango.Services.CouponAPI 
1. AuthAPI and CouponAPI project is already configured with EF Core with SQLite, and the .db is already included. If not, run the usual EF Core migrations and create your own local SQlite file.
1. Set 'Multiple Startup Projects' for all 3 projects, and run all three.
1. Create a user account with any role of your choice with the web app
1. Login with the account you created, and play around with the CRUD features under, 'Content Management' > Coupon.
1. Play around with the AuthAPI Swagger or Postman to see the auth features
1. Play around with the CouponAPI Swagger or Postman to see the auth features

# running on VS Code (windows/mac)

1. Tested on Windows with VS Code. Running.
1. Tested on Mac with VS Code. Running.
1. Open each project folder, one by one, and run them separately. 
    1. https://medium.com/projectwt/running-visual-studio-windows-projects-on-vs-code-windows-mac-9a7068defed0
1. Create a user account with any role of your choice with the web app
1. Login with the account you created, and play around with the CRUD features under, 'Content Management' > Coupon.
1. Play around with the AuthAPI Swagger or Postman to see the auth features
1. Play around with the CouponAPI Swagger or Postman to see the auth features

# book a session with me

1. https://calendly.com/jaycodingtutor/30min

# hire and get to know me

find ways to hire me, follow me and stay in touch with me.

1. https://github.com/Jay-study-nildana
1. https://thechalakas.com
1. https://www.upwork.com/fl/vijayasimhabr
1. https://www.fiverr.com/jay_codeguy
1. https://www.codementor.io/@vijayasimhabr
1. https://stackoverflow.com/users/5338888/jay