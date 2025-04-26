
using System.Text.Json.Serialization;

public class Objeto
{
    [JsonPropertyName("partes")]
    public Dictionary<string, Parte> Partes { get; set; }
    
    [JsonPropertyName("x")]
    public float part_X { get; set; }
    
    [JsonPropertyName("y")]
    public float part_Y { get; set; }
    
    [JsonPropertyName("z")]
    public float part_Z { get; set; }

    public Objeto()
    {
        Partes = new Dictionary<string, Parte>();
    }

    public Objeto(Dictionary<string, Parte> partes, float x, float y, float z)
    {
        this.Partes = new Dictionary<string, Parte>();
        CrearCopia(partes);
        this.part_X = x;
        this.part_Y = y;
        this.part_Z = z;
    }

    public void Inicializar()
    {
        foreach (var parte in Partes.Values)
            parte.Inicializar();
    }
    
    private void CrearCopia(Dictionary<string, Parte> _partes)
    {
        foreach (var prt in _partes)
        {
            Partes.Add(prt.Key, prt.Value);
        }
    }
    
    public void Rotacion(float grado_X,float grado_Y,float grado_Z)
    {
        foreach (var parte in Partes.Values)
        {
            parte.Rotacion(grado_X, grado_Y, grado_Z);
        }
    }
    
    public void Escalacion(float N)
    {
        foreach (var parte in Partes.Values)
        {
            parte.Escalacion(N);
        }
    }
    
    public void Traslacion(float x, float y, float z)
    {
        foreach (var parte in Partes.Values)
        {
            parte.Traslacion(x, y, z);
        }
    }
    
    public void actualizarCentrosMasas(float x, float y, float z)
    {
        this.part_X += x;
        this.part_Y += y;
        this.part_Z += z;
        foreach (var parte in Partes.Values)
        {
            parte.actualizarCentrosMasas(part_X, part_Y, part_Z);
        }
    }

    public void RecalcularTransformaciones()
    {
        foreach (var parte in Partes.Values)
        {
            parte.RecalcularTransformaciones();
        }
    }


    public void Dibujar(int shaderProgram)
    {
        foreach (var parte in Partes.Values)
            parte.Dibujar(shaderProgram);
    }
}