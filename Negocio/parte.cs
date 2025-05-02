using System.Text.Json.Serialization;
using OpenTK.Mathematics;
using OpenTKCubo3D;

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

    [JsonIgnore]
    public Vector3 centroDeMasa { get; set; }
    [JsonIgnore]
    public Transformaciones Transform { get; } = new Transformaciones();
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
        centroDeMasa = CalcularCentro();
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

    public Vector3 CalcularCentro(){
        var _vertices = DicCaras.Values.SelectMany(c => c._vertices.Values);
        List<Vertice> todosvrt = new List<Vertice>();
        foreach (var lista in _vertices)
        {
            todosvrt.AddRange(lista);
        }
        return Vertice.CalcularCentro(todosvrt);
    }

    public void RecalcualarCentro(){
        centroDeMasa = CalcularCentro();
        foreach (var cara in DicCaras.Values)
                cara.RecalcualarCentro();
    }
    

    public void Dibujar(int shaderProgram,Matrix4 matrizPadre)
    {
         Matrix4 matrizLocal = Transform.GetMatrix(centroDeMasa);
        Matrix4 matrizAcumulada = matrizLocal * matrizPadre;
        foreach (var cara in DicCaras.Values)
            cara.Dibujar(shaderProgram,matrizAcumulada);
    }
}