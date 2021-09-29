namespace TeletopiaAPI.Models
{
    public enum DeliveryReportStatusCode
    {
        MessageInTransit = 1000,
        Delivered = 2000,

        // Billing Failed (PR messages only)
        BillingFailedReasonUnknown = 3000,
        SubscriberTemporairlyBarred = 3001,
        SubscriberInsufficientFunds = 3002,
        SubscriberTooYoung = 3003,

        // Barred (PR messages only)
        BarredReasonUnknown = 4000,
        BarredSubscriberAccount = 4001,
        SubscriberAccountClosed = 4002,
        SubscriberReservedOvercharged = 4003,

        AccessFailed = 5000,

        MessageRejected = 6000,
        InvalidRecipientMSISDN = 6001,
        InvalidSenderID = 6002,
        InvalidPrice = 6003,
        InvalidContent = 6004,

        MessageUndeliverable = 7000,
        MessageUnroutable = 7001,
        RejectedPayload = 7002,
        TechnocalError = 7003,
        UnknownSubscriber = 7005,

        Expired = 9000,
        PremiumRateTransactionRefunded = 9001
    }
}