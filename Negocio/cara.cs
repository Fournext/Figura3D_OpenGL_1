using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
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
    private int _vao, _vbo;
    [JsonIgnore]
    private Matrix4 _modelo;


    public Cara()
    {
        _vertices = new Dictionary<string, List<Vertice>>();
        _modelo = Matrix4.Identity;
    }


    public Cara(string name, List<Vertice> vertices, float x, float y, float z)
    {
        _vertices = new Dictionary<string, List<Vertice>>();
        Convertir_a_Vertices(vertices, name);
        this.cara_X = x;
        this.cara_Y = y;
        this.cara_Z = z;
        _modelo = Matrix4.Identity;
    }

    public void Traslacion(float x, float y, float z)
    {   
        _modelo = _modelo * Matrix4.CreateTranslation(x, y, z);
    }
    public void Rotacion(float grado_X,float grado_Y,float grado_Z)
    {

        Matrix4 rotacion = Matrix4.Identity;

        rotacion =  Matrix4.CreateRotationX(MathHelper.DegreesToRadians(grado_X))*
                    Matrix4.CreateRotationY(MathHelper.DegreesToRadians(grado_Y))*
                    Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(grado_Z));
        _modelo = rotacion * _modelo;
    }
    public void Escalacion(float N)
    {
        if(N!=0)
            _modelo = Matrix4.CreateScale(N, N, N) * _modelo;
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


    public void actualizarCentrosMasas(float x, float y, float z)
    {
        this.cara_X += x;
        this.cara_Y += y;
        this.cara_Z += z;
        _modelo = Matrix4.CreateTranslation(cara_X, cara_Y, cara_Z);
    }

    public void RecalcularTransformaciones()
    {
        _modelo = Matrix4.CreateTranslation(cara_X , cara_Y , cara_Z);
    }


    public void Dibujar(int shaderProgram)
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

        GL.UseProgram(shaderProgram);
        GL.BindVertexArray(_vao);

        int modelLocation = GL.GetUniformLocation(shaderProgram, "model");
        GL.UniformMatrix4(modelLocation, false, ref _modelo);

        GL.DrawArrays(PrimitiveType.Triangles, 0, verticesCombinados.Length / 6);
        GL.BindVertexArray(0);
    }

}

