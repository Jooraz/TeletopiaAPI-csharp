using System.Text.Json.Serialization;

namespace TeletopiaAPI.Models
{
    public class DeliveryReportParameters
    {
        public string MessageId { get; set; }
        public string ClientRef { get; set; }
        public int Delivered { get; set; }
        public DeliveryReportStatusCode StatusCode { get; set; }
        public string Time { get; set; }
        public double Timestamp { get; set; }

        [JsonPropertyName("mcc")]
        public string MCC { get; set; }

        [JsonPropertyName("mnc")]
        public string MNC { get; set; }
        public int PartCount { get; set; }
    }
}