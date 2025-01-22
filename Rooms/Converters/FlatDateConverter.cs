using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Rooms.Converters
{
    public class FlatDateConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.ParseExact(reader.GetString()!, Consts.DateFormat, CultureInfo.InvariantCulture);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(Consts.DateFormat, CultureInfo.InvariantCulture));
        }
    }
}
