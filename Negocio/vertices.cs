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

    public static Vector3 CalcularCentro(List<Vertice> vert)
    {
        float minX = float.MaxValue, maxX = float.MinValue;
        float minY = float.MaxValue, maxY = float.MinValue;
        float minZ = float.MaxValue, maxZ = float.MinValue;

        bool hayPuntos = false;
        foreach (var p in vert)
        {
            if (p.Posicion.X < minX) minX = p.Posicion.X;
            if (p.Posicion.X > maxX) maxX = p.Posicion.X;
            if (p.Posicion.Y < minY) minY = p.Posicion.Y;
            if (p.Posicion.Y > maxY) maxY = p.Posicion.Y;
            if (p.Posicion.Z < minZ) minZ = p.Posicion.Z;
            if (p.Posicion.Z > maxZ) maxZ = p.Posicion.Z;

            hayPuntos = true;
        }
        if(!hayPuntos) return Vector3.Zero;

        return new Vector3((minX + maxX) / 2, (minY + maxY) / 2, (minZ + maxZ) / 2);

    }         
}