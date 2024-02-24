using MultiplayerGame.Web;
using MultiplayerGame.Web.Components;
using MultiplayerGame.Web.Components.Services;
using MultiplayerGame.Web.Model;
using MultiplayerGame.Web.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddOutputCache();

builder.Services.AddHttpClient<WeatherApiClient>(client => client.BaseAddress = new("http://apiservice"));

builder.AddMongoDBClient("mongodb");

builder.Services.AddScoped<IMongoRepository<Player>, MongoRepository<Player>>();

builder.Services.AddSingleton<Matchmaking>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();

app.UseAntiforgery();

app.UseOutputCache();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();

app.Run();