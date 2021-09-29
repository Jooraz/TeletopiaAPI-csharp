namespace TeletopiaAPI.Models
{
    public class ReceiveMessage
    {
        public string MessageId { get; set; }
        public string TeletopiaAPI { get; set; }
        public string Recipient { get; set; }
        public string Text { get; set; }
    }
}