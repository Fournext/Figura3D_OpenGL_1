
using System.Text.Json.Serialization;
using OpenTK.Mathematics;
using OpenTKCubo3D;

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
    [JsonIgnore]
    public Vector3 centroDeMasa { get; set; }
    [JsonIgnore]
    public Transformaciones Transform { get; } = new Transformaciones();

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
        centroDeMasa = CalcularCentro();
        this.Traslacion(part_X,part_Y,part_Z);
        this.RecalcualarCentro();
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

    public Vector3 CalcularCentro(){
        var _vertices = Partes.Values.SelectMany(p => p.DicCaras.Values.SelectMany(c => c._vertices.Values));
        List<Vertice> todosvrt = new List<Vertice>();
        foreach (var lista in _vertices)
        {
            todosvrt.AddRange(lista);
        }
        return Vertice.CalcularCentro(todosvrt);
    }

    public void RecalcualarCentro(){
        centroDeMasa = CalcularCentro();
        foreach (var parte in Partes.Values)
                parte.RecalcualarCentro();
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
        Matrix4 matrizLocal = Transform.GetMatrix(centroDeMasa);
        Matrix4 matrizAcumulada = matrizLocal * matrizPadre;
        foreach (var parte in Partes.Values)
            parte.Dibujar(shaderProgram,matrizAcumulada);
    }
}