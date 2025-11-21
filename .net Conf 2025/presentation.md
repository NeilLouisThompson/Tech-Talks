# Building a Real-Time Multiplayer Shooter with Blazor WASM & SignalR

## .NET Conf 2025 Talk - 45 Minutes

---

## 📄 Abstract

**Title**: Building a Real-Time Multiplayer Shooter with Blazor WASM & SignalR

**Duration**: 45 minutes

**Level**: Beginner to Intermediate

**Description**:

Ever wondered if you could build a multiplayer game using just .NET and a browser? In this live-coding session, we'll build a fully functional real-time multiplayer space shooter from scratch using Blazor WebAssembly and SignalR—no game engines, no plugins, just pure .NET!

Watch as we create a browser-based game where multiple players can join from any device (desktop or mobile), shoot each other, track scores on a live leaderboard, and compete in real-time. You'll see firsthand how Blazor WASM runs C# directly in the browser, how SignalR enables seamless real-time communication, and how JavaScript Interop unlocks performance-critical rendering with HTML5 Canvas.

By the end of this session, you'll understand:

- How to architect real-time multiplayer applications with SignalR
- Client-side prediction patterns for smooth gameplay despite network latency
- Server-authoritative validation to prevent cheating
- Mobile-responsive design with touch controls
- Automatic matchmaking systems
- Performance optimization with JavaScript Interop

Whether you're building collaborative apps, real-time dashboards, or just want to explore what's possible with modern .NET, this session will demonstrate the versatility and power of Blazor and SignalR. Plus, you'll get to play the game on your phone during the demo!

**Key Takeaways**:

1. Blazor WASM can power real-time, interactive applications beyond traditional CRUD apps
2. SignalR makes complex real-time communication patterns simple and scalable
3. .NET 9 provides a complete stack for building modern, browser-based multiplayer experiences
4. Mobile-first design is achievable with pure C# and Blazor

**Technologies**: .NET 9, Blazor WebAssembly, ASP.NET Core SignalR, HTML5 Canvas, JavaScript Interop, C# 12

**Target Audience**: Developers of all levels interested in real-time web applications, game development, or pushing the boundaries of what Blazor can do. No prior game development experience required!

---

## 🎯 Talk Overview

**Target Audience**: Junior to Senior .NET Developers  
**Duration**: 45 minutes  
**Level**: Beginner to Intermediate  
**Technologies**: .NET 9, Blazor WebAssembly, SignalR, HTML5 Canvas, JavaScript Interop

### What You'll Learn

- How to build real-time multiplayer games with .NET
- Blazor WASM architecture and capabilities
- SignalR for real-time communication
- JavaScript Interop for canvas rendering
- Mobile-friendly game controls
- Local network multiplayer setup

---

## 📋 Table of Contents

1. [Introduction (5 min)](#1-introduction)
2. [Architecture Overview (5 min)](#2-architecture-overview)
3. [Setting Up the Project (5 min)](#3-setting-up-the-project)
4. [Building the Backend (10 min)](#4-building-the-backend)
5. [Creating the Frontend (12 min)](#5-creating-the-frontend)
6. [Mobile Support (3 min)](#6-mobile-support)
7. [Running & Testing (3 min)](#7-running--testing)
8. [Q&A (2 min)](#8-qa)

---

## 1. Introduction (5 min)

### Why Multiplayer Games with .NET?

**The Challenge**: Traditional game development often requires complex engines like Unity or Unreal. But what if you want to build a simple, browser-based multiplayer game?

**The Solution**:

- ✅ Blazor WASM runs C# in the browser
- ✅ SignalR provides real-time communication
- ✅ No plugins required - just a web browser
- ✅ Works on desktop AND mobile
- ✅ Easy to host and deploy

### What We're Building

A **real-time multiplayer space shooter** where:

- **Automatic matchmaking** - just click "Play Now"!
- **Live scoreboard** - track kills, deaths, and health
- Up to 4 players per room
- System fills existing rooms before creating new ones
- Each player controls a spaceship
- Shoot bullets at other players
- Real-time position sync
- **Mobile-responsive** with D-pad button controls for precise movement
- Local network play

**Demo Time!** _(Show the finished game)_

---

## 2. Architecture Overview (5 min)

### The Big Picture

```
┌─────────────────────────────────────────────────────┐
│                  Client Browser                     │
│  ┌──────────────────────────────────────────────┐  │
│  │         Blazor WebAssembly (C#)              │  │
│  │  • Game Logic                                │  │
│  │  • Input Handling                            │  │
│  │  • State Management                          │  │
│  └──────────────────┬───────────────────────────┘  │
│                     │                               │
│  ┌──────────────────▼───────────────────────────┐  │
│  │    JavaScript Interop Layer                  │  │
│  │  • Canvas Rendering                          │  │
│  │  • Keyboard/Touch Input                      │  │
│  └──────────────────────────────────────────────┘  │
└─────────────────────┬───────────────────────────────┘
                      │
                      │ SignalR WebSocket
                      │
┌─────────────────────▼───────────────────────────────┐
│              ASP.NET Core Server                    │
│  ┌──────────────────────────────────────────────┐  │
│  │            SignalR Hub                       │  │
│  │  • Matchmaking (Rooms)                       │  │
│  │  • State Broadcasting                        │  │
│  │  • Hit Detection                             │  │
│  │  • Player Management                         │  │
│  └──────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────┘
```

### Key Design Decisions

1. **Auto-Matchmaking**: System finds available rooms or creates new ones automatically
2. **Client-Side Prediction**: Players move immediately, then sync with server
3. **Server Authority**: Hit detection happens on server (prevents cheating)
4. **Efficient Updates**: Only broadcast changes, not full game state
5. **Smart Room Distribution**: Fills existing rooms before creating new ones

### Data Flow

```
Player Input → Blazor Component → SignalR Hub → Broadcast to Room → Update All Clients
```

---

## 3. Setting Up the Project (5 min)

### Step 1: Create the Blazor WASM Hosted Project

```powershell
# Create the project
dotnet new blazorwasm -ho -n MultiplayerShooter
cd MultiplayerShooter
```

### Step 2: Upgrade to .NET 9

Edit all `.csproj` files and change:

- `<TargetFramework>net7.0</TargetFramework>` → `<TargetFramework>net9.0</TargetFramework>`
- Package versions to `9.0.0`

### Step 3: Add SignalR Client

```powershell
cd Client
dotnet add package Microsoft.AspNetCore.SignalR.Client
```

### Project Structure

```
MultiplayerShooter/
├── Client/                    # Blazor WASM App
│   ├── Pages/
│   │   └── Game.razor        # Main game component
│   ├── wwwroot/
│   │   └── js/
│   │       └── game.js       # Canvas rendering JS
│   └── Program.cs
├── Server/                    # ASP.NET Core Host
│   ├── Hubs/
│   │   └── GameHub.cs        # SignalR Hub
│   └── Program.cs
└── Shared/                    # Shared Models
    └── Models/
        ├── Player.cs
        ├── Bullet.cs
        ├── GameRoom.cs
        └── GameState.cs
```

---

## 4. Building the Backend (10 min)

### Step 1: Create Shared Models

**Player.cs** - Represents a player in the game

```csharp
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
}
```

**Bullet.cs** - Represents a projectile

```csharp
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
```

**GameRoom.cs** - Manages a game session

```csharp
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
```

### Step 2: Create the SignalR Hub

**GameHub.cs** - The heart of multiplayer functionality

```csharp
using Microsoft.AspNetCore.SignalR;
using MultiplayerShooter.Shared.Models;
using System.Collections.Concurrent;

namespace MultiplayerShooter.Server.Hubs;

public class GameHub : Hub
{
    // Thread-safe storage for all active game rooms
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

            // Notify all players
            await Clients.Group(availableRoom.RoomId).SendAsync("PlayerJoined", player);

            // Send current state to new player
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

    // Alternative: Join a specific room by code (optional feature)
    public async Task<bool> JoinSpecificRoom(string roomCode, string playerName)
    {
        var room = _rooms.Values.FirstOrDefault(r => r.RoomCode == roomCode);
        if (room == null || room.IsFull) return false;

        var playerId = Context.ConnectionId;
        var player = new Player
        {
            Id = playerId,
            Name = playerName,
            X = 400,
            Y = 300,
            Color = _colors[room.Players.Count % _colors.Length],
            Health = 100
        };

        room.Players.TryAdd(playerId, player);
        await Groups.AddToGroupAsync(Context.ConnectionId, room.Id);

        return true;
    }

        return roomCode;
    }

    // Join an existing room (kept for reference)
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

        // Notify all players
        await Clients.Group(room.RoomId).SendAsync("PlayerJoined", player);

        // Send current state to new player
        var gameState = new GameState
        {
            Players = room.Players.Values.ToList(),
            Bullets = room.Bullets
        };
        await Clients.Caller.SendAsync("GameState", gameState);

        return true;
    }

    // Update player position (called frequently)
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

            // Broadcast to other players only
            await Clients.OthersInGroup(room.RoomId).SendAsync("PlayerMoved", player);
        }
    }

    // Handle shooting
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

            // Auto-cleanup after 3 seconds
            _ = Task.Run(async () =>
            {
                await Task.Delay(3000);
                room.Bullets.Remove(bullet);
            });
        }
    }

    // Server-side hit detection
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
                await Clients.Group(room.RoomId).SendAsync("PlayerDied", targetPlayerId);
            }
        }
    }

    // Respawn a dead player
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

    // Handle disconnections
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
```

### Step 3: Configure Server

**Program.cs** - Wire up SignalR

```csharp
using Microsoft.AspNetCore.ResponseCompression;
using MultiplayerShooter.Server.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSignalR();

// Response compression for SignalR
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});

var app = builder.Build();

app.UseResponseCompression();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();

// Map the SignalR hub
app.MapHub<GameHub>("/gamehub");
app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
```

**Key Points to Explain**:

- `ConcurrentDictionary` for thread-safe room storage
- SignalR Groups for room-based messaging
- `Clients.OthersInGroup()` to avoid echoing back to sender
- Server-side hit detection for security

---

## 5. Creating the Frontend (12 min)

### Step 1: JavaScript Interop for Canvas

**wwwroot/js/game.js** - Handle rendering

```javascript
// Game rendering and input handling via JavaScript Interop

let canvasContext = null;
let dotNetHelper = null;

window.isMobileDevice = () => {
  return /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(
    navigator.userAgent
  );
};

window.initializeCanvas = (canvas) => {
  canvasContext = canvas.getContext("2d");
};

window.setupKeyboardInput = (helper) => {
  dotNetHelper = helper;

  document.addEventListener("keydown", (e) => {
    if (dotNetHelper) {
      dotNetHelper.invokeMethodAsync("OnKeyDown", e.key);
    }
  });

  document.addEventListener("keyup", (e) => {
    if (dotNetHelper) {
      dotNetHelper.invokeMethodAsync("OnKeyUp", e.key);
    }
  });
};

window.clearCanvas = (canvas) => {
  const ctx = canvas.getContext("2d");
  ctx.fillStyle = "#1a1a2e";
  ctx.fillRect(0, 0, canvas.width, canvas.height);

  // Draw grid
  ctx.strokeStyle = "rgba(255, 255, 255, 0.05)";
  ctx.lineWidth = 1;
  for (let x = 0; x < canvas.width; x += 50) {
    ctx.beginPath();
    ctx.moveTo(x, 0);
    ctx.lineTo(x, canvas.height);
    ctx.stroke();
  }
  for (let y = 0; y < canvas.height; y += 50) {
    ctx.beginPath();
    ctx.moveTo(0, y);
    ctx.lineTo(canvas.width, y);
    ctx.stroke();
  }
};

window.drawPlayer = (canvas, x, y, color, angle, isAlive) => {
  const ctx = canvas.getContext("2d");

  if (!isAlive) {
    ctx.globalAlpha = 0.3;
  }

  ctx.save();
  ctx.translate(x, y);
  ctx.rotate(angle);

  // Draw ship body
  ctx.fillStyle = color;
  ctx.beginPath();
  ctx.moveTo(15, 0);
  ctx.lineTo(-10, -10);
  ctx.lineTo(-10, 10);
  ctx.closePath();
  ctx.fill();

  // Draw ship outline
  ctx.strokeStyle = "white";
  ctx.lineWidth = 2;
  ctx.stroke();

  // Draw thruster
  ctx.fillStyle = "#ff6b6b";
  ctx.fillRect(-10, -3, -5, 6);

  ctx.restore();
  ctx.globalAlpha = 1;
};

window.drawBullet = (canvas, x, y) => {
  const ctx = canvas.getContext("2d");

  ctx.fillStyle = "#ffff00";
  ctx.beginPath();
  ctx.arc(x, y, 3, 0, Math.PI * 2);
  ctx.fill();

  // Glow effect
  ctx.fillStyle = "rgba(255, 255, 0, 0.3)";
  ctx.beginPath();
  ctx.arc(x, y, 6, 0, Math.PI * 2);
  ctx.fill();
};

window.drawText = (canvas, text, x, y, color) => {
  const ctx = canvas.getContext("2d");

  ctx.font = "14px Arial";
  ctx.fillStyle = color;
  ctx.textAlign = "center";
  ctx.fillText(text, x, y);
};
```

**Why JavaScript?**

- Canvas API is JavaScript-only
- Much faster than C# for rendering
- C# handles game logic, JS handles drawing

### Step 2: The Game Component (Simplified Version)

**Game.razor** - Main game logic

```razor
@page "/game"
@using Microsoft.AspNetCore.SignalR.Client
@using MultiplayerShooter.Shared.Models
@inject NavigationManager Navigation
@inject IJSRuntime JS
@implements IAsyncDisposable

<PageTitle>Multiplayer Shooter</PageTitle>

<div class="game-container">
    @if (!_isInGame)
    {
        <!-- Menu UI -->
        <div class="menu">
            <h1>🚀 Multiplayer Shooter</h1>
            <input @bind="_playerName" placeholder="Enter your name" />
            <button @onclick="JoinOrCreateRoom" disabled="@(!IsConnected)">
                Play Now
            </button>

            @if (!string.IsNullOrEmpty(_currentRoomCode))
            {
                <div class="room-code">
                    <h3>Room Code: @_currentRoomCode</h3>
                    <p>Automatically matched!</p>
                </div>
            }
        </div>
    }
    else
    {
        <!-- Game UI -->
        <div class="game-ui">
            <div class="hud">
                <div class="health-bar">
                    <span>Health: @_localPlayer?.Health</span>
                </div>
            </div>

            <canvas @ref="_canvasRef" width="800" height="600"
                    @onmousedown="OnMouseDown"
                    @onmousemove="OnMouseMove"></canvas>

            @if (!_localPlayer?.IsAlive ?? false)
            {
                <div class="death-screen">
                    <h2>You Died!</h2>
                    <button @onclick="Respawn">Respawn</button>
                </div>
            }
        </div>
    }
</div>

@code {
    private HubConnection? _hubConnection;
    private ElementReference _canvasRef;
    private string _playerName = "";
    private string _roomCode = "";
    private string _currentRoomCode = "";
    private bool _isInGame = false;

    private Player? _localPlayer;
    private Dictionary<string, Player> _players = new();
    private List<Bullet> _bullets = new();

    private bool[] _keys = new bool[256];
    private float _mouseAngle = 0;
    private DateTime _lastShot = DateTime.MinValue;

    private System.Threading.Timer? _gameLoopTimer;

    private bool IsConnected => _hubConnection?.State == HubConnectionState.Connected;

    protected override async Task OnInitializedAsync()
    {
        // Initialize SignalR connection
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(Navigation.ToAbsoluteUri("/gamehub"))
            .Build();

        // Register event handlers
        _hubConnection.On<Player>("PlayerJoined", (player) =>
        {
            _players[player.Id] = player;
            InvokeAsync(StateHasChanged);
        });

        _hubConnection.On<GameState>("GameState", (state) =>
        {
            foreach (var player in state.Players)
            {
                _players[player.Id] = player;
                if (player.Id == _hubConnection.ConnectionId)
                    _localPlayer = player;
            }
            _bullets = state.Bullets;
            InvokeAsync(StateHasChanged);
        });

        _hubConnection.On<Player>("PlayerMoved", (player) =>
        {
            if (_players.ContainsKey(player.Id))
                _players[player.Id] = player;
        });

        _hubConnection.On<Bullet>("BulletFired", (bullet) =>
        {
            _bullets.Add(bullet);
        });

        _hubConnection.On<string, int>("PlayerHit", (playerId, health) =>
        {
            if (_players.TryGetValue(playerId, out var player))
            {
                player.Health = health;
                if (playerId == _localPlayer?.Id)
                {
                    _localPlayer.Health = health;
                    InvokeAsync(StateHasChanged);
                }
            }
        });

        await _hubConnection.StartAsync();
    }

    private async Task JoinOrCreateRoom()
    {
        if (string.IsNullOrWhiteSpace(_playerName)) return;

        _currentRoomCode = await _hubConnection!.InvokeAsync<string>("JoinOrCreateRoom", _playerName);
        _isInGame = true;
        _localPlayer = new Player { Id = _hubConnection.ConnectionId!, Name = _playerName };
        _players[_localPlayer.Id] = _localPlayer;

        StartGameLoop();
    }

    // Alternative: Join specific room by code (optional)
    private async Task JoinSpecificRoom()
    {
        if (string.IsNullOrWhiteSpace(_playerName) || string.IsNullOrWhiteSpace(_roomCode)) return;

        var success = await _hubConnection!.InvokeAsync<bool>("JoinSpecificRoom", _roomCode.ToUpper(), _playerName);
        if (success)
        {
            _isInGame = true;
            _currentRoomCode = _roomCode.ToUpper();
            StartGameLoop();
        }
    }

    private void StartGameLoop()
    {
        // 60 FPS game loop
        _gameLoopTimer = new System.Threading.Timer(async _ => await GameLoop(), null, 0, 16);
    }

    private async Task GameLoop()
    {
        if (_localPlayer == null || !_localPlayer.IsAlive) return;

        // Update position based on keyboard input
        float speed = 3f;
        float dx = 0, dy = 0;

        if (_keys['W'] || _keys['w']) dy -= speed;
        if (_keys['S'] || _keys['s']) dy += speed;
        if (_keys['A'] || _keys['a']) dx -= speed;
        if (_keys['D'] || _keys['d']) dx += speed;

        if (dx != 0 || dy != 0)
        {
            _localPlayer.X += dx;
            _localPlayer.Y += dy;

            // Keep in bounds
            _localPlayer.X = Math.Clamp(_localPlayer.X, 20, 780);
            _localPlayer.Y = Math.Clamp(_localPlayer.Y, 20, 580);

            // Send to server
            await _hubConnection!.SendAsync("UpdatePosition", _localPlayer.X, _localPlayer.Y, _mouseAngle);
        }

        // Update bullets and check collisions
        for (int i = _bullets.Count - 1; i >= 0; i--)
        {
            var bullet = _bullets[i];
            bullet.X += bullet.VelocityX;
            bullet.Y += bullet.VelocityY;

            // Collision detection
            foreach (var player in _players.Values.Where(p => p.IsAlive && p.Id != bullet.PlayerId))
            {
                var distance = MathF.Sqrt(
                    MathF.Pow(bullet.X - player.X, 2) +
                    MathF.Pow(bullet.Y - player.Y, 2));

                if (distance < 15)
                {
                    await _hubConnection!.SendAsync("CheckHit", bullet.Id, player.Id);
                    _bullets.RemoveAt(i);
                    break;
                }
            }

            // Remove out of bounds bullets
            if (bullet.X < 0 || bullet.X > 800 || bullet.Y < 0 || bullet.Y > 600)
                _bullets.RemoveAt(i);
        }

        // Render
        await RenderGame();
    }

    private async Task RenderGame()
    {
        await JS.InvokeVoidAsync("clearCanvas", _canvasRef);

        // Draw all players
        foreach (var player in _players.Values)
        {
            await JS.InvokeVoidAsync("drawPlayer", _canvasRef,
                player.X, player.Y, player.Color, player.Angle, player.IsAlive);
            await JS.InvokeVoidAsync("drawText", _canvasRef,
                player.Name, player.X, player.Y - 30, "white");
        }

        // Draw all bullets
        foreach (var bullet in _bullets)
        {
            await JS.InvokeVoidAsync("drawBullet", _canvasRef, bullet.X, bullet.Y);
        }
    }

    [JSInvokable]
    public void OnKeyDown(string key)
    {
        if (key.Length == 1)
            _keys[key[0]] = true;
    }

    [JSInvokable]
    public void OnKeyUp(string key)
    {
        if (key.Length == 1)
            _keys[key[0]] = false;
    }

    private void OnMouseMove(MouseEventArgs e)
    {
        if (_localPlayer == null) return;

        // Calculate angle to mouse
        _mouseAngle = MathF.Atan2(
            (float)e.OffsetY - _localPlayer.Y,
            (float)e.OffsetX - _localPlayer.X);
        _localPlayer.Angle = _mouseAngle;
    }

    private void OnMouseDown(MouseEventArgs e)
    {
        Shoot();
    }

    private async void Shoot()
    {
        // Rate limit: 250ms between shots
        if ((DateTime.UtcNow - _lastShot).TotalMilliseconds < 250) return;
        if (_localPlayer == null || !_localPlayer.IsAlive) return;

        _lastShot = DateTime.UtcNow;
        await _hubConnection!.SendAsync("Shoot", _mouseAngle);
    }

    private async Task Respawn()
    {
        await _hubConnection!.SendAsync("Respawn");
    }

    public async ValueTask DisposeAsync()
    {
        _gameLoopTimer?.Dispose();
        if (_hubConnection is not null)
        {
            await _hubConnection.DisposeAsync();
        }
    }
}
```

**Key Concepts to Highlight**:

1. **SignalR Event Handlers**: `.On<T>()` methods
2. **Game Loop**: 60 FPS timer for updates
3. **Client-Side Prediction**: Immediate movement, then sync
4. **JavaScript Interop**: `IJSRuntime` for canvas calls
5. **Two-Way Callbacks**: `[JSInvokable]` for JS → C#

---

## 6. Mobile Support (3 min)

### D-Pad Button Controls

The game includes responsive touch controls with directional buttons for mobile devices:

```razor
@if (_isMobile)
{
    <div class="mobile-controls">
        <div class="dpad-container">
            <button class="dpad-btn dpad-up"
                    @ontouchstart="() => OnDirectionPress('W')"
                    @ontouchend="() => OnDirectionRelease('W')">
                ▲
            </button>
            <div class="dpad-middle">
                <button class="dpad-btn dpad-left"
                        @ontouchstart="() => OnDirectionPress('A')"
                        @ontouchend="() => OnDirectionRelease('A')">
                    ◄
                </button>
                <button class="dpad-btn dpad-right"
                        @ontouchstart="() => OnDirectionPress('D')"
                        @ontouchend="() => OnDirectionRelease('D')">
                    ►
                </button>
            </div>
            <button class="dpad-btn dpad-down"
                    @ontouchstart="() => OnDirectionPress('S')"
                    @ontouchend="() => OnDirectionRelease('S')">
                ▼
            </button>
        </div>
        <button class="shoot-button" @ontouchstart="OnShootTouch">🔫</button>
    </div>
}
```

**Touch Handler Code**:

```csharp
private void OnDirectionPress(char key)
{
    _keys[key] = true;  // Set the direction key as active
}

private void OnDirectionRelease(char key)
{
    _keys[key] = false;  // Release the direction key
}
```

**Why This Matters**:

- No external libraries needed - pure Blazor!
- Responsive D-pad buttons provide precise control
- Touch events handled natively in C#
- Full-screen responsive design on mobile
- Works on any mobile browser
- Same codebase for desktop and mobile!

---

## 7. Running & Testing (3 min)

### Local Network Setup

1. **Find Your Local IP**:

```powershell
ipconfig
# Look for IPv4 Address (e.g., 192.168.1.100)
```

2. **Update Launch Settings** (launchSettings.json):

```json
{
  "profiles": {
    "MultiplayerShooter.Server": {
      "commandName": "Project",
      "launchBrowser": true,
      "applicationUrl": "https://0.0.0.0:7001;http://0.0.0.0:5000",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
```

3. **Run the Server**:

```powershell
cd Server
dotnet run
```

4. **Connect from Devices**:

- **Desktop**: `http://localhost:5000`
- **Mobile**: `http://10.112.4.38:3000` (use your local IP on port 3000)

### Testing Checklist

✅ Click "Play Now" on desktop to auto-join  
✅ Join from phone - automatically matched to same room  
✅ Test movement (WASD on desktop, D-pad buttons on mobile)  
✅ Test shooting (click on desktop, shoot button on mobile)  
✅ Verify health decreases on hit  
✅ Test respawn functionality  
✅ Check player disconnect handling

### Common Issues & Solutions

**Issue**: Can't connect from phone  
**Solution**: Check firewall, ensure same WiFi network

**Issue**: Lag or stuttering  
**Solution**: Reduce SignalR message frequency, optimize rendering

**Issue**: Bullets not hitting  
**Solution**: Tune collision detection distance (currently 15px)

---

## 8. Q&A (2 min)

### Potential Questions & Answers

**Q: Can this scale to 100+ players?**  
A: Not as-is. You'd need:

- Redis backplane for SignalR
- Spatial partitioning for collision detection
- Dedicated game servers
- Rate limiting

**Q: How do you prevent cheating?**  
A: Server-side validation! All hit detection happens on the server. Clients just report actions.

**Q: Can I add AI bots?**  
A: Yes! Create "fake" players in the Hub and update them with timer-based logic.

**Q: How about adding powerups, weapons, maps?**  
A: Absolutely! Extend the `GameRoom` model and broadcast state changes.

**Q: What about production deployment?**  
A: Deploy to Azure App Service, AWS, or any container platform. SignalR works great in production!

---

## 🎁 Bonus: Extensions & Challenges

### For Attendees to Try:

1. **Add Powerups**: Health packs, speed boosts, shields
2. **Multiple Weapons**: Different bullet types, damage, speeds
3. **Score System**: Track kills, deaths, leaderboard
4. **Custom Maps**: Obstacles, walls, different layouts
5. **Spectator Mode**: Watch games after death
6. **Tournament Mode**: Brackets, elimination rounds
7. **Particle Effects**: Explosions, trails, impact effects
8. **Sound Effects**: Using Howler.js via JS interop
9. **Persistent Stats**: Entity Framework Core for user profiles
10. **Mobile Optimization**: Better touch controls, haptic feedback

---

## 📚 Resources

### Documentation

- [Blazor Documentation](https://learn.microsoft.com/aspnet/core/blazor/)
- [SignalR with Blazor](https://learn.microsoft.com/aspnet/core/blazor/tutorials/signalr-blazor)
- [JavaScript Interop](https://learn.microsoft.com/aspnet/core/blazor/javascript-interoperability/)
- [Canvas API](https://developer.mozilla.org/en-US/docs/Web/API/Canvas_API)

### GitHub Repository

Complete source code: `[Your GitHub URL]`

### Follow-Up Learning

- **Multiplayer Architecture**: Look into client-side prediction, lag compensation
- **Game Loop Patterns**: Fixed timestep vs variable timestep
- **WebGL with Blazor**: For 3D games (Blazor + Three.js)
- **Performance Profiling**: Browser DevTools, dotnet-trace

---

## 🎬 Closing

### Key Takeaways

1. ✅ **Blazor WASM is production-ready** for interactive web apps
2. ✅ **SignalR makes real-time easy** - no WebSocket plumbing needed
3. ✅ **JavaScript Interop bridges the gap** for browser APIs
4. ✅ **C# can do game logic** - you don't always need Unity!
5. ✅ **Mobile support is built-in** - progressive web apps FTW

### What's Next?

- Try extending this game with your own features
- Build other real-time apps: Chat, collaborative tools, dashboards
- Explore Blazor Hybrid (MAUI) for native mobile apps
- Join the .NET community on GitHub, Discord, Twitter

### Thank You!

**Questions?** Let's discuss!

**Stay Connected**:

- Twitter: @YourHandle
- GitHub: github.com/yourusername
- Email: your@email.com

---

## 🛠️ Appendix: Full CSS Styling

```css
.game-container {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 100vh;
  background: linear-gradient(135deg, #1e3c72 0%, #2a5298 100%);
  padding: 20px;
}

.menu {
  background: rgba(255, 255, 255, 0.95);
  padding: 40px;
  border-radius: 20px;
  box-shadow: 0 10px 40px rgba(0, 0, 0, 0.3);
  max-width: 400px;
  width: 100%;
  text-align: center;
}

.menu h1 {
  margin-bottom: 30px;
  color: #1e3c72;
  font-size: 2.5rem;
}

canvas {
  background: #1a1a2e;
  border: 4px solid #16213e;
  border-radius: 10px;
  cursor: crosshair;
  display: block;
}

.health-bar {
  position: relative;
  width: 200px;
  height: 30px;
  background: rgba(0, 0, 0, 0.5);
  border: 2px solid white;
  border-radius: 15px;
  overflow: hidden;
}

.joystick-base {
  width: 100px;
  height: 100px;
  background: rgba(255, 255, 255, 0.3);
  border: 3px solid white;
  border-radius: 50%;
  position: relative;
}

.shoot-button {
  width: 80px;
  height: 80px;
  background: linear-gradient(135deg, #f5576c 0%, #f093fb 100%);
  border: 4px solid white;
  border-radius: 50%;
  font-size: 32px;
}

@media (max-width: 768px) {
  canvas {
    width: 100%;
    height: auto;
  }
}
```

---

## 🎯 Time Management

- **00:00-05:00**: Introduction & Demo
- **05:00-10:00**: Architecture explanation
- **10:00-15:00**: Project setup & structure
- **15:00-25:00**: Backend implementation (models + hub)
- **25:00-37:00**: Frontend implementation (Blazor + JS)
- **37:00-40:00**: Mobile support
- **40:00-43:00**: Running & testing
- **43:00-45:00**: Q&A & closing

---

**End of Presentation** 🎮
