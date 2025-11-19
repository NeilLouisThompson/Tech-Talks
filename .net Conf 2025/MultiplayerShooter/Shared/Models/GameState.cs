namespace MultiplayerShooter.Shared.Models;

public class GameState
{
    public List<Player> Players { get; set; } = new();
    public List<Bullet> Bullets { get; set; } = new();
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
