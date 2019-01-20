using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using System.IO;

namespace BlackMarketBot
{
    public class Program
    {
        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();
        public int[] userClips;
        public async Task MainAsync()
        {
            var client = new DiscordSocketClient();

            client.Log += Log;
            client.MessageReceived += Client_MessageReceived;
            string token = File.ReadAllText("token.txt"); 
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private async Task Client_MessageReceived(SocketMessage message)
        {
            var user = message.Author;
            var Author = message.Author.ToString();
            var wordArray = message.Content.Split(" ");
            if (wordArray[0] == "%ping")
            {
                await message.Channel.SendMessageAsync("Pong");
            }
            else if (wordArray[0] == "%help")
            {
                await message.Channel.SendMessageAsync("Commands: \n %ping");
            }
            else if (wordArray[0] == "%work")
            {
                if (wordArray[1].ToLower() == "pen")
                {
                    var rng = new Random();
                    int randPen = rng.Next(1, 5);
                    int profit = randPen * 2;
                    await message.Channel.SendMessageAsync("You sold " + randPen + " pen(s) and earned " + profit + " clips");

                }
            }
            
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}

