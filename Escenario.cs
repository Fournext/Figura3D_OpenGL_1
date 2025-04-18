using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
public class Escenario
{
    public List<Objeto> _objetos { get; set; } = new List<Objeto>();
    public Vector3 CentroMasa { get; set; }

    private int _shaderProgram;
    public Escenario()
    {
    }
    public Escenario(Vector3 centroMasa, int shaderProgram)
    {
        CentroMasa = centroMasa;
        _shaderProgram = shaderProgram;
        _objetos = new List<Objeto>();
    }   

    public void AgregarObjeto(Objeto objeto)
    {
        _objetos.Add(objeto);
    }

    public void SetShaderProgram(int shaderProgram)
    {
        _shaderProgram = shaderProgram;
        ReinitializeBuffers();
    }

    private void ReinitializeBuffers()
    {
        foreach (var objeto in _objetos)
        {
            foreach (var parte in objeto._partes)
            {
                foreach (var cara in parte._caras)
                {
                    cara.InicializarBuffers();
                }
            }
        }
    }

    public void Dibujar()
    {
        //Console.WriteLine(_shaderProgram);
        Matrix4 view = Matrix4.LookAt(new Vector3(5, 5, 7), CentroMasa, Vector3.UnitY);
            
        Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 800f/600f, 0.1f, 100f);

        // Configurar matrices en el shader
        GL.UseProgram(_shaderProgram);

        // Dibujar todos los objetos
        foreach (var obj in _objetos)
        {
            obj.Dibujar(CentroMasa);
        }
    }
}