using System.Text.Json;
using System.Text.Json.Serialization;
using OpenTK.Mathematics;

public class Serializer
{
    private static readonly JsonSerializerOptions DefaultOptions = new()
    {
        WriteIndented = true,
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNameCaseInsensitive = true,
        Converters = { new OpenTKVector3Converter() }
    };

    public void GuardarAJson<T>(T objeto, string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("Ruta de archivo no válida", nameof(filePath));

        string json = JsonSerializer.Serialize(objeto, DefaultOptions);
        File.WriteAllText(filePath, json);
    }

    public T? CargarDesdeJson<T>(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException("Archivo no encontrado", filePath);

        string json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<T>(json, DefaultOptions);
    }
    
}

public class OpenTKVector3Converter : JsonConverter<Vector3>
{
    public override Vector3 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using JsonDocument doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;
        return new Vector3(
            root.GetProperty("X").GetSingle(),
            root.GetProperty("Y").GetSingle(),
            root.GetProperty("Z").GetSingle()
        );
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