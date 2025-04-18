using OpenTK.Mathematics;

public class Vertice
{
    public Vector3 PosicionRelativa { get; }
    public Vector3 Color { get; }

    public Vertice(Vector3 posicionRelativa, Vector3 color)
    {
        PosicionRelativa = posicionRelativa;
        Color = color;
    }

    public float[] ToArray(Vector3 origen)
    {
        Vector3 posAbsoluta = origen + PosicionRelativa;
        return new float[] 
        {
            posAbsoluta.X, posAbsoluta.Y, posAbsoluta.Z,
            Color.X, Color.Y, Color.Z
        };
    }
}