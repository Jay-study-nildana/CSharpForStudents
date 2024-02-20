using MVCWebApp.Service;
using MVCWebApp.Service.IService;
using MVCWebApp.Utility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//needed for making and consuming API stuff
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddHttpClient<IComicBookService, ComicBookService>();

//get the coupon api URL
SD.ComicBookAPIBase = builder.Configuration["ServiceUrls:ComicBookAPI"];

//add the base service. this base service will be the main service for making any type of HTTP calls
//think of it as a Postman for our project. 
builder.Services.AddScoped<IBaseService, BaseService>();
//uses the base service to talk to the coupon api
builder.Services.AddScoped<IComicBookService, ComicBookService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
