namespace MultiplayerShooter.Shared.Models;

public class GameRoom
{
    public string RoomId { get; set; } = string.Empty;
    public string RoomCode { get; set; } = string.Empty;
    public Dictionary<string, Player> Players { get; set; } = new();
    public List<Bullet> Bullets { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int MaxPlayers { get; set; } = 4;
    public bool IsFull => Players.Count >= MaxPlayers;
}
