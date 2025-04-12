using OpenTK.Mathematics;
using System.Text.Json.Serialization;
public class Parte
{
    [JsonPropertyName("caras")]
    public List<Cara> _caras { get; set; } = new List<Cara>();
    [JsonPropertyName("origenRelativo")]
    public Vector3 OrigenRelativo { get; set; }


    [JsonConstructor]
    public Parte() { }
    public Parte(Vector3 origenRelativo)
    {
        OrigenRelativo = origenRelativo;
    }

    public void AgregarCara(Cara cara)
    {
        _caras.Add(cara);
    }

    public void Dibujar(Vector3 origenObjeto)
    {
        Vector3 origenAbsoluto = origenObjeto + OrigenRelativo;
        
        foreach (var cara in _caras)
        {
            cara.Dibujar(origenAbsoluto);
        }
    }

    public void ReconstruirBuffers()
    {
        foreach (var cara in _caras)
        {
            cara.ReconstruirBuffer();
        }
    }
}