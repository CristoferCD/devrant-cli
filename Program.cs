using System;
using devRantNetCore;
using System.Threading.Tasks;
using System.Net.Http;

namespace devrant_cli
{
    class Program
    {
        private static RantProducer producer = new RantProducer();
        static async Task Main(string[] args)
        {
            while(true) {
                var rant = await producer.GetNextRantAsync();
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("\n\t" + rant.UserName + "\n");
                Console.ResetColor();
                Console.WriteLine(rant.Content);
                var key = Console.ReadKey();
                switch(key.Key) {
                    case ConsoleKey.Spacebar:
                        continue;
                    case ConsoleKey.C:
                        await loadComments(rant);
                        break;
                    case ConsoleKey.Q:
                        return;
                }
            }
        }

        private static async Task loadComments(Rant rant)
        {
            var comments = await producer.GetComments(rant);
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("\n\t\tComments:\n");
            for (int i = 0; i < rant.NumComments; i++) {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("\tUser: " + comments[i].UserName);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("\t\t" + comments[i].Content + "\n");
                switch(Console.ReadKey().Key) {
                    case ConsoleKey.Spacebar:
                        break;
                    case ConsoleKey.Q:
                        return;
                }
            }
        }
    }
}
