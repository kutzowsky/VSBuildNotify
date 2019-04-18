using System;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using VSBuildNotify.DTO;

namespace VSBuildNotify.Notifiers.Pushbullet
{
    public class PushbulletClient
    {
        public Uri BaseUri { get; set; } = new Uri("https://api.pushbullet.com/v2/");
        public string AuthToken { get; set; }

        public PushbulletClient(string authToken)
        {
            AuthToken = authToken;
        }

        public void PushTo(string targetDeviceId, Notification notification)
        {
            var push = new Push
            {
                type = "note",
                title = notification.Title,
                body = notification.Body,
                device_iden = targetDeviceId
            };

            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseUri;

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Access-Token", AuthToken);

                var content = new StringContent(JsonConvert.SerializeObject(push), Encoding.UTF8, "application/json");
                var result = client.PostAsync("pushes", content).Result;

                if (!result.IsSuccessStatusCode) throw new Exception(result.Content.ReadAsStringAsync().Result);
            }
        }
    }
}
