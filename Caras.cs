using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

public class Cara
{
    private int _vao;
    private int _vbo;
    private int _vertexCount;
    public Dictionary<string, Vertice> Vertices { get; set; } = new Dictionary<string, Vertice>();
    public Vector3 Rotacion { get; private set; } // Rotacion
    public Vector3 Posicion { get; private set; }  // Traslacion
    public Vector3 Escala { get; private set; } = Vector3.One; //Escalacion
    public Cara() { }
    
    public Cara(Dictionary<string, Vertice> vertices)
    {
        Vertices = vertices;
        _vertexCount = vertices.Count;
        InicializarBuffers();
    }

    public void Rotar(float anguloX, float anguloY, float anguloZ)
    {
        Rotacion += new Vector3(anguloX, anguloY, anguloZ);
    }
    public void Trasladar(float x, float y, float z)
    {
        Posicion += new Vector3(x, y, z);
    }
    public void Escalar(float N)
    {
        if(N!=0){
            Escala *= new Vector3(N, N, N);
        }else{
           Console.WriteLine("Error al mandar los puntos");
        }
    }

    public void InicializarBuffers()
    {
        _vertexCount = Vertices.Count;
        _vao = GL.GenVertexArray();
        _vbo = GL.GenBuffer();

        GL.BindVertexArray(_vao);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);

        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(1);

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindVertexArray(0);
    }

    public void Dibujar(Vector3 origen,Matrix4 transformacionPadre)
    {
        
        Matrix4 escala = Matrix4.CreateScale(Escala);
        Matrix4 rotacion = Matrix4.CreateRotationX(Rotacion.X) * 
                        Matrix4.CreateRotationY(Rotacion.Y) * 
                        Matrix4.CreateRotationZ(Rotacion.Z);
        Matrix4 traslacion = Matrix4.CreateTranslation(Posicion+origen);
        Matrix4 transformacionCompleta = transformacionPadre * traslacion * rotacion * escala;

        var vertexData = Vertices.Values.SelectMany(v => 
    {
        Vector4 posTransformada = new Vector4(v.PosicionRelativa, 1.0f) * transformacionCompleta;
            return new float[] 
            {
                posTransformada.X, posTransformada.Y, posTransformada.Z,
                v.Color.X, v.Color.Y, v.Color.Z
            };
        }).ToArray();

        GL.BindVertexArray(_vao);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(BufferTarget.ArrayBuffer,vertexData.Length * sizeof(float),vertexData,BufferUsageHint.StreamDraw);

        GL.DrawArrays(PrimitiveType.Lines, 0, _vertexCount);

        GL.BindVertexArray(0);
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
    }
};
