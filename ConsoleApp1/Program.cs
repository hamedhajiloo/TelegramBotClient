using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TelegramBot;

namespace ConsoleApp1
{
    class Program
    {
        public static string token = "1852172683:AAGDoHdb_YjI7KqncQNuPKoG2CT4MeQz_tA";
        public static int lastUpdateId = 0;
        static async Task Main(string[] args)
        {
            var telegramBot = new TelegramBotClient(token);
            //await telegramBot.SendMessageAsync("120714541", "test from vafs!");

            while (true)
            {
                var res = await telegramBot.GetUpdatesAsync(lastUpdateId + 1);
                if (res.Any())
                {
                    lastUpdateId = res.Last().UpdateId;

                    foreach (var message in res)
                    {
                        if (message.Text == "سلام")
                        {
                            await telegramBot.SendMessageAsync(message.SenderChatId, "علیک");
                        }
                        else if (message.Text == "هوی")
                        {
                            await telegramBot.SendMessageAsync(message.SenderChatId, "های");
                        }
                        else
                        {
                            var sendMessage = $"{message.FullName} says {message.Text}";
                            await telegramBot.SendMessageAsync(message.SenderChatId, sendMessage);
                        }
                    }

                }

                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }
    }
}
