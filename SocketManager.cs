using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SocketIOClient;

namespace ChattApp
{
    internal class SocketManager
    {
        public static string Username { get; set; } = "Okänd";
        private static SocketIOClient.SocketIO _client;
        private static readonly string path = "/sys25d";
        private static readonly string EventName = "Yassine88_message";
        public static List<string> Message = new List<string>();

        public static async Task ConnectAsync()
        {

            _client = new SocketIOClient.SocketIO("wss://api.leetcode.se", new SocketIOOptions
            {
                Path = path
            });

            _client.On(EventName, response =>
            {
                string receiveMessage = response.GetValue<string>();
                Message.Add(receiveMessage);

                var time = DateTime.Now.ToString("HH:mm");
                Console.WriteLine($"{time} [Server] {receiveMessage}");
            });

            _client.OnConnected += async (sender, args) =>
            {
                Console.WriteLine("Connected");
                await Task.CompletedTask;
            };
            _client.OnDisconnected += async (sender, args) =>
            {
                Console.WriteLine("Disconnected");
                await Task.CompletedTask;
            };

            await _client.ConnectAsync();
            await Task.Delay(200);
            Console.WriteLine($"Connected: {_client.Connected}");

        }

        public static async Task SendMessageAsync(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return;
            await _client.EmitAsync(EventName, message);
        }

        public static async Task DisconnectAsync()
        {
            await _client.DisconnectAsync();
            Console.WriteLine("Du har lämnat chatten.");
        }
    }
}
