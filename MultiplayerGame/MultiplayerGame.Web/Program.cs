using MultiplayerGame.Web;
using MultiplayerGame.Web.Components;
using MultiplayerGame.Web.Components.Services;
using MultiplayerGame.Web.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddOutputCache();

builder.Services.AddHttpClient<WeatherApiClient>(client => client.BaseAddress = new("http://apiservice"));

builder.Services.AddSingleton<Matchmaking>();
builder.Services.AddSingleton<MatchmakingChat>();

builder.AddMongoDBClient("mongodb");

builder.Services.AddScoped<IMongoRepository<Player>, MongoRepository<Player>>();

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