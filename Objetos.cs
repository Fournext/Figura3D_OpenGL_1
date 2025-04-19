using OpenTK.Mathematics;

public class Objeto
{
    public Dictionary<string, Parte> Partes { get; set; } = new Dictionary<string, Parte>();
    public Vector3 OrigenRelativo { get; set; }
    public Vector3 Rotacion { get; private set; }
    public Vector3 Posicion { get; private set; }  // Traslacion
    public Vector3 Escala { get; private set; } = Vector3.One; // Escalacion
    
    public Objeto(Vector3 origenRelativo)
    {
        OrigenRelativo = origenRelativo;
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
    public Parte Parte(string nombreParte) => Partes.TryGetValue(nombreParte, out var parte) ? parte : null;

    public void AgregarParte(string nombre, Parte parte)
    {
        Partes[nombre] = parte;
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

        foreach (var parte in Partes.Values)
        {
            // ESTA es la línea crucial:
            parte.Dibujar(OrigenRelativo + centroMasa + Posicion, transformacionCompleta);
        }
    }


};