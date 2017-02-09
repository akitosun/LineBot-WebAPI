using Domain;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Utility.Helper;

namespace LineBot.Controllers
{
    public class LineController : ApiController
    {
        private string channelSecret = ConfigHelper.GetInstance().GetAppSettingValue("channelSecret");

        // POST: api/Line
        public async Task<HttpResponseMessage> Post()
        {
            try
            {
                var signature = Request.Headers.GetValues("X-Line-Signature").FirstOrDefault();
                var body = await Request.Content.ReadAsStringAsync();
                var cryptoResult = SHA256Crypto(body);
                if (signature == cryptoResult)
                {
                    var value = JsonConvert.DeserializeObject<WebhookModel>(body);
                    var handler = Factory.CreateLineHandler();
                    await handler.ProcessMessage(value);
                }
                else
                {
                    // signature not valid

                    var response = Request.CreateResponse(HttpStatusCode.Unauthorized);
                    return response;
                }
            }
            catch (Exception ex)
            {
                // handle exception
                var response = Request.CreateResponse(HttpStatusCode.BadRequest);
                return response;
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        private string SHA256Crypto(string text)
        {
            using (HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(channelSecret)))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(text));
                return Convert.ToBase64String(hash);
            }
        }
    }
}