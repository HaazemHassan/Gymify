using Gymify.Application.Common.Enums;

namespace Gymify.Application.Common.Helpers
{
    public static class DateTimeHelper
    {
        public static DateTimeOffset GetStartDate(TimePeriod period, DateTimeOffset now)
        {
            return period switch
            {
                TimePeriod.Today => now.Date,
                TimePeriod.ThisWeek => now.AddDays(-(int)now.DayOfWeek),
                TimePeriod.ThisMonth => new DateTimeOffset(now.Year, now.Month, 1, 0, 0, 0, TimeSpan.Zero),
                _ => now.Date
            };
        }
    }
}
