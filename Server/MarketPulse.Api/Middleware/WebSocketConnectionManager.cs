using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

namespace MarketPulse.Api.Middleware
{
    public class WebSocketConnectionManager
    {
        private readonly ConcurrentDictionary<string, WebSocket> _clients = new ConcurrentDictionary<string, WebSocket>();
        private readonly ILogger<WebSocketConnectionManager> _logger;
        public WebSocketConnectionManager(ILogger<WebSocketConnectionManager> logger)
        {
            _logger = logger;
        }
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
            _logger.LogInformation($"Change received: {message}. Broadcasting the change to {_clients.Count.ToString("n")}");

            foreach (var client in _clients.Values)
            {
                // Performance: we only broadcast the changes to the active clients
                if (client.State == WebSocketState.Open)
                {
                    try
                    {
                        await client.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);

                        _logger.LogTrace($"Message has been broadcasted to the client");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Error occurred when broadcasting the change to the client");
                    }
                }
            }
        }
    }
}