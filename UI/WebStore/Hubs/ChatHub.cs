using Microsoft.AspNetCore.SignalR;

namespace WebStore.Hubs;

public class ChatHub : Hub
{
    private readonly ILogger<ChatHub> _Logger;

    public ChatHub(ILogger<ChatHub> Logger) => _Logger = Logger;

    public async Task SendMessage(string Message)
    {
        _Logger.LogInformation("Message {0}", Message);
        await Clients.Others.SendAsync("MessageFromServer", Message);
    }
}
