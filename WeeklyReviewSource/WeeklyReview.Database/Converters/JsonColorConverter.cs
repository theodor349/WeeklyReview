using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WeeklyReview.Database.Converters
{
    public class JsonColorConverter : JsonConverter<Color>
    {
        public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                var argbValue = reader.GetInt32();
                return Color.FromArgb(argbValue);
            }

            throw new JsonException("Invalid color format in JSON.");
        }

        public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value.ToArgb());
        }
    }
}