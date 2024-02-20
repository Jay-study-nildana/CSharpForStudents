using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebAPI.Config;
using WebAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//injecting the SQlite stuff

string SQLiteConnectionString = builder.Configuration.GetConnectionString("DefaultSqliteConnection");

//if you are getting database connection errors, for example,
//

builder.Services.AddDbContext<AppDBContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultSqliteConnection"));
});

//including the automapper via dependency injection
IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
