using System.Net.WebSockets;
using System.Text;

namespace MarketPulse.Api.Middleware
{

    public class WebSocketMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly WebSocketConnectionManager _connectionManager;

        public WebSocketMiddleware(RequestDelegate next, WebSocketConnectionManager connectionManager)
        {
            _next = next;
            _connectionManager = connectionManager;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Check if the request is for a WebSocket connection
            if (context.Request.Path == "/ws")
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    // Accept the WebSocket connection
                    WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();

                    // Generate a unique ID for this client
                    var clientId = Guid.NewGuid().ToString();

                    // Register the client WebSocket with the connection manager
                    _connectionManager.AddClient(clientId, webSocket);

                    // Handle the communication with this WebSocket client
                    await HandleWebSocketConnection(clientId, webSocket);

                    // Remove the client when the connection is closed
                    await _connectionManager.RemoveClient(clientId);
                }
                else
                {
                    // If not a WebSocket request, set a bad request response
                    context.Response.StatusCode = 400;
                }
            }
            else
            {
                // If not a WebSocket endpoint, proceed to the next middleware
                await _next(context);
            }
        }

        private async Task HandleWebSocketConnection(string clientId, WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];

            while (webSocket.State == WebSocketState.Open)
            {
                // Listen for messages from the client (optional)
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var clientMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    Console.WriteLine($"Received message from client {clientId}: {clientMessage}");

                    // Echo the message back to the client, or broadcast updates here if needed
                    var serverMessage = Encoding.UTF8.GetBytes($"Server received: {clientMessage}");
                    await webSocket.SendAsync(new ArraySegment<byte>(serverMessage), WebSocketMessageType.Text, true, CancellationToken.None);
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    // Handle client disconnecting
                    Console.WriteLine($"Client {clientId} disconnected.");
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connection closed by client", CancellationToken.None);
                }
            }
        }
    }
}