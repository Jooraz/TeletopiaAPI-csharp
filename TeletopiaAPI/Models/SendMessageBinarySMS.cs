using System.Text.Json.Serialization;

namespace TeletopiaAPI.Models
{
    public class SendMessageBinarySMS
    {
        [JsonPropertyName("dcs")]
        public int DCS { get; set; }

        [JsonPropertyName("pid")]
        public int PID { get; set; }

        [JsonPropertyName("udh")]
        public string UDH { get; set; }

        [JsonPropertyName("ud")]
        public int UD { get; set; }
    }
}