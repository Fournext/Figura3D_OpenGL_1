using OpenTK.Mathematics;
using System.Text.Json.Serialization;
public class Vertice
{
    [JsonPropertyName("posicion")]
    public Vector3 Posicion { get; set; }
    
    [JsonPropertyName("color")]
    public Vector3 Color { get; set; }

    public Vertice() {} 

    public Vertice(float x, float y, float z, float r, float g, float b)
    {
        Posicion = new Vector3(x, y, z);
        Color = new Vector3(r, g, b);
    }
}