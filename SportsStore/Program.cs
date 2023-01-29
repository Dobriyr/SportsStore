using SportsStore.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<StoreDbContext>(opts => {
    opts.UseSqlServer(
        builder.Configuration["ConnectionStrings:SportsStoreConnection"]);
    });
builder.Services.AddScoped<IStoreRepository, EFStoreRepository>(); //AddScoped - Each request gets own repository

var app = builder.Build();
//app.MapGet("/", () => "Hello World!");

app.UseStaticFiles();
app.MapControllerRoute("pagination", "Products/Page{productPage}", new { Controller = "Home", action = "Index" });
app.MapDefaultControllerRoute();

SeedData.EnsurePopulated(app);
app.Run();
