using System;
using System.Threading.Tasks;

namespace ChattApp2
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("===================================");
            Console.WriteLine("===== Välkommen till ChattApp =====");
            Console.WriteLine("===================================");
            Console.WriteLine();

            await Task.Delay(100);
            while (Console.KeyAvailable) Console.ReadKey(true);
            
            string username = "";
            while (string.IsNullOrWhiteSpace(username))
            {
                Console.Write("Ange ditt namn: ");
                username = Console.ReadLine()?.Trim() ?? "";
            }

            SocketManager.Username = username;

            Console.WriteLine($"\nHej {username}! Ansluter till servern...");
            await SocketManager.ConnectAsync();

            Console.WriteLine("\nSkriv ditt meddelande (skriv '/quit' för att avsluta):\n");

            bool running = true;
            while (running)
            {
                string input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) continue;

                if (input.Trim().ToLower() == "/quit")
                {
                    running = false;
                    continue;
                }

                await SocketManager.SendMessageAsync(input);
            }
            await SocketManager.DisconnectAsync();

            Console.WriteLine("Programmet avslutat. ");

        }

    }
}

