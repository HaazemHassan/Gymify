namespace Gymify.Infrastructure.Common.Options
{
    public class EmailVerificationCodeOptions
    {
        public const string SectionName = "VerificationCodeOptions";

        public int EmailExpireInMinutes { get; set; }
        public int CodeLength { get; set; }
    }
}
