using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

public class Serialicer
{
    private static readonly JsonSerializerOptions DefaultOptions = new()
    {
        WriteIndented = true,
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNameCaseInsensitive = true,
        Converters = { new OpenTKVector3Converter() } // Añade el convertidor aquí
    };

    public void GuardarAJson<T>(T objeto, string filePath)
    {
        string json = JsonSerializer.Serialize(objeto, DefaultOptions);
        File.WriteAllText(filePath, json);
    }

    public T CargarDesdeJson<T>(string filePath)
    {
        string json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<T>(json, DefaultOptions);
    }
}