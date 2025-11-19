using Microsoft.AspNetCore.SignalR;
using MultiplayerShooter.Shared.Models;
using System.Collections.Concurrent;

namespace MultiplayerShooter.Server.Hubs;

public class GameHub : Hub
{
    private static readonly ConcurrentDictionary<string, GameRoom> _rooms = new();
    private static readonly Random _random = new();
    private static readonly string[] _colors = { "#FF0000", "#00FF00", "#0000FF", "#FFFF00" };

    // Auto-matchmaking: Join first available room or create new one
    public async Task<string> JoinOrCreateRoom(string playerName)
    {
        // Try to find an available room first
        var availableRoom = _rooms.Values.FirstOrDefault(r => !r.IsFull);
        
        if (availableRoom != null)
        {
            // Join existing room
            var playerId = Context.ConnectionId;
            var playerIndex = availableRoom.Players.Count;
            
            var player = new Player
            {
                Id = playerId,
                Name = playerName,
                X = 100 + (playerIndex * 200),
                Y = 100 + (playerIndex * 150),
                Color = _colors[playerIndex % _colors.Length],
                Health = 100
            };

            availableRoom.Players.TryAdd(playerId, player);
            await Groups.AddToGroupAsync(Context.ConnectionId, availableRoom.RoomId);

            // Notify all players in room
            await Clients.Group(availableRoom.RoomId).SendAsync("PlayerJoined", player);
            
            // Send current game state to new player
            var gameState = new GameState
            {
                Players = availableRoom.Players.Values.ToList(),
                Bullets = availableRoom.Bullets
            };
            await Clients.Caller.SendAsync("GameState", gameState);

            return availableRoom.RoomCode;
        }
        else
        {
            // Create new room
            var roomCode = GenerateRoomCode();
            var roomId = Guid.NewGuid().ToString();
            var playerId = Context.ConnectionId;

            var player = new Player
            {
                Id = playerId,
                Name = playerName,
                X = 400,
                Y = 300,
                Color = _colors[0],
                Health = 100
            };

            var room = new GameRoom
            {
                RoomId = roomId,
                RoomCode = roomCode,
                Players = new Dictionary<string, Player> { { playerId, player } }
            };

            _rooms.TryAdd(roomId, room);
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);

            return roomCode;
        }
    }

    // Keep the old methods for backward compatibility (optional)
    public async Task<string> CreateRoom(string playerName)
    {
        var roomCode = GenerateRoomCode();
        var roomId = Guid.NewGuid().ToString();
        var playerId = Context.ConnectionId;

        var player = new Player
        {
            Id = playerId,
            Name = playerName,
            X = 400,
            Y = 300,
            Color = _colors[0],
            Health = 100
        };

        var room = new GameRoom
        {
            RoomId = roomId,
            RoomCode = roomCode,
            Players = new Dictionary<string, Player> { { playerId, player } }
        };

        _rooms.TryAdd(roomId, room);
        await Groups.AddToGroupAsync(Context.ConnectionId, roomId);

        return roomCode;
    }

    public async Task<bool> JoinRoom(string roomCode, string playerName)
    {
        var room = _rooms.Values.FirstOrDefault(r => r.RoomCode == roomCode);
        if (room == null || room.IsFull)
            return false;

        var playerId = Context.ConnectionId;
        var playerIndex = room.Players.Count;
        
        var player = new Player
        {
            Id = playerId,
            Name = playerName,
            X = 100 + (playerIndex * 200),
            Y = 100 + (playerIndex * 150),
            Color = _colors[playerIndex % _colors.Length],
            Health = 100
        };

        room.Players.TryAdd(playerId, player);
        await Groups.AddToGroupAsync(Context.ConnectionId, room.RoomId);

        // Notify all players in room
        await Clients.Group(room.RoomId).SendAsync("PlayerJoined", player);
        
        // Send current game state to new player
        var gameState = new GameState
        {
            Players = room.Players.Values.ToList(),
            Bullets = room.Bullets
        };
        await Clients.Caller.SendAsync("GameState", gameState);

        return true;
    }

    public async Task UpdatePosition(float x, float y, float angle)
    {
        var room = GetPlayerRoom(Context.ConnectionId);
        if (room == null) return;

        if (room.Players.TryGetValue(Context.ConnectionId, out var player))
        {
            player.X = x;
            player.Y = y;
            player.Angle = angle;
            player.LastUpdate = DateTime.UtcNow;

            await Clients.OthersInGroup(room.RoomId).SendAsync("PlayerMoved", player);
        }
    }

    public async Task Shoot(float angle)
    {
        var room = GetPlayerRoom(Context.ConnectionId);
        if (room == null) return;

        if (room.Players.TryGetValue(Context.ConnectionId, out var player))
        {
            var bullet = new Bullet
            {
                PlayerId = player.Id,
                X = player.X,
                Y = player.Y,
                VelocityX = (float)Math.Cos(angle) * 10,
                VelocityY = (float)Math.Sin(angle) * 10
            };

            room.Bullets.Add(bullet);
            await Clients.Group(room.RoomId).SendAsync("BulletFired", bullet);

            // Start bullet cleanup task
            _ = Task.Run(async () =>
            {
                await Task.Delay(3000);
                room.Bullets.Remove(bullet);
            });
        }
    }

    public async Task CheckHit(string bulletId, string targetPlayerId)
    {
        var room = GetPlayerRoom(Context.ConnectionId);
        if (room == null) return;

        var bullet = room.Bullets.FirstOrDefault(b => b.Id == bulletId);
        if (bullet == null) return;

        if (room.Players.TryGetValue(targetPlayerId, out var targetPlayer))
        {
            targetPlayer.Health -= 20;
            room.Bullets.Remove(bullet);

            await Clients.Group(room.RoomId).SendAsync("PlayerHit", targetPlayerId, targetPlayer.Health);

            if (!targetPlayer.IsAlive)
            {
                // Track kill for shooter
                if (room.Players.TryGetValue(bullet.PlayerId, out var shooter))
                {
                    shooter.Kills++;
                }
                
                // Track death for victim
                targetPlayer.Deaths++;
                
                await Clients.Group(room.RoomId).SendAsync("PlayerDied", targetPlayerId);
                
                // Broadcast updated scores
                await Clients.Group(room.RoomId).SendAsync("ScoreUpdated", shooter, targetPlayer);
            }
        }
    }

    public async Task Respawn()
    {
        var room = GetPlayerRoom(Context.ConnectionId);
        if (room == null) return;

        if (room.Players.TryGetValue(Context.ConnectionId, out var player))
        {
            player.Health = 100;
            player.X = _random.Next(100, 700);
            player.Y = _random.Next(100, 500);

            await Clients.Group(room.RoomId).SendAsync("PlayerRespawned", player);
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var room = GetPlayerRoom(Context.ConnectionId);
        if (room != null)
        {
            room.Players.Remove(Context.ConnectionId);
            await Clients.Group(room.RoomId).SendAsync("PlayerLeft", Context.ConnectionId);

            // Clean up empty rooms
            if (room.Players.Count == 0)
            {
                _rooms.TryRemove(room.RoomId, out _);
            }
        }

        await base.OnDisconnectedAsync(exception);
    }

    private GameRoom? GetPlayerRoom(string playerId)
    {
        return _rooms.Values.FirstOrDefault(r => r.Players.ContainsKey(playerId));
    }

    private static string GenerateRoomCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, 6)
            .Select(s => s[_random.Next(s.Length)]).ToArray());
    }
}
