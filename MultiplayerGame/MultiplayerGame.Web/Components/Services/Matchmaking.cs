using Microsoft.AspNetCore.SignalR;

namespace MultiplayerGame.Web.Components.Services;

public class Matchmaking
{
    public List<GameController> Games { get; set; } = [];

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
            return emptyGame.GameId;
        }
    }

    public GameController? Game(Guid gameGuid)
    {
        return Games.FirstOrDefault(g => g.GameId == gameGuid);
    }
}