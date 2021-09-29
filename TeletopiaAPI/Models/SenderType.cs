namespace TeletopiaAPI.Models
{
    public enum SenderType : uint
    {
        Unknown = 0,
        Shortcode = 1,
        Reserved = 2,
        National = 3,
        InternationalMSISDN = 4,
        Alphanumeric = 5
    }
}