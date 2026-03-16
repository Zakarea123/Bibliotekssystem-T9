using Bibliotekssystem_T9_App.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Lägg till tjänster i containern.
builder.Services.AddControllersWithViews();


// Registera HttpClient för LoanService
builder.Services.AddHttpClient("LoanService", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiUrls:LoanService"]!);
});

// Registera LoanApiService för dependency injection.
builder.Services.AddScoped<LoanApiService>();

//Registrera HttpClient för CatalogService
builder.Services.AddHttpClient("CatalogService", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiUrls:CatalogService"]!);
});

builder.Services.AddScoped<CatalogApiService>();

// Cookie-Autentisering
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        options.SlidingExpiration = true;
    });

//Registrera HttpClient i MVC
var userServiceBaseUrl = builder.Configuration["UserService:BaseUrl"];
var userServiceApiKey = builder.Configuration["UserService:ApiKey"];

builder.Services.AddHttpClient<UserApiService>(client =>
{
    client.BaseAddress = new Uri(userServiceBaseUrl!);
    client.DefaultRequestHeaders.Add("X-API-KEY", userServiceApiKey!);
});

var app = builder.Build();

// Konfigurera HTTP-begäran pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Account}/{action=Login}/{id?}")
    .WithStaticAssets();

app.Run();