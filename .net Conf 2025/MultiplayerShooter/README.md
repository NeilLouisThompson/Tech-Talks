# 🚀 Multiplayer Shooter - Blazor WASM & SignalR

A real-time multiplayer space shooter game built with .NET 9, Blazor WebAssembly, and SignalR. Perfect for demonstrating modern .NET web capabilities!

## ✨ Features

- 🎮 Real-time multiplayer (up to 4 players per room)
- 🤖 **Automatic matchmaking** - just click "Play Now"!
- � **Live scoreboard** - track kills, deaths, and health
- �🌐 Browser-based (no plugins required)
- 📱 **Fully mobile-responsive** with virtual joystick and touch controls
- 🔫 Shoot bullets at other players
- ❤️ Health system with respawn
- 🎨 HTML5 Canvas rendering
- 🔐 Smart room system - fills existing rooms first
- 🏠 Local network play

## 🛠️ Technologies

- .NET 9
- Blazor WebAssembly
- ASP.NET Core SignalR
- HTML5 Canvas
- JavaScript Interop
- C# 12

## 🚀 Quick Start

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Any modern web browser
- (Optional) Mobile device on same network for testing

### Running the Game

1. **Clone or navigate to the project**:

```powershell
cd MultiplayerShooter
```

2. **Restore packages**:

```powershell
dotnet restore
```

3. **Run the server**:

```powershell
cd Server
dotnet run
```

4. **Open in browser**:

- Desktop: `https://localhost:7001` or `http://localhost:5000`
- Mobile: `http://[YOUR_IP]:5000` (e.g., `http://192.168.1.100:5000`)

### Finding Your Local IP

**Windows**:

```powershell
ipconfig
# Look for "IPv4 Address" under your active network adapter
```

**macOS/Linux**:

```bash
ifconfig
# or
ip addr show
```

## 🎮 How to Play

### Desktop Controls

- **WASD**: Move your spaceship
- **Mouse**: Aim
- **Left Click**: Shoot

### Mobile Controls

- **D-Pad Buttons** (left): Up, Down, Left, Right movement
- **Shoot Button** (right): Fire bullets
- **Responsive Design**: Full-screen canvas optimized for mobile devices

### Game Flow

1. Enter your name
2. Click **"Play Now"** - the system automatically:
   - Joins an existing room with available slots, OR
   - Creates a new room if all existing rooms are full
3. You'll see a room code displayed (for reference)
4. Other players clicking "Play Now" will automatically join your room
5. Once 4 players are in a room, new players create/join other rooms
6. Shoot other players to reduce their health
7. When killed, click **Respawn** to continue playing

**That's it! No manual room codes needed - just play!** 🎮

## 📁 Project Structure

```
MultiplayerShooter/
├── Client/                          # Blazor WebAssembly App
│   ├── Pages/
│   │   ├── Index.razor             # Landing page
│   │   └── Game.razor              # Main game component
│   ├── wwwroot/
│   │   ├── js/
│   │   │   └── game.js             # Canvas rendering & input
│   │   └── index.html
│   └── Program.cs
│
├── Server/                          # ASP.NET Core Host
│   ├── Hubs/
│   │   └── GameHub.cs              # SignalR Hub (multiplayer logic)
│   └── Program.cs                  # Server configuration
│
└── Shared/                          # Shared Models
    └── Models/
        ├── Player.cs               # Player state
        ├── Bullet.cs               # Projectile data
        ├── GameRoom.cs             # Room management
        └── GameState.cs            # Synchronized game state
```

## 🔧 Configuration

### Allow Remote Connections

To allow devices on your local network to connect, edit `Server/Properties/launchSettings.json`:

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

### Firewall Settings

Make sure Windows Firewall allows incoming connections on port 5000:

```powershell
# Run as Administrator
netsh advfirewall firewall add rule name="Blazor Game" dir=in action=allow protocol=TCP localport=5000
```

## 🎯 Architecture

### Client-Side (Blazor WASM)

- Runs game loop at ~60 FPS
- Handles local input and rendering
- Client-side prediction for smooth movement
- Communicates with server via SignalR

### Server-Side (ASP.NET Core)

- SignalR Hub manages game rooms
- **Auto-matchmaking**: Fills existing rooms before creating new ones
- Broadcasts player positions and bullets
- Server-authoritative hit detection (prevents cheating)
- Smart room system with automatic player distribution

### Data Flow

```
Player Input → Blazor Component → SignalR Hub → Broadcast to Room → Update All Clients
```

## 🐛 Troubleshooting

### Mobile Can't Connect

- ✅ Ensure mobile is on same WiFi network
- ✅ Check Windows Firewall allows port 5000
- ✅ Try `http://` (not `https://`) on mobile
- ✅ Verify IP address is correct (use `ipconfig`)

### Game Lag

- Reduce SignalR message frequency in `UpdatePosition`
- Optimize JavaScript rendering calls
- Check network latency

### Players Not Moving

- Ensure keyboard input is working (click canvas first)
- Check browser console for errors
- Verify SignalR connection is established

## 🚀 Extending the Game

### Ideas for Enhancement

1. **Powerups**: Add health packs, speed boosts, shields
2. **Weapons**: Different bullet types, damage values
3. **Obstacles**: Walls, barriers on the map
4. **Score System**: Kills, deaths, leaderboard
5. **Particle Effects**: Explosions, bullet trails
6. **Sound Effects**: Using Howler.js
7. **Persistent Stats**: Add database for user profiles
8. **AI Bots**: Server-controlled players
9. **Multiple Maps**: Different level layouts
10. **Tournament Mode**: Brackets, elimination

## 📚 Learning Resources

- [Blazor Documentation](https://learn.microsoft.com/aspnet/core/blazor/)
- [SignalR with Blazor](https://learn.microsoft.com/aspnet/core/blazor/tutorials/signalr-blazor)
- [JavaScript Interop](https://learn.microsoft.com/aspnet/core/blazor/javascript-interoperability/)
- [Canvas API](https://developer.mozilla.org/en-US/docs/Web/API/Canvas_API)

## 📝 License

This project is for educational purposes. Feel free to use it in your talks, tutorials, or learning!

## 🤝 Contributing

This is a demo project for conference talks. Feel free to fork and extend it for your own presentations!

## 💡 Tips for Presenters

- **Test beforehand**: Run the game on multiple devices
- **Prepare backup**: Have screenshots/recordings in case of network issues
- **Engage audience**: Have them join from their phones!
- **Show code gradually**: Don't overwhelm - start with models, then hub, then UI
- **Emphasize concepts**: Real-time communication, client-side prediction, server authority

## 📧 Contact

For questions about this demo, reach out at your conference or via GitHub issues.

---

**Built with ❤️ for .NET Conf 2025**
