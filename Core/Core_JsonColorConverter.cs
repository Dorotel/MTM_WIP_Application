using System.Text.Json;
using System.Text.Json.Serialization;

namespace MTM_Inventory_Application.Core;

/// <summary>
/// JSON converter for nullable Color types
/// </summary>
public class JsonColorConverter : JsonConverter<Color?>
{
    #region JsonConverter Implementation

    /// <summary>
    /// Read Color value from JSON
    /// </summary>
    /// <param name="reader">JSON reader</param>
    /// <param name="typeToConvert">Type being converted</param>
    /// <param name="options">Serializer options</param>
    /// <returns>Nullable Color value</returns>
    public override Color? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var colorString = reader.GetString();
        if (string.IsNullOrWhiteSpace(colorString)) return null;
        if (colorString.Equals("Transparent", StringComparison.OrdinalIgnoreCase))
            return Color.Transparent;
        return ColorTranslator.FromHtml(colorString);
    }

    /// <summary>
    /// Write Color value to JSON
    /// </summary>
    /// <param name="writer">JSON writer</param>
    /// <param name="value">Color value to write</param>
    /// <param name="options">Serializer options</param>
    public override void Write(Utf8JsonWriter writer, Color? value, JsonSerializerOptions options)
    {
        if (value.HasValue)
            writer.WriteStringValue(ColorTranslator.ToHtml(value.Value));
        else
            writer.WriteNullValue();
    }

    #endregion
}