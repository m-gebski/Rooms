namespace Rooms.Utilities
{
    public static class DateUtils
    {
        public static bool DateRangeOverlaps(DateTime start1, DateTime end1, DateTime start2, DateTime end2)
        {
            return (start2 >= start1 && start2 < end1) ||
                   (end2 > start1 && end2 <= end1) ||
                   (start2 < start1 && end2 > end1);
        }

        public static bool DateIntersectsRange(DateTime date, DateTime rangeStart, DateTime rangeEnd)
        {
            return date >= rangeStart && date <= rangeEnd;
        }
    }
}
