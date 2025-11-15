using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketIOClient;

namespace ChattApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            Console.WriteLine("=================================");
            Console.WriteLine("===== Välkommen till ChatApp ====");
            Console.WriteLine("=================================");
            Console.WriteLine();

            string username = "";
            while (string.IsNullOrWhiteSpace(username))
            {
                Console.Write("Ange ditt namn: ");
                Console.Out.Flush();
                username = Console.ReadLine();
                Console.WriteLine();
            }

            SocketManager.Username = username;

            Console.WriteLine($"\nHej {username}! Ansluter till servern...");
            await SocketManager.ConnectAsync();

            Console.WriteLine("\nSkriv ditt meddelande eller skriv '/quit' för att avsluta:\n");

            bool running = true;
            while (running)
            {
                string input = Console.ReadLine();
                if (input == null)
                    continue;
                if (input.Trim().ToLower() == "/quit")
                {
                    running = false;
                    break;
                }

                string message = $"{username}: {input}";
                var time = DateTime.Now.ToString("HH:mm");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{time} {message}");
                Console.ResetColor();

                await SocketManager.SendMessageAsync(message);
            }

            await SocketManager.DisconnectAsync();
            Console.WriteLine("Programmet avslutat.");
        }
    }
}
