using Core.Utilities.Formatting;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Core.Utilities.JsonConverters
{
    public class JsonDateOnlyConverter : JsonConverter<DateOnly>
    {
        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateOnly.Parse(reader.GetString()!);
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(Format.DateOnly));
        }
    }
}
