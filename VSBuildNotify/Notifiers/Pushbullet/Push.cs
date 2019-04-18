using Newtonsoft.Json;

namespace VSBuildNotify.Notifiers.Pushbullet
{
    public class Push
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "body")]
        public string Body { get; set; }

        [JsonProperty(PropertyName = "device_iden")]
        public string TargetDevice { get; set; }
    }
}
