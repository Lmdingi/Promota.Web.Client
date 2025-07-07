using Client;
using Client.Components;
using Client.GlobalEventCallBacks;
using Client.Security;
using Microsoft.AspNetCore.Components.Authorization;
using Serilog;
using Services;
using Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/client.txt")
    .MinimumLevel.Information()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// services
builder.Services.AddScoped<ICookieService, CookieService>();
builder.Services.AddScoped<IAccessTokenService, AccessTokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();
builder.Services.AddScoped<IAPIService, APIService>(); 
builder.Services.AddScoped<IResourceService, ResourceService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddSingleton<GlobalEventCallBacks>();

builder.Services.AddHttpClient("ApiClient", options =>
{
    options.BaseAddress = new Uri("https://localhost:7041/api/");
});

builder.Services.AddAuthorization();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("EmailVerifiedOnly", policy =>
        policy.RequireClaim("email_verified", "true"));
});

builder.Services.AddAuthentication()
    .AddScheme<CustomOption, JWTAuthenticationHandler>("JWTAuth", options =>
    {

    });

builder.Services.AddScoped<JWTAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider, JWTAuthenticationStateProvider>();

builder.Services.AddCascadingAuthenticationState();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
