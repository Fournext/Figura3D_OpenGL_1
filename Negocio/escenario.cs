using System.Text.Json.Serialization;
using System.Collections.Generic;
using OpenTK.Mathematics;
using OpenTKCubo3D;

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
    [JsonIgnore]
    public Vector3 centroDeMasa { get; set; }
    [JsonIgnore]
    public Transformaciones Transform { get; } = new Transformaciones();

    
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
        this.Traslacion(esc_X,esc_Y,esc_Z);
        this.RecalcualarCentro();
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

    public Escenario Clonar()
    {
        var objetosClonados = new Dictionary<string, Objeto>();
        foreach (var kv in Objetos)
        {
            objetosClonados[kv.Key] = kv.Value.Clonar();
        }

        return new Escenario(objetosClonados, esc_X, esc_Y, esc_Z);
    }


    public Vector3 CalcularCentro(){
        var _vertices = Objetos.Values.SelectMany(obj => obj.Partes.Values.SelectMany(p => p.DicCaras.Values.SelectMany(c => c._vertices.Values)));
        List<Vertice> todosvrt = new List<Vertice>();
        foreach (var lista in _vertices)
        {
            todosvrt.AddRange(lista);
        }
        return Vertice.CalcularCentro(todosvrt);
    }

    public void RecalcualarCentro(){
        centroDeMasa = CalcularCentro();
        foreach (var obj in Objetos.Values)
                obj.RecalcualarCentro();
    }

    
    public void Rotacion(float grado_X,float grado_Y,float grado_Z)
    {
        Transform.RotateA(centroDeMasa, grado_X, grado_Y, grado_Z);
    }
    
    public void Escalacion(float N)
    {
        Transform.Posicion -= centroDeMasa;
        Transform.Escalate(N);
        Transform.Posicion += centroDeMasa;
    }
    
    public void Traslacion(float x, float y, float z)
    {
        Transform.Transladate(x, y, z);
    }
    

    public void Dibujar(int shaderProgram,Matrix4 matrizPadre)
    {
        var matrizLocal = Transform.GetMatrix(centroDeMasa);
        Matrix4 matrizAcumulada = matrizLocal * matrizPadre;
        foreach (var obj in Objetos.Values)
            obj.Dibujar(shaderProgram,matrizAcumulada);
    }
}