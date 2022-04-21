using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using webapiauth0demo.AuthorizationScopes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c=>
    {
        //use the below stuff to add additional information to be displayed 
        //in your swagger page

        //c.SwaggerDoc(tempConfigHelperStuff.SwaggerDocname, new OpenApiInfo
        //{
        //    Version = tempConfigHelperStuff.SwaggerDocVersion,
        //    Title = tempConfigHelperStuff.SwaggerDocTitle,
        //    Description = tempConfigHelperStuff.SwaggerDocDescription,
        //    TermsOfService = new Uri(tempConfigHelperStuff.SwaggerDocTermsOfService),
        //    Contact = new OpenApiContact
        //    {
        //        Name = tempConfigHelperStuff.SwaggerDocContactName,
        //        Email = tempConfigHelperStuff.SwaggerDocContactEmail,
        //        Url = new Uri(tempConfigHelperStuff.SwaggerDocContactUrl),
        //    },
        //    License = new OpenApiLicense
        //    {
        //        Name = tempConfigHelperStuff.SwaggerDocLicenseName,
        //        Url = new Uri(tempConfigHelperStuff.SwaggerDocLicenseUrl),
        //    }
        //});

        //lets allow Swagger to put security tokens.
        //you will get a nice cool swagger box to add your bearer token
        //essential to call endpoints walled behind authorization
        var securityScheme = new OpenApiSecurityScheme()
        {
            Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT"
        };

        var securityRequirement = new OpenApiSecurityRequirement
                {
                    {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "bearerAuth"
                                }
                            },
                            new string[] {}
                    }
                };
        c.AddSecurityDefinition("bearerAuth", securityScheme);
        c.AddSecurityRequirement(securityRequirement);

    });

//at this stage, ensure that your AuthorizationScopes folder is ready with 
//the neccessary two classes
//now, with the folder and two classes ready, let's resume.

// Add Authentication Services
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    //I would strongly recommend that you double check this
    //with the Auth0 Quick start page for your web api

    options.Authority = "https://webapiauth0demo.us.auth0.com/";
    options.Audience = "https://ameeshapatelishot.com/";
});

// Register the scope authorization handler
//this is what maps the scopes in the token with the scopes in your api project
builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();


builder.Services.AddAuthorization(options =>
{
    //ensure this matches, letter by letter, in your Auth0 dashboard
    //remember that permissions dont' cascade just because you imagine
    //in your head, that it cascades
    //permission are less like a cascading waterfall
    //more like a venn diagram, a whole bunch of diagrams and circles being
    //draw individual permissions in the Auth0 dashboard.
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
});

var app = builder.Build();

//IMPORTANT : Please note that the sequence here is actually pretty important
//for example,
//UseAuthentication
//really should come before
//UseAuthorization

//I am sure there is a reason for this, but, I am not such an expert in .Net
//that I can explain it to you. Sorry.

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

//I have moved this outside the above section
//I do this because, you don't want to get a nasty surprise when you deploy this
//and there is no swagger showing up. 
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
