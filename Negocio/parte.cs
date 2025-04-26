using System.Text.Json.Serialization;

public class Parte
{
    [JsonPropertyName("caras")]
    public Dictionary<string, Cara> DicCaras { get; set; }

    [JsonPropertyName("x")]
    public float parte_X { get; set; }

    [JsonPropertyName("y")]
    public float parte_Y { get; set; }

    [JsonPropertyName("z")]
    public float parte_Z { get; set; }

    public Parte()
    {
        DicCaras = new Dictionary<string, Cara>();
    }

    public Parte(Dictionary<string, Cara> caras, float x, float y, float z)
    {
        this.DicCaras = new Dictionary<string, Cara>();
        CrearCopia(caras);
        this.parte_X = x;
        this.parte_Y = y;
        this.parte_Z = z;
    }

    public void Inicializar()
    {
        foreach (var cara in DicCaras.Values)
            cara.Inicializar();
    }
    
    private void CrearCopia(Dictionary<string, Cara> caras)
    {
        foreach (var cr in caras)
        {
            DicCaras.Add(cr.Key, cr.Value);
        }
    }

    public void Rotacion(float grado_X,float grado_Y,float grado_Z)
    {
        foreach (var cara in DicCaras.Values)
        {
            cara.Rotacion(grado_X, grado_Y, grado_Z);
        }
    }

    public void Escalacion(float N)
    {
        foreach (var cara in DicCaras.Values)
        {
            cara.Escalacion(N);
        }
    }

    public void Traslacion(float x, float y, float z)
    {
        foreach (var cara in DicCaras.Values)
        {
            cara.Traslacion(x, y, z);
        }
    }
    public void actualizarCentrosMasas(float x, float y, float z)
    {
        this.parte_X += x;
        this.parte_Y += y;
        this.parte_Z += z;
        foreach (var cara in DicCaras.Values)
        {
            cara.actualizarCentrosMasas(parte_X, parte_Y, parte_Z);
        }
    }  

    public void RecalcularTransformaciones()
    {
        foreach (var cara in DicCaras.Values)
        {
            cara.RecalcularTransformaciones();
        }
    }
  

    public void Dibujar(int shaderProgram)
    {
        foreach (var cara in DicCaras.Values)
            cara.Dibujar(shaderProgram);
    }
}