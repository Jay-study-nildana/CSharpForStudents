using WebAPIWithDependencyInjection.Interfaces;
using WebAPIWithDependencyInjection.POCO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Ideally, you want to use one of the two implementation. 
//Ultimatley, the goal here, of Dependency Injection,
//is the flexibility to change the implementation class
//without affecting the code within.
//use the first implementation
//builder.Services.AddScoped<ISayMyName,SayMyNameOne>();
//use the second implementation
builder.Services.AddScoped<ISayMyName, SayMyNameTwo>();

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
