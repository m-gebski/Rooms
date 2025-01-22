using Rooms.Model;
using Rooms.Utilities;

namespace Rooms.Commands
{
    public class Search : BasicCommand, ICommand
    {
        public Dictionary<DateTime, (DateTime date, int free)> ResultSeries { get; set; } = [];

        public void Execute(string inputParams, IEnumerable<Hotel> hotelData, IEnumerable<Booking> bookingData)
        {
            if (TryGetParameters(inputParams, out var parsedParameters))
            {
                var queriedHotel = hotelData.FirstOrDefault(h => h.Id.Equals(parsedParameters.hotelId, StringComparison.InvariantCultureIgnoreCase));
                if (queriedHotel == null)
                {
                    return;
                }

                var queriedRoomCount = queriedHotel.Rooms.Count(r => r.RoomType.Equals(parsedParameters.roomType, StringComparison.InvariantCultureIgnoreCase));
                var allOverlapingBookings = bookingData.Where(b => b.HotelId.Equals(parsedParameters.hotelId, StringComparison.InvariantCultureIgnoreCase) &&
                                                             b.RoomType.Equals(parsedParameters.roomType, StringComparison.InvariantCultureIgnoreCase) &&
                                                             DateUtils.DateRangeOverlaps(parsedParameters.startDate, parsedParameters.endDate, b.Arrival, b.Departure))
                                                    .OrderBy(b => b.Arrival);

                var daysBetween = (parsedParameters.endDate - parsedParameters.startDate).Days;

                if (daysBetween == 0)
                {
                    var count = queriedRoomCount - allOverlapingBookings.Count();
                    ResultSeries.Add(parsedParameters.startDate, (parsedParameters.endDate, count));
                    return;
                }

                var bookingCounts = new Dictionary<DateTime, int>();
                for (int i = 0; i < daysBetween; i++)
                {
                    var dateBookingCount = allOverlapingBookings.Count(b => DateUtils.DateIntersectsRange(parsedParameters.startDate.AddDays(i), b.Arrival, b.Departure));
                    bookingCounts.Add(parsedParameters.startDate.AddDays(i), dateBookingCount);
                }

                var isSeriesOpen = false;
                foreach((DateTime date, int count) in bookingCounts)
                {
                    var freeCount = queriedRoomCount - count;

                    if (isSeriesOpen)
                    {
                        var openSeries = ResultSeries.Last();

                        if (openSeries.Value.free == freeCount)
                        {
                            ResultSeries[openSeries.Key] = (date, freeCount);
                            continue;
                        }
                        else
                        {
                            isSeriesOpen = false;
                        }
                    }

                    if (!isSeriesOpen && !ResultSeries.ContainsKey(date))
                    {
                        ResultSeries.Add(date, (date, freeCount));
                        isSeriesOpen = true;
                        continue;
                    }
                }
            }
        }

        public string GetFormattedOutput()
        {
            var formattedOutput = new List<string>();
            foreach (var series in ResultSeries)
            {
                var dateString = series.Key != series.Value.date ? $"{series.Key:yyyyMMdd}-{series.Value.date:yyyyMMdd}" : $"{series.Key:yyyyMMdd}";
                formattedOutput.Add($"({dateString}, {series.Value.free})");
            }

            return string.Join(", ", formattedOutput);
        }

        internal override (DateTime start, DateTime end) ParseDateParameters(string rawValue)
        {
            var today = DateTime.Today;

            var daysAhead = int.Parse(rawValue);

            return (today, today.AddDays(daysAhead));
        }
    }
}
