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
        public string[] userClips = new string[21474836];
        public async Task MainAsync()
        {
            for (long i = 0; i < userClips.Length; i++)
            {
                userClips[i] = "";
            }
            
            
            string ClipsImport = File.ReadAllText("Clips.txt");
            userClips = ClipsImport.Split('_');
            
            var client = new DiscordSocketClient();

            client.Log += Log;
            client.MessageReceived += Client_MessageReceived;
            string token = File.ReadAllText("token.txt"); 
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();
            GC.Collect();
            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private async Task Client_MessageReceived(SocketMessage message)
        {
            var client = new DiscordSocketClient();
            var user = message.Author;
            var Author = message.Author.ToString();
            var wordArray = message.Content.Split(" ");
            var guild = message.Source;
            if (wordArray[0] == "%ping")
            {
                await message.Channel.SendMessageAsync("Pong");
            }
            else if (wordArray[0] == "%help")
            {
                await message.Channel.SendMessageAsync("Commands: \n %ping \n %work");
            }
            else if (wordArray[0] == "%work")
            {
                if (wordArray[1].ToLower() == "pen")
                {
                    var rng = new Random();
                    int randPen = rng.Next(1, 5);
                    int profit = randPen * 2;
                    await message.Channel.SendMessageAsync("You sold " + randPen + " pen(s) and earned " + profit + " clips");
                    bool hap = false;
                    for (int i = 0; i < userClips.Length; i++)
                    {
                        if (userClips[i].Contains(Author))
                        {
                            int sClip = Convert.ToInt32(userClips[i + 1]);
                            userClips[i + 1] = Convert.ToString(sClip + profit);
                            hap = true;
                        }
                        else if (!hap && userClips[i] == "")
                        {
                            userClips[i] = Author;
                            userClips[i + 1] = profit.ToString();
                            hap = true;
                        }
                    }
                    string clips = string.Join("_", userClips);
                    if (!File.Exists("Clips.txt"))
                    {
                        File.Create("Clips.txt");
                    }
                    File.WriteAllText("Clips.txt", clips);
                    
                }
            }
            else if (wordArray[0] == "%wallet")
            {
                for (int i = 0; i < userClips.Length; i++)
                {
                    if (userClips[i] == Author)
                    {
                        var msg = await message.Channel.SendMessageAsync("You have " + userClips[i + 1] + " clips");
                        var emoji = new Emoji("📎");
                        await msg.AddReactionAsync(emoji);
                        
                        
                        
                    }
                }
            }
            else if (wordArray[0] == "%mug")
            {
                
                
            }
            
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}

