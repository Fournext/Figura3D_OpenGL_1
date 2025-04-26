using System.Text.Json.Serialization;
using System.Collections.Generic;

public class Escenario
{
    [JsonPropertyName("objetos")]
    public Dictionary<string, Objeto> Objetos { get; set; }
    
    [JsonPropertyName("x")]
    public float esc_X { get; set; }
    
    [JsonPropertyName("y")]
    public float esc_Y { get; set; }
    
    [JsonPropertyName("z")]
    public float esc_Z { get; set; }

    
    public Escenario()
    {
        Objetos = new Dictionary<string, Objeto>();
    }

    public Escenario(Dictionary<string, Objeto> objetos, float x, float y, float z)
    {
        this.Objetos = new Dictionary<string, Objeto>();
        CrearCopia(objetos);
        this.esc_X = x;
        this.esc_Y = y;
        this.esc_Z = z;
        actualizarCentrosMasas();
    }

    public void Inicializar()
    {
        foreach (var obj in Objetos.Values)
            obj.Inicializar();
    } 
    
    private void CrearCopia(Dictionary<string, Objeto> objetos)
    {
        foreach (var obj in objetos)
        {
            Objetos.Add(obj.Key, obj.Value);
        }
    }

    private void actualizarCentrosMasas(){
        foreach (var obj in this.Objetos.Values)
        {
            obj.actualizarCentrosMasas(esc_X, esc_Y, esc_Z);
        }
    }

    public void RecalcularTransformaciones()
    {
        foreach (var obj in Objetos.Values)
        {
            obj.RecalcularTransformaciones();
        }
    }

    
    public void Rotacion(float grado_X,float grado_Y,float grado_Z)
    {
        foreach (var item in Objetos.Values)
        {
            item.Rotacion(grado_X, grado_Y, grado_Z);
        }
    }
    
    public void Escalacion(float N)
    {
        foreach (var obj in Objetos.Values)
        {
            obj.Escalacion(N);
        }
    }
    
    public void Traslacion(float x, float y, float z)
    {
        foreach (var obj in Objetos.Values)
        {
            obj.Traslacion(x, y, z);
        }
    }
    

    public void Dibujar(int shaderProgram)
    {
        foreach (var obj in Objetos.Values)
            obj.Dibujar(shaderProgram);
    }
}