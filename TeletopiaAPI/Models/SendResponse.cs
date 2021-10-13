namespace TeletopiaAPI.Models
{
    public class SendResponsesArray
    {
        public SendResponse[] Responses { get; set; }
    }

    public class SendResponse
    {
        public bool Accepted { get; set; }
        public string ClientRef { get; set; }
        public string MessageId { get; set; }
        public string Recipient { get; set; }
        public DeliveryReportStatusCode StatusCode { get; set; }
        public string StatusDescription { get; set; }
    }
}