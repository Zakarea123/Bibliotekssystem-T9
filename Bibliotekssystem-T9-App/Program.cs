using Bibliotekssystem_T9_App.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Lägg till tjänster i containern.
builder.Services.AddControllersWithViews();

// Cookie-autentisering
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
builder.Services.AddHttpClient<UserApiService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7055");
    client.DefaultRequestHeaders.Add("X-API-KEY", "DEV-CHANGE-ME-123");
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