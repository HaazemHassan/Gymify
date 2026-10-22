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

        public static (DateTimeOffset Start, DateTimeOffset End) GetMonthDateRange(int year, int month)
        {
            var start = new DateTimeOffset(year, month, 1, 0, 0, 0, TimeSpan.Zero);
            var end = start.AddMonths(1);
            return (start, end);
        }

        public static (DateTimeOffset Start, DateTimeOffset End) GetYearDateRange(int year)
        {
            var start = new DateTimeOffset(year, 1, 1, 0, 0, 0, TimeSpan.Zero);
            var end = start.AddYears(1);
            return (start, end);
        }
    }
}
