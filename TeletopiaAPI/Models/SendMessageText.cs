using System.Text.Json.Serialization;

namespace TeletopiaAPI.Models
{
    public class SendMessageText
    {
        [JsonPropertyName("dcs")]
        public int DCS { get; set; }
        public string Text { get; set; }
        public int RecodeText { get; set; }
    }
}