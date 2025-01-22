using Rooms.Converters;
using System.Text.Json.Serialization;

namespace Rooms.Model
{
    public class Booking
    {
        public string HotelId { get; set; }

        [JsonConverter(typeof(FlatDateConverter))]
        public DateTime Arrival { get; set; }

        [JsonConverter(typeof(FlatDateConverter))]
        public DateTime Departure { get; set; }

        public string RoomType { get; set; }

        public string RoomRate { get; set; }
    }
}
