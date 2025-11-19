namespace MultiplayerShooter.Shared.Models;

public class Bullet
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string PlayerId { get; set; } = string.Empty;
    public float X { get; set; }
    public float Y { get; set; }
    public float VelocityX { get; set; }
    public float VelocityY { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
