using System.Text.Json;
using System.Text.Json.Serialization;

namespace MTM_Inventory_Application.Core;

public class JsonColorConverter : JsonConverter<Color?>
{
    public override Color? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var colorString = reader.GetString();
        if (string.IsNullOrWhiteSpace(colorString)) return null;
        if (colorString.Equals("Transparent", StringComparison.OrdinalIgnoreCase))
            return Color.Transparent;
        return ColorTranslator.FromHtml(colorString);
    }

    public override void Write(Utf8JsonWriter writer, Color? value, JsonSerializerOptions options)
    {
        if (value.HasValue)
            writer.WriteStringValue(ColorTranslator.ToHtml(value.Value));
        else
            writer.WriteNullValue();
    }
}