//TODO Step 01 -- Add signalR nuget dependency - Microsoft.AspNetCore.SignalR.Client
//TODO Step 02 -- Add Chat Hub folder and class 
//using Microsoft.AspNetCore.SignalR;

//namespace Demo.Hubs
//{
//    public class ChatHub : Hub ///base class for a SignalR hub
//    {
//        public async Task SendGlobalMessage(string user, string message)
//        {
//            await Clients.All.SendAsync("ReceiveMessage", user, message);
//        }

//        public async Task SendRoomMessage(string user, string message, string roomName)
//        {
//            await Clients.Group(roomName).SendAsync("ReceiveMessage", user, message);

//        }

//        public async Task JoinRoom(string user, string roomName)
//        {
//            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
//            await Clients.Group(roomName).SendAsync("SystemMessage", user + " joined the room.");
//        }

//        public async Task LeaveRoom(string user, string roomName)
//        {
//            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
//            await Clients.Group(roomName).SendAsync("SystemMessage", user + " left the room.");
//        }
//    }
//}
