using OpenTK.Mathematics;

public class Parte
{
    public Dictionary<string, Cara> Caras { get; set; } = new Dictionary<string, Cara>();
    public Vector3 OrigenRelativo { get; set; }
    public Vector3 Rotacion { get; private set; }
    public Vector3 Posicion { get; private set; }  // Traslacion
    public Vector3 Escala { get; private set; } = Vector3.One; // Escalacion

    public Parte() { }
    public Parte(Vector3 origenRelativo)
    {
        OrigenRelativo = origenRelativo;
    }

    public void AgregarCara(string nombre, Cara cara)
    {
        Caras[nombre] = cara;
    }

    public Cara Cara(string nombreCara)
    {
        return Caras.TryGetValue(nombreCara, out var cara) ? cara : null;
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

    public void Dibujar(Vector3 centroMasa, Matrix4 transformacionPadre)
    {   
        Vector3 origen = OrigenRelativo + centroMasa;

    Matrix4 transformacionLocal = 
        Matrix4.CreateTranslation(-origen) *
        Matrix4.CreateScale(Escala) *
        Matrix4.CreateRotationX(Rotacion.X) *
        Matrix4.CreateRotationY(Rotacion.Y) *
        Matrix4.CreateRotationZ(Rotacion.Z) *
        Matrix4.CreateTranslation(origen + Posicion);

    Matrix4 transformacionCompleta = transformacionPadre * transformacionLocal;
        foreach (var cara in Caras.Values)
        {
            cara.Dibujar(OrigenRelativo + centroMasa, transformacionCompleta);
        }
    }
};