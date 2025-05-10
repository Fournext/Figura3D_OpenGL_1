public class InstruccionAnimacion
{
    public string NombreObjeto { get; set; }
    public TipoTransformacion Tipo { get; set; }
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }

    public float TiempoInicio { get; set; }
    public float TiempoFin { get; set; }

    public InstruccionAnimacion(){}

    public InstruccionAnimacion(string nombre, TipoTransformacion tipo, float x, float y, float z, float tInicio, float tFin)
    {
        NombreObjeto = nombre;
        Tipo = tipo;
        X = x;
        Y = y;
        Z = z;
        TiempoInicio = tInicio;
        TiempoFin = tFin;
    }
}
