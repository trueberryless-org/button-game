using System.ComponentModel.DataAnnotations;

namespace MultiplayerGame.Web.Model;

public class Player
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Points { get; set; }
}