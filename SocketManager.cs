using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using SocketIOClient;

namespace ChattApp2
{

    internal class SocketManager
    {
        public static string Username { get; set; } = "Okänd";
        private static SocketIOClient.SocketIO _client;
        private static readonly string Path = "/sys25d";
        private static readonly string MessageEvent = "yassine_message";
        private static readonly string JoinEvent = "user_joined";
        private static readonly string LeaveEvent = "user_left";

        public static List<string> MessageHistory = new();

        public static async Task ConnectAsync()
        {
            _client = new SocketIOClient.SocketIO("wss://api.leetcode.se", new SocketIOOptions{ Path = Path });
            
            _client.On(MessageEvent, response =>
            {
                var data = response.GetValue<Message>();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{data.Time} {data.Sender}: {data.Text}");
                Console.ResetColor();
            });

            _client.On(JoinEvent, response =>
            {
                string joineUser = response.GetValue<string>();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"*** {joineUser} har anslutit till chatten ***");
                Console.ResetColor();
            });

            _client.OnConnected += (sender, args) =>
            {
                Console.WriteLine("Connected till servern!");
            };

            _client.OnDisconnected += (sender, args) =>
            {
                Console.WriteLine("Disconnected från servern!");
            };

            await _client.ConnectAsync();
            await Task.Delay(350);
            await _client.EmitAsync(JoinEvent, Username);
        }

        public static async Task SendMessageAsync(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;
            var message = new Message
            {
                Sender = Username,
                Text = text,
                Time = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
            };
            await _client.EmitAsync(MessageEvent, message);
        }

        public static async Task DisconnectAsync()
                
        {
            
            await _client.EmitAsync(LeaveEvent, Username);
            await Task.Delay(350);
            await _client.DisconnectAsync();
            Console.WriteLine("Du har lämnat chatten.");
        }
    }
}
