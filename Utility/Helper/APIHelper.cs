using Domain;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Utility.Helper;

namespace Utility.Helper
{
    public class APIHelper
    {
        private readonly static string channelToken = ConfigHelper.GetInstance().GetAppSettingValue("channelToken");

        public static async Task<HttpResponseMessage> ReplyMessage(ReplyModel reply)
        {
            using (HttpClient client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(reply);
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {channelToken}");
                return await client.PostAsync("https://api.line.me/v2/bot/message/reply",
                    new StringContent(json, Encoding.UTF8, "application/json"));
            }
        }
    }
}