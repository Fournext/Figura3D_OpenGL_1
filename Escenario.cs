using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

public class Escenario
{
    public Dictionary<string, Objeto> Objetos { get; set; } = new Dictionary<string, Objeto>();
    public Vector3 CentroMasa { get; set; }
    public Vector3 Rotacion { get; private set; }
    public Vector3 Posicion { get; private set; }  // Traslacion
    public Vector3 Escala { get; private set; } = Vector3.One; //Escalacion

    private int _shaderProgram;
    
    public Escenario() { }
    
    public Escenario(Vector3 centroMasa, int shaderProgram)
    {
        CentroMasa = centroMasa;
        _shaderProgram = shaderProgram;
    }   

    public void AgregarObjeto(string nombre, Objeto objeto)
    {
        Objetos[nombre] = objeto;
    }

    public void SetShaderProgram(int shaderProgram)
    {
        _shaderProgram = shaderProgram;
        ReinitializeBuffers();
    }

    private void ReinitializeBuffers()
    {
        foreach (var objeto in Objetos.Values)
        {
            foreach (var parte in objeto.Partes.Values)
            {
                foreach (var cara in parte.Caras.Values)
                {
                    cara.InicializarBuffers();
                }
            }
        }
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
    public Objeto Objeto(string nombre) => Objetos.TryGetValue(nombre, out var obj) ? obj : null;
    public void Dibujar()
    {
        GL.UseProgram(_shaderProgram);
        Matrix4 escala = Matrix4.CreateScale(Escala);
        Matrix4 rotacion = Matrix4.CreateRotationX(Rotacion.X) 
                        * Matrix4.CreateRotationY(Rotacion.Y) 
                        * Matrix4.CreateRotationZ(Rotacion.Z);
        Matrix4 traslacion = Matrix4.CreateTranslation(Posicion + CentroMasa);
        
        Matrix4 transformacionGlobal = traslacion * rotacion * escala;
        


        Matrix4 view = Matrix4.LookAt(new Vector3(5, 5, 7), CentroMasa + Posicion, Vector3.UnitY);
        Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 800f/600f, 0.1f, 100f);
        
        GL.UniformMatrix4(GL.GetUniformLocation(_shaderProgram, "view"), false, ref view);
        GL.UniformMatrix4(GL.GetUniformLocation(_shaderProgram, "projection"), false, ref projection);
        GL.UniformMatrix4(GL.GetUniformLocation(_shaderProgram, "rotacionGlobal"), false, ref transformacionGlobal);

        foreach (var obj in Objetos.Values)
        {
            obj.Dibujar(CentroMasa+Posicion,transformacionGlobal);
        }
    }
};