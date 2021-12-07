using DotNet6APIEFCoreSQLite.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var tempOtherStuff = new OtherStuff();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<KaijuDBContext>(options =>
        options.UseSqlite("Data Source=kaijusman.db"));

var app = builder.Build();

IConfiguration configuration = app.Configuration;

//services.AddDbContext<SchoolContext>(options =>
//    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
//options.UseSqlite(Configuration["SqliteConnectionString"]));
//adding db context
//"Data Source=kaijusman.db"
//builder.Services.AddDbContext<KaijuDBContext>(options =>
//        options.UseSqlite(configuration["SqliteConnectionString"]));

//create or check for db and perform initiliation of seed data
tempOtherStuff.CreateDbIfNotExists(app);


// Configure the HTTP request pipeline.
//I have commented the following development only swagger UI.
//this is because, I want the Swagger to show up even after deployment.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
