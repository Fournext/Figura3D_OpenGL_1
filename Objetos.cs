using OpenTK.Mathematics;
using System.Text.Json.Serialization;

public class Objeto
{
    [JsonPropertyName("partes")]
    public List<Parte> _partes { get; set; } = new List<Parte>();
    [JsonPropertyName("origenRelativo")]
    public Vector3 OrigenRelativo { get; set; }

    [JsonConstructor]
     public Objeto() { }
    public Objeto(Vector3 origenRelativo)
    {
        OrigenRelativo = origenRelativo;
    }

    public void AgregarParte(Parte parte)
    {
        _partes.Add(parte);
    }

    public void Dibujar(Vector3 centroMasa)
    {
        Vector3 origenAbsoluto = centroMasa + OrigenRelativo;
        
        foreach (var parte in _partes)
        {
            parte.Dibujar(origenAbsoluto);
        }
    }

     public void ReconstruirBuffers()
    {
        foreach (var parte in _partes)
        {
            parte.ReconstruirBuffers();
        }
    }
}