using Microsoft.AspNetCore.SignalR;

namespace EquipmentManagementPlatform.API.Websockets
{
    public class NotificationHub : Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("StateUpdate", message);
        }
    }
}
