using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using OpenTK.Mathematics;

public class Vector3Converter : JsonConverter<Vector3>
{
    public override Vector3 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
        {
            var root = doc.RootElement;
            return new Vector3(
                root.GetProperty("x").GetSingle(),
                root.GetProperty("y").GetSingle(),
                root.GetProperty("z").GetSingle());
        }
    }

    public override void Write(Utf8JsonWriter writer, Vector3 value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber("x", value.X);
        writer.WriteNumber("y", value.Y);
        writer.WriteNumber("z", value.Z);
        writer.WriteEndObject();
    }
}