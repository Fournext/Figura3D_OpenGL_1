using OpenTK.Mathematics;
using System.Text.Json.Serialization;

public class Objeto
{
    public List<Parte> _partes { get; set; } = new List<Parte>();
    public Vector3 OrigenRelativo { get; set; }
    
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

}