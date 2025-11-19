namespace MultiplayerShooter.Shared.Models;

public class Player
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public float X { get; set; }
    public float Y { get; set; }
    public float Angle { get; set; }
    public int Health { get; set; } = 100;
    public bool IsAlive => Health > 0;
    public string Color { get; set; } = "#FF0000";
    public DateTime LastUpdate { get; set; } = DateTime.UtcNow;
    public int Kills { get; set; } = 0;
    public int Deaths { get; set; } = 0;
}
