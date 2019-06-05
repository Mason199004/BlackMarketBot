using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using System.IO;
using Discord.Net.Rest;
using System.Collections.Generic;

namespace BlackMarketBot
{
    public class Program
    {
        
        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();
		public List<string> userClips = new List<string>();
        public async Task MainAsync()
        {
           
            var client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Debug,
                AlwaysDownloadUsers = true
            });

            string[] ClipsImport = File.ReadAllText("Clips.txt").Split('_');
			foreach (var e in ClipsImport)
			{
				userClips.Add(e);
			}
            
            

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
            
            if (wordArray[0] == "%ping")
            {
                await message.Channel.SendMessageAsync("Pong");
            }
            else if (wordArray[0] == "%help")
            {
                await message.Channel.SendMessageAsync("Commands: \n %ping \n %work \n %wallet");
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
					if (userClips.Contains(user.Id.ToString()))
					{

						int ind = userClips.IndexOf(user.Id.ToString());
						int sClip = Convert.ToInt32(userClips[ind + 1]);
						userClips[ind + 1] = Convert.ToString(sClip + profit);
					}
					else
					{
						userClips.Add(user.Id.ToString());
						userClips.Add(profit.ToString());
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
				int ind = userClips.IndexOf(user.Id.ToString());
				var msg = await message.Channel.SendMessageAsync("You Have " + userClips[ind + 1] + " clips");
				var emoji = new Emoji("📎");
				await msg.AddReactionAsync(emoji);
				
            }
            else if (wordArray[0] == "%mug")
            {
                int j = 0;
                int k = 0;

				//string idUserO = wordArray[1];
				//string idUserO1 = idUserO.Substring(3);
				//string idUserO2 = idUserO1.Remove(idUserO1.IndexOf('>'));
				//ulong idUserO3 = Convert.ToUInt64(idUserO2);

				int mind = userClips.IndexOf(user.Id.ToString());

				if (message.MentionedUsers.Count == 1)
				{
					foreach (var u in message.MentionedUsers)
					{
						var meind = userClips.IndexOf(u.Id.ToString());
						if (Convert.ToUInt32(userClips[mind + 1]) > 100 && Convert.ToUInt32(userClips[meind + 1]) > 100)
						{
							var rnd = new Random();
							int rand = rnd.Next(1, 10);
							if (rand > 7)
							{
								string sclips = Convert.ToString(Convert.ToInt32(userClips[meind + 1]) + 100);
								userClips[meind + 1] = sclips;
								sclips = Convert.ToString(Convert.ToInt32(userClips[mind + 1]) - 100);
								userClips[mind + 1] = sclips;
								await message.Channel.SendMessageAsync(u.Mention + " pulled a reverse card and mugged " + user.Mention);
							}
							else
							{
								string sclips = Convert.ToString(Convert.ToInt32(userClips[meind + 1]) - 100);
								userClips[j + 1] = sclips;
								sclips = Convert.ToString(Convert.ToInt32(userClips[mind + 1]) + 100);
								userClips[k + 1] = sclips;
								await message.Channel.SendMessageAsync(user.Mention + " mugged 100 clips from " + u.Mention);
							}
						}
					}
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

