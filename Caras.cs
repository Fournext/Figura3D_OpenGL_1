using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Text.Json.Serialization;

public class Cara
{
    [JsonIgnore]
    private  int _vao;
    [JsonIgnore]
    private  int _vbo;
    private  int _vertexCount;
    [JsonPropertyName("vertices")]
    public List<Vertice> _vertices { get; set; } = new List<Vertice>();

    [JsonConstructor]
    public Cara() { }   
    public Cara(List<Vertice> vertices)
    {
        _vertices = vertices;
        _vertexCount = vertices.Count;
        
        // Configuración de buffers
        InicializarBuffers();
    }

    private void InicializarBuffers()
    {
        _vao = GL.GenVertexArray();
        _vbo = GL.GenBuffer();
        
        GL.BindVertexArray(_vao);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        
        // Configurar atributos
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);
        
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(1);
    }

    public void Dibujar(Vector3 origen)
    {
        var vertexData = _vertices.SelectMany(v => v.ToArray(origen)).ToArray();
        
        // Actualizar buffer y dibujar
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, vertexData.Length * sizeof(float), vertexData, BufferUsageHint.StreamDraw);

        GL.DrawArrays(PrimitiveType.Lines, 0, _vertexCount);
    }

    public void ReconstruirBuffer()
    {
        if (_vao != 0) GL.DeleteVertexArray(_vao);
        if (_vbo != 0) GL.DeleteBuffer(_vbo);

        _vao = GL.GenVertexArray();
        _vbo = GL.GenBuffer();

        GL.BindVertexArray(_vao);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);

        GL.EnableVertexAttribArray(0); // Posición
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
        
        GL.EnableVertexAttribArray(1); // Color
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));

        GL.BindVertexArray(0); // Importante!
    }
}