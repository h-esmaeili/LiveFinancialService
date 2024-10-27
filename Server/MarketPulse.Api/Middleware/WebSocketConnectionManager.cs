using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

namespace MarketPulse.Api.Middleware
{
    public class WebSocketConnectionManager
    {
        private readonly ConcurrentDictionary<string, WebSocket> _clients = new ConcurrentDictionary<string, WebSocket>();

        public void AddClient(string clientId, WebSocket socket)
        {
            _clients.TryAdd(clientId, socket);
        }

        public async Task RemoveClient(string clientId)
        {
            if (_clients.TryRemove(clientId, out var socket))
            {
                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connection closed", CancellationToken.None);
            }
        }

        public async Task BroadcastToClientsAsync(string message)
        {
            var buffer = Encoding.UTF8.GetBytes(message);

            foreach (var client in _clients.Values)
            {
                if (client.State == WebSocketState.Open)
                {
                    await client.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }
    }
}
