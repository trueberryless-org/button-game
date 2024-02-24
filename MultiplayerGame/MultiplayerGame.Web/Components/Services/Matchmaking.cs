using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR;
using MultiplayerGame.Web.Model;
using MultiplayerGame.Web.Repositories;

namespace MultiplayerGame.Web.Components.Services;

public class Matchmaking
{
    private readonly IServiceProvider _serviceProvider;
    public List<GameController> Games { get; set; } = [];

    public Matchmaking(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Guid JoinGame(Guid playerId)
    {
        var emptyGame = Games.FirstOrDefault(t => t.PlayerOne == Guid.Empty || t.PlayerTwo == Guid.Empty);

        if (emptyGame != null)
        {
            if (emptyGame.PlayerOne == Guid.Empty)
            {
                emptyGame.PlayerOne = playerId;
            }
            else
            {
                emptyGame.PlayerTwo = playerId;
            }

            return emptyGame.GameId;
        }
        else
        {
            emptyGame = new GameController(playerId, Guid.Empty);
            Games.Add(emptyGame);
            // If no player wants to play within 10s, then you play Bot. She's cool, believe me...
            Task.Run(() => JoinBot(emptyGame));
            return emptyGame.GameId;
        }
    }
    
    private async Task JoinBot(GameController game)
    {
        await Task.Delay(5000);
        
        using (var scope = _serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<IMongoRepository<Player>>();
            
            var players = await context.ReadAllAsync();
            if (players.Find(p => p.Name == "Bot") == null)
                await context.CreateAsync(new Player() { Name = "Bot", Points = 0 });
            var botId = (await context.ReadAsync(p => p.Name == "Bot"))[0].Id;
            
            if (game.PlayersReady()) return;
            JoinGame(botId);
        }
    }

    public GameController? Game(Guid gameGuid)
    {
        return Games.FirstOrDefault(g => g.GameId == gameGuid);
    }
}