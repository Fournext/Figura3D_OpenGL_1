using System.Diagnostics;
using System.Text.Json.Serialization;
using OpenTK.Mathematics;

public class LibretoAnimacion
{
    [JsonIgnore] private Escenario escenario;
    [JsonIgnore] private Queue<InstruccionAnimacion> colaInstrucciones = new();
    [JsonInclude] public List<InstruccionAnimacion> Instrucciones { get; private set; } = new(); // Para serializar
    [JsonIgnore] private Thread? hiloAnimacion;
    [JsonIgnore] private bool activo;
    [JsonIgnore] private Stopwatch reloj;
    [JsonIgnore] private bool loop;

     public LibretoAnimacion() {}
    public LibretoAnimacion(Escenario escenario)
    {
        this.escenario = escenario;
    }

    public void AgregarInstruccion(InstruccionAnimacion instruccion)
    {
        colaInstrucciones.Enqueue(instruccion);
        Instrucciones.Add(instruccion);
    }

    public void InicializarColaDesdeJSON()
    {
        colaInstrucciones = new Queue<InstruccionAnimacion>(Instrucciones);
    }

    public void CargarEscenario(Escenario escenario)
    {
        this.escenario = escenario;
    }

    public void Iniciar()
    {
        if (activo || colaInstrucciones.Count == 0)
            return;

        activo = true;
        reloj = new Stopwatch();
        reloj.Start();

        hiloAnimacion = new Thread(() =>
        {
            try
            {

                while (activo && colaInstrucciones.Count > 0)
                {
                    var inst = colaInstrucciones.Peek();
                    float duracion = inst.TiempoFin - inst.TiempoInicio;
                    float tiempoInicio = reloj.ElapsedMilliseconds / 1000f;

                    if (!escenario.Objetos.TryGetValue(inst.NombreObjeto, out var obj))
                        break;

                    Vector3 origen = Vector3.Zero;
                    Vector3 destino = Vector3.Zero;
                    Vector3 acumulado = Vector3.Zero; // Para Rotacion

                    switch (inst.Tipo)
                    {
                        case TipoTransformacion.Trasladar:
                            origen = obj.Transform.Posicion;
                            destino = origen + new Vector3(inst.X, inst.Y, inst.Z);
                            break;
                        case TipoTransformacion.Escalar:
                            origen = new Vector3(1);
                            destino = new Vector3(inst.X);
                            break;
                        case TipoTransformacion.Rotar:
                            destino = new Vector3(inst.X, inst.Y, inst.Z);
                            break;
                    }

                    while (activo)
                    {
                        float tiempoActual = reloj.ElapsedMilliseconds / 1000f;
                        float t = (tiempoActual - tiempoInicio) / duracion;
                        t = Math.Clamp(t, 0f, 1f);

                        Vector3 valorInterpolado = Vector3.Lerp(origen, destino, t);

                        switch (inst.Tipo)
                        {
                            case TipoTransformacion.Trasladar:
                                var deltaT = valorInterpolado - obj.Transform.Posicion;
                                obj.Traslacion(deltaT.X, deltaT.Y, deltaT.Z);
                                break;
                            case TipoTransformacion.Escalar:
                                float escalaActual = obj.Transform.Escala.X;
                                float escalaDestino = valorInterpolado.X;
                                float factorEscala = escalaDestino / escalaActual;
                                obj.Escalacion(factorEscala);
                                break;
                            case TipoTransformacion.Rotar:
                                var deltaR = valorInterpolado - acumulado;
                                obj.Rotacion(deltaR.X, deltaR.Y, deltaR.Z);
                                acumulado = valorInterpolado;
                                break;
                        }

                        if (t >= 1f) break;
                        Thread.Sleep(16);
                    }

                    colaInstrucciones.Dequeue();
                }
            }
            finally
            {
                activo = false;
                reloj.Stop();
            }
        });

        hiloAnimacion.IsBackground = true;
        hiloAnimacion.Start();
    }

    public void Detener()
    {
        if (!activo) return;

        activo = false;
        hiloAnimacion?.Join();
        hiloAnimacion = null;
        reloj?.Stop();
    }

    [JsonIgnore] public bool EstaActivo => activo;
}
