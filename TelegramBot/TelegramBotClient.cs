using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot
{
    public class TelegramBotClient
    {
        private readonly string _token;
        private readonly HttpClient client;

        public TelegramBotClient(string token)
        {
            _token = token ?? throw new ArgumentNullException(nameof(token));
            client = new HttpClient();
        }

        public async Task SendMessageAsync(string chatId, string text, string parsmode = "Html")
        {
            var url = $"https://api.telegram.org/bot{_token}/sendMessage";

            var message = new
            {
                chat_id = chatId,
                text = text,
                parse_mode = parsmode
            };

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json")
            };

            await client.SendAsync(httpRequest);
        }

        public async Task<List<Message>> GetUpdatesAsync(int offset)
        {
            var updates = new List<Message>();
            var url = $"https://api.telegram.org/bot{_token}/getUpdates";

            var message = new
            {
                offset = offset,
            };

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json")
            };

            var result = await client.SendAsync(httpRequest);

            var response = await result.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(response);

            var status = json["ok"].ToString();
            if (!bool.Parse(status)) return updates;

            var resultUpdates = json["result"].ToList();

            foreach (var resultUpdate in resultUpdates)
            {
                var messageJson = resultUpdate["message"];
                var message1 = new Message
                {
                    UpdateId = int.Parse(resultUpdate["update_id"].ToString()),
                    Id = int.Parse(messageJson["message_id"].ToString()),
                    Text = messageJson["text"].ToString(),
                    SenderChatId = messageJson["from"]["id"].ToString(),
                    FullName = $"{messageJson["from"]["first_name"]} {messageJson["from"]["last_name"]}"
                };

                updates.Add(message1);
            }

            return updates;
        }


        public class Message
        {
            public int UpdateId { get; set; }
            public int Id { get; set; }
            public string Text { get; set; }
            public string SenderChatId { get; set; }
            public string FullName { get; set; }
        }
    }
}
