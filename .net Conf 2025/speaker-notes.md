# Speaker Notes - Multiplayer Shooter Talk

## .NET Conf 2025 - Internal Guide

---

## 🎤 Pre-Talk Checklist

### 1 Week Before

- [ ] Test the demo on your laptop
- [ ] Test on mobile device (iOS & Android if possible)
- [ ] Verify local network setup works
- [ ] Prepare backup recordings/screenshots
- [ ] Practice the demo flow
- [ ] Time your presentation (aim for 40-42 min, leaving 3-5 for Q&A)

### 1 Day Before

- [ ] Update .NET SDK if needed
- [ ] Clone fresh copy and test build
- [ ] Charge all devices
- [ ] Test projector/screen sharing
- [ ] Prepare QR code for GitHub repo
- [ ] Print room code on paper (backup)

### 1 Hour Before

- [ ] Start the server early
- [ ] Test WiFi connection
- [ ] Open all code files in VS Code
- [ ] Have browser tabs ready
- [ ] Join conference WiFi (if using it)
- [ ] Get your local IP address

### Right Before Talk

- [ ] Close unnecessary apps
- [ ] Turn off notifications
- [ ] Set Do Not Disturb
- [ ] Increase font size in IDE
- [ ] Test microphone
- [ ] Smile! 😊

---

## 🎯 Talk Flow & Timing

### Intro (5 min) - Build Excitement

**Energy: HIGH**

**Opening Line**:

> "Who here has played multiplayer games? _[Show of hands]_ Great! Today, we're going to build one from scratch—in under 45 minutes—using only .NET and a browser. No game engines, no plugins, just C# and SignalR. And by the end, you'll be playing it on your phones!"

**Key Points**:

- This is about **possibilities** with .NET
- Show that .NET is versatile (not just CRUD apps!)
- Build excitement with the live demo

**Demo the Finished Game**:

1. Show it running on your laptop
2. Pull out phone, join the game
3. Shoot yourself (audience loves this!)
4. "This is what we're building. Let's go!"

**Transition**: "So how does this actually work? Let's look at the architecture..."

---

### Architecture (5 min) - Set Foundation

**Energy: MEDIUM-HIGH**

**Visual**: Show the architecture diagram from slides

**Key Messages**:

1. **Client-Side**: Blazor WASM runs C# in the browser
2. **Server-Side**: SignalR Hub manages game state
3. **JavaScript Interop**: For canvas rendering (performance)
4. **Real-Time**: WebSocket keeps everything in sync

**Audience Engagement**:

- "Who's used SignalR before?" _[Show of hands]_
- "Who's built a game before?" _[Usually few hands]_
- "Perfect! This will be new for most of us."

**Important Concept - Client-Server Split**:

> "Here's the clever part: The client predicts movement immediately—so it feels smooth—then the server confirms it. The server does all hit detection to prevent cheating. This is how real multiplayer games work!"

**Transition**: "Let's start building. First, the project setup..."

---

### Project Setup (5 min) - Get Everyone Oriented

**Energy: MEDIUM**

**Live Coding**: Open terminal

```powershell
# Talk while typing
dotnet new blazorwasm -ho -n MultiplayerShooter
```

**While it runs**:

- Explain "hosted" means client + server together
- Mention we're using .NET 9 (latest stable)
- Show the three projects: Client, Server, Shared

**Quick Tour**:

1. **Server**: "This hosts the app and runs our SignalR hub"
2. **Client**: "This is the Blazor WASM app that runs in the browser"
3. **Shared**: "Models used by both client and server"

**Add SignalR**:

```powershell
cd Client
dotnet add package Microsoft.AspNetCore.SignalR.Client
```

**Pro Tip for Audience**:

> "When you do this at home, make sure to use `--hosted` or `-ho` flag. That's what gives you the server project. Without it, you'd need to set up SignalR separately."

**Transition**: "Alright, project is ready. Let's build the backend first..."

---

### Backend - Models (3 min) - Show the Data

**Energy: MEDIUM**

**Show the Models**: Open each file briefly

**Player.cs**:

```csharp
public class Player
{
    public string Id { get; set; }
    public float X { get; set; }
    public float Y { get; set; }
    public int Health { get; set; } = 100;
    public bool IsAlive => Health > 0;
    // ...
}
```

**Key Points**:

- "Simple POCO classes—nothing fancy"
- "Notice `IsAlive`—computed property, very handy"
- "We track position (X, Y), angle, health"

**Bullet.cs**:

- "Even simpler—just position and velocity"
- "Each bullet knows who shot it (PlayerId)"

**GameRoom.cs**:

- "This is where the magic happens"
- "One room per game session"
- "Dictionary of players—fast lookups"
- "Auto-matchmaking fills rooms up to 4 players"

**Transition**: "Now for the real backend work—the SignalR Hub..."

---

### Backend - SignalR Hub (7 min) - The Core Logic

**Energy: HIGH** (This is the meat!)

**Open GameHub.cs**

**Explain the Setup**:

```csharp
private static readonly ConcurrentDictionary<string, GameRoom> _rooms = new();
```

> "ConcurrentDictionary because multiple people can create/join rooms at the same time. Thread safety matters!"

**Walk Through Key Methods**:

**1. JoinOrCreateRoom** (Auto-Matchmaking):

```csharp
public async Task<string> JoinOrCreateRoom(string playerName)
{
    // Find first available room
    var room = _rooms.Values.FirstOrDefault(r => !r.IsFull);

    if (room == null)
    {
        // Create new room if all are full
        var roomCode = GenerateRoomCode();
        room = new GameRoom { RoomCode = roomCode };
        _rooms.TryAdd(room.Id, room);
    }

    // Add player to room
    var player = new Player { Id = Context.ConnectionId, Name = playerName };
    room.Players.TryAdd(player.Id, player);

    await Groups.AddToGroupAsync(Context.ConnectionId, room.Id);
    return room.RoomCode;
}
```

**Points**:

- SignalR Groups = isolated broadcast channels
- Auto-matchmaking: finds first non-full room
- Creates new room only if all existing rooms are full
- Simplified UX: one button, instant play

**2. UpdatePosition**:

```csharp
public async Task UpdatePosition(float x, float y, float angle)
{
    var room = _rooms.Values.FirstOrDefault(r => r.Players.ContainsKey(Context.ConnectionId));
    if (room == null || room.IsFull) return false;
    // ...
    await Clients.Group(room.RoomId).SendAsync("PlayerJoined", player);
}
```

**Points**:

- Find room by code
- Add to SignalR group
- Broadcast to existing players: "Hey, someone joined!"

**3. UpdatePosition** (Most called):

```csharp
public async Task UpdatePosition(float x, float y, float angle)
{
    // Update player in room
    await Clients.OthersInGroup(room.RoomId).SendAsync("PlayerMoved", player);
}
```

**Emphasize**:

> "Notice `OthersInGroup`—not `Group`. We don't echo back to the sender. They already moved on their screen! This saves bandwidth."

**4. Shoot**:

```csharp
public async Task Shoot(float angle)
{
    var bullet = new Bullet { /* ... */ };
    room.Bullets.Add(bullet);
    await Clients.Group(room.RoomId).SendAsync("BulletFired", bullet);

    // Cleanup
    _ = Task.Run(async () => {
        await Task.Delay(3000);
        room.Bullets.Remove(bullet);
    });
}
```

**Points**:

- Create bullet, broadcast to everyone
- Auto-cleanup after 3 seconds (fire and forget)

**5. CheckHit** (Server Authority):

```csharp
public async Task CheckHit(string bulletId, string targetPlayerId)
{
    // Validate on server
    targetPlayer.Health -= 20;
    await Clients.Group(room.RoomId).SendAsync("PlayerHit", ...);
}
```

**Emphasize**:

> "This is crucial! The CLIENT detects a collision and asks the server to confirm. The SERVER decides if it's valid. This prevents cheating. Never trust the client in multiplayer games!"

**6. OnDisconnectedAsync**:

```csharp
public override async Task OnDisconnectedAsync(Exception? exception)
{
    room.Players.Remove(Context.ConnectionId);
    await Clients.Group(room.RoomId).SendAsync("PlayerLeft", ...);
}
```

**Points**:

- Automatic cleanup when someone disconnects
- Notify other players

**Transition**: "That's the backend! Now let's build the fun part—the UI..."

---

### Frontend - JavaScript Interop (3 min) - The Bridge

**Energy: MEDIUM-HIGH**

**Open game.js**

**Explain Why JavaScript**:

> "Blazor is amazing, but Canvas API is JavaScript-only. And frankly, JS is faster for rendering. So we use the best tool for each job."

**Show Key Functions**:

**drawPlayer**:

```javascript
window.drawPlayer = (canvas, x, y, color, angle, isAlive) => {
  const ctx = canvas.getContext("2d");
  ctx.save();
  ctx.translate(x, y);
  ctx.rotate(angle);
  // Draw triangle ship
  ctx.restore();
};
```

**Points**:

- Canvas transforms (translate, rotate)
- Called from C# via `IJSRuntime`

**setupKeyboardInput**:

```javascript
window.setupKeyboardInput = (helper) => {
  document.addEventListener("keydown", (e) => {
    helper.invokeMethodAsync("OnKeyDown", e.key);
  });
};
```

**Points**:

- JS listens to keyboard
- Calls back into C# via `DotNetObjectReference`
- Bidirectional communication!

**Transition**: "Now the Blazor component that ties it all together..."

---

### Frontend - Blazor Component (9 min) - The Main Event

**Energy: HIGH**

**Open Game.razor**

**Structure Overview**:

- Menu UI (create/join room)
- Game UI (canvas, HUD)
- SignalR connection code
- Game loop

**1. SignalR Setup** (2 min):

```csharp
_hubConnection = new HubConnectionBuilder()
    .WithUrl(Navigation.ToAbsoluteUri("/gamehub"))
    .Build();

_hubConnection.On<Player>("PlayerJoined", (player) => {
    _players[player.Id] = player;
    InvokeAsync(StateHasChanged);
});
```

**Points**:

- Create connection to `/gamehub`
- Register event handlers for server messages
- `InvokeAsync(StateHasChanged)` to update UI

**2. Join Room** (2 min):

```csharp
private async Task JoinOrCreateRoom()
{
    _currentRoomCode = await _hubConnection!.InvokeAsync<string>("JoinOrCreateRoom", _playerName);
    _isInGame = true;
    StartGameLoop();
}
```

**Points**:

- Call hub method via `InvokeAsync`
- Auto-matchmaking finds or creates room
- Switch to game view and start the game loop
- Simplified to single "Play Now" button

**3. Game Loop** (3 min) - Most Important:

```csharp
private void StartGameLoop()
{
    _gameLoopTimer = new System.Threading.Timer(
        async _ => await GameLoop(), null, 0, 16); // ~60 FPS
}

private async Task GameLoop()
{
    // 1. Handle input
    if (_keys['W']) dy -= speed;

    // 2. Update position
    _localPlayer.X += dx;
    await _hubConnection!.SendAsync("UpdatePosition", ...);

    // 3. Update bullets
    foreach (var bullet in _bullets) {
        bullet.X += bullet.VelocityX;
        // Check collisions
    }

    // 4. Render
    await RenderGame();
}
```

**Emphasize**:

> "This runs 60 times per second. That's your game loop—the heart of any game. Input, Update, Render. Every frame."

**Client-Side Prediction**:

> "See how we update the position immediately? We don't wait for the server. This makes it feel smooth. The server will sync us if we're wrong, but usually we're right!"

**4. Rendering** (1 min):

```csharp
private async Task RenderGame()
{
    await JS.InvokeVoidAsync("clearCanvas", _canvasRef);

    foreach (var player in _players.Values)
    {
        await JS.InvokeVoidAsync("drawPlayer", _canvasRef,
            player.X, player.Y, player.Color, ...);
    }
}
```

**Points**:

- Call JavaScript rendering functions
- Draw each player and bullet
- Simple but effective!

**5. Input Handling** (1 min):

```csharp
[JSInvokable]
public void OnKeyDown(string key)
{
    if (key.Length == 1)
        _keys[key[0]] = true;
}
```

**Points**:

- `[JSInvokable]` allows JS to call this
- Track key state in array
- Used in game loop

**Transition**: "One more thing—mobile support..."

---

### Mobile Support (3 min) - Modern Touch

**Energy: MEDIUM-HIGH**

**Show the D-Pad Controls**:

```razor
<div class="mobile-controls">
    <div class="dpad-container">
        <button class="dpad-btn dpad-up"
                @ontouchstart="() => OnDirectionPress('W')"
                @ontouchend="() => OnDirectionRelease('W')">
            ▲
        </button>
        <!-- Left, Right, Down buttons... -->
    </div>
    <button class="shoot-button">🔫</button>
</div>
```

**Touch Handler**:

```csharp
private void OnDirectionPress(char key)
{
    _keys[key] = true;  // Set movement key active
}

private void OnDirectionRelease(char key)
{
    _keys[key] = false;  // Release movement key
}
```

**Key Point**:

> "Blazor handles touch events natively! We're using simple button presses - no complex joystick math. The D-pad gives players precise, responsive controls on mobile. And it's just C# - no JavaScript libraries needed!"

**Show Mobile Responsive Design**:

> "The canvas scales to full-screen on mobile devices, and we've added breakpoints for both tablets and phones. Everything from the HUD to the scoreboard adapts automatically using CSS media queries."

**Show Mobile Detection**:

```javascript
window.isMobileDevice = () => {
  return /Android|webOS|iPhone|iPad|iPod/.test(navigator.userAgent);
};
```

**Transition**: "Alright, enough coding. Let's run this thing!"

---

### Running & Testing (3 min) - The Payoff

**Energy: VERY HIGH** 🎉

**1. Start the Server**:

```powershell
cd Server
dotnet run
```

**While it starts**:

- "This compiles and runs the ASP.NET Core server"
- "It's hosting both the SignalR hub AND the Blazor WASM files"

**2. Get Your IP**:

```powershell
ipconfig
```

**Show your local IP**: e.g., `192.168.1.100`

**3. Open on Desktop**:

- Navigate to `http://localhost:5000`
- Enter name, click "Play Now"
- **Show that you're automatically matched into a room**

**4. Join from Phone**:

- Open browser on phone
- Go to `http://10.112.4.38:3000` (your IP on port 3000)
- Enter name, click "Play Now"
- Automatically joins your room!

**5. Play the Game!**:

- Move around on both devices
- Shoot each other
- Show health decreasing
- Die and respawn

**Audience Participation** (if possible):

> "If you're on the same WiFi, scan this QR code and join! Just click 'Play Now' and you'll automatically join the game. Let's see how many people can join!"

**Celebrate**:

> "And there we have it! A fully functional multiplayer game, browser-based, no plugins, working on desktop and mobile, built entirely in .NET!"

---

### Q&A (2-5 min) - Wrap Up

**Energy: CONVERSATIONAL**

**Common Questions**:

**Q: "Can this scale to 100+ players?"**  
A: "Not as-is. You'd need Redis backplane for SignalR, spatial partitioning for collisions, dedicated game servers. But for a LAN party? Absolutely!"

**Q: "What about production deployment?"**  
A: "Deploy to Azure App Service, AWS, or any container platform. SignalR works great in production. Just configure WebSocket support."

**Q: "How do you prevent lag?"**  
A: "Client-side prediction helps a lot. You can also interpolate other players' positions between updates. For pro games, look into 'lag compensation' techniques."

**Q: "Could you add [feature X]?"**  
A: "Yes! The code is on GitHub. Try adding powerups, different weapons, obstacles. It's a great learning project!"

**Q: "Why not use Unity?"**  
A: "Unity's great for complex games. But if you want a simple browser game that works everywhere—no downloads, no plugins—Blazor is perfect. Plus, you already know C#!"

**Closing Statement**:

> "The point of this talk isn't that you should build the next Fortnite in Blazor. It's to show what's _possible_ with modern .NET. Real-time communication, browser-based apps, mobile support—it's all there. So next time you have a real-time requirement, remember: .NET has you covered. Thank you!"

---

## 🎬 Backup Plans

### If Demo Fails

**Network Issues**:

- Have a pre-recorded video ready
- Show screenshots of gameplay
- Walk through code anyway—still valuable

**Build Errors**:

- Have a working build ready to copy from
- "This is why we test, folks!" (humor helps)

**No Volunteers Join**:

- Use your own phone
- "That's okay, I'll play with myself" (always gets a laugh)

### If Running Long

**Cut These Parts**:

- Detailed CSS explanation (just show it quickly)
- Some model explanations (combine into one)
- Extended Q&A (take questions offline)

**Speed Up**:

- Skip typing, just show pre-written code
- "I'll talk through this quickly..."

### If Running Short

**Extend With**:

- Show browser DevTools (WebSocket frames)
- Discuss architecture decisions in depth
- Live code a small feature (health bar color change)
- Deep dive into SignalR internals

---

## 💡 Pro Tips

### Presentation Style

1. **Use Analogies**:

   - "SignalR is like a walkie-talkie for your apps"
   - "Client-side prediction is like guessing where your friend will be"

2. **Engage Frequently**:

   - Ask questions (even rhetorical)
   - Make eye contact
   - React to audience reactions

3. **Celebrate Small Wins**:

   - "Look at that—first player joined!"
   - "Did you see how smooth that movement is?"

4. **Be Honest**:
   - "This isn't production-ready for 1000 players"
   - "There are better ways to do [X], but this is clear"

### Technical Tips

1. **Font Size**: Minimum 16pt in IDE
2. **Zoom Level**: 150-200% on browser
3. **Hide Distractions**: Close email, Slack, etc.
4. **Test on Projector**: Colors may look different
5. **Backup Internet**: Phone hotspot if conference WiFi fails

### Handling Problems

1. **Stay Calm**: "Huh, interesting. Let's debug this together!"
2. **Time Box It**: "I'll spend 2 minutes on this, then move on"
3. **Have Checkpoints**: Commit working versions you can revert to
4. **Embrace Errors**: "This is real development, folks!"

---

## 📝 Post-Talk Checklist

### Immediately After

- [ ] Share GitHub repo URL (Twitter, LinkedIn, conference chat)
- [ ] Thank attendees for participation
- [ ] Save any chat questions you didn't answer
- [ ] Take feedback notes while fresh

### Within 24 Hours

- [ ] Post slides/recording if available
- [ ] Answer questions on social media
- [ ] Write blog post expanding on key topics
- [ ] Reach out to anyone who had issues

### Within 1 Week

- [ ] Update GitHub repo with any improvements
- [ ] Create issues for suggested features
- [ ] Write retrospective on what worked/didn't
- [ ] Prepare improved version for next talk

---

## 🎯 Success Metrics

**You Nailed It If**:

- ✅ People joined your game from the audience
- ✅ At least 3 people asked questions
- ✅ Someone said "I want to try this!"
- ✅ GitHub repo gets stars
- ✅ You stayed in time (43-45 min)
- ✅ You had fun!

**Room for Improvement If**:

- ⚠️ Ran over/under by 5+ minutes
- ⚠️ Demo didn't work
- ⚠️ Audience seemed confused
- ⚠️ Too fast or too slow pace
- ⚠️ No questions (too clear or too unclear?)

---

## 🎁 Bonus: Variations for Different Audiences

### For Juniors

- Emphasize fundamentals: "What is SignalR?"
- Explain concepts clearly: "Client vs. Server"
- Show where to learn more
- Encourage experimentation

### For Seniors

- Discuss architecture decisions: "Why not event sourcing?"
- Compare to alternatives: "SignalR vs gRPC streams"
- Talk scaling considerations
- Invite critiques

### For Mixed Audience

- Start simple, layer complexity
- "For those new to SignalR..."
- "For experienced devs, note that..."
- Multiple levels of takeaways

---

**Good luck with your talk! You've got this! 🚀**
