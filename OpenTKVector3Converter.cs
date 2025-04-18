using System.Text.Json;
using System.Text.Json.Serialization;
using OpenTK.Mathematics;

public class OpenTKVector3Converter : JsonConverter<Vector3>
{
    public override Vector3 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException();

        Vector3 result = new();
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                return result;

            if (reader.TokenType != JsonTokenType.PropertyName)
                throw new JsonException();

            string propName = reader.GetString();
            reader.Read();
            
            switch (propName?.ToLower())
            {
                case "x":
                    result.X = reader.GetSingle();
                    break;
                case "y":
                    result.Y = reader.GetSingle();
                    break;
                case "z":
                    result.Z = reader.GetSingle();
                    break;
            }
        }
        
        throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, Vector3 value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber("X", value.X);
        writer.WriteNumber("Y", value.Y);
        writer.WriteNumber("Z", value.Z);
        writer.WriteEndObject();
    }
}