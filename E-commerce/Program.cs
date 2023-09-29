using E_commerce.Controllers;
using E_commerce.Models;
using E_commerce.Services;
using Ecommerce.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<EcommerceContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
                         new MySqlServerVersion(new Version(8, 0, 26)));
});

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<EcommerceContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;        // Require at least one digit (0-9)
    options.Password.RequiredLength = 0;         // Minimum password length
    options.Password.RequireNonAlphanumeric = false; // Require a non-alphanumeric character (e.g., @, #, $)
    options.Password.RequireUppercase = false;    // Require an uppercase letter (A-Z)
    options.Password.RequireLowercase = false;    // Require a lowercase letter (a-z)
    options.Password.RequiredUniqueChars = 0;    // Require unique characters within the password
});

builder.Services.AddScoped<ProductsController>();

builder.Services.AddScoped<OrdersController>();

builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession();

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

//Order matter
app.UseAuthentication();

app.UseAuthorization();

app.UseSession();

app.MapRazorPages();

app.Run();
