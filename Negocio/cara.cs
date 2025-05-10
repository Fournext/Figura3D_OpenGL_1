using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTKCubo3D;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Text.Json.Serialization;

public class Cara
{
    [JsonPropertyName("vertices")]
    public Dictionary<string, List<Vertice>> _vertices { get; set; }

    [JsonPropertyName("x")]
    public float cara_X { get; set; }

    [JsonPropertyName("y")]
    public float cara_Y { get; set; }

    [JsonPropertyName("z")]
    public float cara_Z { get; set; }

    [JsonIgnore]
    public Vector3 centroDeMasa { get; set; }
    [JsonIgnore]
    public Transformaciones Transform { get; } = new Transformaciones();

    [JsonIgnore]
    private int _vao, _vbo;


    public Cara()
    {
        _vertices = new Dictionary<string, List<Vertice>>();
    }


    public Cara(string name, List<Vertice> vertices, float x, float y, float z)
    {
        _vertices = new Dictionary<string, List<Vertice>>();
        Convertir_a_Vertices(vertices, name);
        this.cara_X = x;
        this.cara_Y = y;
        this.cara_Z = z;
        centroDeMasa = CalcularCentro();
    }

    public void Traslacion(float x, float y, float z)
    {   
        Transform.Transladate(x,y,z);
    }
    public void Rotacion(float grado_X,float grado_Y,float grado_Z)
    {
        Transform.RotateA(centroDeMasa,grado_X,grado_Y,grado_Z);
    }
    public void Escalacion(float N)
    {
        Transform.Posicion -= centroDeMasa;
        Transform.Escalate(N);
        Transform.Posicion += centroDeMasa;
    }
    private void Convertir_a_Vertices(List<Vertice> vertices, string nombre)
    {
        _vertices[nombre] = vertices;
    }


    public void Inicializar()
    {
        _vao = GL.GenVertexArray();
        _vbo = GL.GenBuffer();

        List<float> todosVertices = new List<float>();
        foreach (var listaDeVertices in _vertices.Values)
        {
            foreach (var vertice in listaDeVertices)
            {
                todosVertices.Add(vertice.Posicion.X);
                todosVertices.Add(vertice.Posicion.Y);
                todosVertices.Add(vertice.Posicion.Z);
                todosVertices.Add(vertice.Color.X);
                todosVertices.Add(vertice.Color.Y);
                todosVertices.Add(vertice.Color.Z);
            }
        }
        float[] verticesCombinados = todosVertices.ToArray();

        GL.BindVertexArray(_vao);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, verticesCombinados.Length * sizeof(float), verticesCombinados, BufferUsageHint.StaticDraw);

        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(1);

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindVertexArray(0);
    }

    public Vector3 CalcularCentro(){
        List<Vertice> todosvrt = new List<Vertice>();
        foreach (var lista in _vertices.Values)
        {
            todosvrt.AddRange(lista);
        }
        return Vertice.CalcularCentro(todosvrt);
    }

    public void RecalcualarCentro(){
        centroDeMasa = CalcularCentro();
    }

    public Cara Clonar()
    {
        var copiaVertices = new Dictionary<string, List<Vertice>>();
        foreach (var kv in _vertices)
        {
            copiaVertices[kv.Key] = kv.Value.Select(v => v.Clonar()).ToList();
        }

        var clon = new Cara();
        clon._vertices = copiaVertices;
        clon.cara_X = cara_X;
        clon.cara_Y = cara_Y;
        clon.cara_Z = cara_Z;
        clon.RecalcualarCentro(); // opcional

        return clon;
    }

    public void Dibujar(int shaderProgram,Matrix4 matrizAcumulada)
    {
        // Combinar todos los vértices del diccionario en un solo array
        List<float> todosVertices = new List<float>();
        foreach (var listaDeVertices in _vertices.Values)
        {
            foreach (var vertice in listaDeVertices)
            {
                todosVertices.Add(vertice.Posicion.X);
                todosVertices.Add(vertice.Posicion.Y);
                todosVertices.Add(vertice.Posicion.Z);
                todosVertices.Add(vertice.Color.X);
                todosVertices.Add(vertice.Color.Y);
                todosVertices.Add(vertice.Color.Z);
            }
        }
        float[] verticesCombinados = todosVertices.ToArray();
        

        Matrix4 matrizLocal = Transform.GetMatrix(centroDeMasa);
        Matrix4 matrizFinal = matrizLocal * matrizAcumulada;


        GL.UseProgram(shaderProgram);
        GL.BindVertexArray(_vao);

        int modelLocation = GL.GetUniformLocation(shaderProgram, "model");
        GL.UniformMatrix4(modelLocation, false, ref matrizFinal);

        GL.DrawArrays(PrimitiveType.Triangles, 0, verticesCombinados.Length / 6);
        GL.BindVertexArray(0);
    }

}

