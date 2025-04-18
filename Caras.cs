using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Text.Json.Serialization;

public class Cara
{

    private  int _vao;
    private  int _vbo;
    private  int _vertexCount;
    public List<Vertice> _vertices { get; set; } = new List<Vertice>();
    public Cara () { }
    public Cara(List<Vertice> vertices)
    {
        _vertices = vertices;
        _vertexCount = vertices.Count;
        
        InicializarBuffers();
    }

    public void InicializarBuffers()
    {
        // 1) Actualiza el conteo de vértices
        _vertexCount = _vertices.Count;

        // 2) Genera VAO/VBO
        _vao = GL.GenVertexArray();
        _vbo = GL.GenBuffer();

        GL.BindVertexArray(_vao);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);

        // 3) Configura los atributos
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(1);

        // 4) Desenlaza para evitar “fugas de estado”
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindVertexArray(0);
    }


    public void Dibujar(Vector3 origen)
    {
        // Convierte la lista de vértices en un float[]
        var vertexData = _vertices.SelectMany(v => v.ToArray(origen)).ToArray();

        // 1) Enlaza el VAO
        GL.BindVertexArray(_vao);

        // 2) Actualiza los datos en el VBO ligado al VAO
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(BufferTarget.ArrayBuffer,
                    vertexData.Length * sizeof(float),
                    vertexData,
                    BufferUsageHint.StreamDraw);

        // 3) Dibuja
        GL.DrawArrays(PrimitiveType.Lines, 0, _vertexCount);

        // 4) Limpia estado
        GL.BindVertexArray(0);
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
    }


}