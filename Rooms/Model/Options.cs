using CommandLine;

namespace Rooms.Model
{
    public class Options
    {
        [Option("hotels", Required = true)]
        public string HotelFile { get; set; } = string.Empty;

        [Option("bookings", Required = true)]
        public string BookingFile { get; set; } = string.Empty;
    }
}
