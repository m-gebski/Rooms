using Rooms.Model;
using Rooms.Utilities;
using System.Globalization;

namespace Rooms.Commands
{
    public class Availability : BasicCommand, ICommand
    {
        public int AvailabilityResult { get; set; } = int.MinValue;

        public void Execute(string inputParams, IEnumerable<Hotel> hotelData, IEnumerable<Booking> bookingData)
        {
            if(TryGetParameters(inputParams, out var parsedParameters))
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
                
                if(daysBetween == 0)
                {
                    AvailabilityResult = queriedRoomCount - allOverlapingBookings.Count();
                    return;
                }

                var bookingCounts = new int[daysBetween];
                for(int i = 0; i < daysBetween; i++)
                {
                    bookingCounts[i] = allOverlapingBookings.Count(b => DateUtils.DateIntersectsRange(parsedParameters.startDate.AddDays(i), b.Arrival, b.Departure));
                }

                AvailabilityResult = queriedRoomCount - bookingCounts.Max();
            }
        }

        public string GetFormattedOutput()
        {
            if(AvailabilityResult > int.MinValue)
            {
                return AvailabilityResult.ToString();
            }

            return string.Empty;
        }

        internal override (DateTime start, DateTime end) ParseDateParameters(string rawValue)
        {
            (DateTime, DateTime) result;

            if (rawValue.Contains('-'))
            {
                var dates = rawValue.Split('-');
                result.Item1 = DateTime.ParseExact(dates[0], Consts.DateFormat, CultureInfo.InvariantCulture);
                result.Item2 = DateTime.ParseExact(dates[1], Consts.DateFormat, CultureInfo.InvariantCulture);
            }
            else
            {
                result.Item1 = DateTime.ParseExact(rawValue, Consts.DateFormat, CultureInfo.InvariantCulture);
                result.Item2 = DateTime.ParseExact(rawValue, Consts.DateFormat, CultureInfo.InvariantCulture);
            }

            return result;
        }
    }
}
