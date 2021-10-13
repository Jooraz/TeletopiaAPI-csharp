using FluentValidation;
using System.Linq;

namespace TeletopiaAPI.Models
{
    public class SendMessage
    {
        public UsernamePassword Auth { get; set; }

        public SingleMessage[] Messages { get; set; }
    }

    public class SingleMessage
    {
        public string ClientRef { get; set; }
        public string Tag { get; set; }
        public string Sender { get; set; }
        public SenderType SenderType { get; set; }
        /// <summary>
        /// 0-9, A-Za-z, &, #, ! characters recommended only
        /// </summary>
        public string TeletopiaAPI { get; set; }
        public string Recipient { get; set; }
        public string Gateway { get; set; }
        public string Expire { get; set; }
        /// <summary>
        /// RFC 3339
        /// </summary>
        /// <example>2015-11-27T22:54:33Z</example>
        /// <example>2015-11-27T22:54:33.231Z</example>
        /// <example>2015-11-27T22:54:33.231+01:00</example>
        public string SendTime { get; set; }
        public int? RequestDlr { get; set; }
        public int? Price { get; set; }
        public int? AgeLimit { get; set; }

        public SendMessageText ContentText { get; set; }
        public SendMessageTextSMS ContentSmsText { get; set; }
        public SendMessageBinarySMS ContentSmsBinary { get; set; }
    }

    public class SingleMessageValidator : AbstractValidator<SingleMessage>
    {
        private const string OneOptionalPropertyMessage = "Only one of ContentText, ContentSmsText or ContentSmsBinary can be set.";

        public SingleMessageValidator()
        {
            RuleFor(x => x.ClientRef.Length <= 100);
            RuleFor(x => x.Tag.Length <= 100);
            RuleFor(x => x.Recipient).NotNull();

            RuleFor(x => x)
                .Must(OnlyOneOfContentCanBeUsed)
                .WithMessage(OneOptionalPropertyMessage);
        }

        private bool OnlyOneOfContentCanBeUsed(SingleMessage obj)
        {
            return new[]
            {
                obj.ContentText is null,
                obj.ContentSmsText is null,
                obj.ContentSmsBinary is null
            }.Count(x => x) == 2;
        }
    }
}