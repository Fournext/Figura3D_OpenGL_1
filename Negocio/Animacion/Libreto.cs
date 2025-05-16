using System.Text.Json.Serialization;
using OpenTK.Mathematics;

public class LibretoAnimacion
{
    [JsonIgnore] private Escenario escenario;
    [JsonInclude] public List<InstruccionAnimacion> Instrucciones { get; private set; } = new();
    [JsonIgnore] private Thread? hiloAnimacion;
    [JsonIgnore] private bool activo;

    [JsonIgnore] public float TiempoGlobal { get; set; } = 0f; // Lo setea Program.cs cada frame

    [JsonIgnore] private Dictionary<InstruccionAnimacion, float> progresoAnterior = new();

    public LibretoAnimacion() { }
    public LibretoAnimacion(Escenario escenario)
    {
        this.escenario = escenario;
    }

    public void AgregarInstruccion(InstruccionAnimacion instruccion)
    {
        Instrucciones.Add(instruccion);
    }

    public void CargarEscenario(Escenario escenario)
    {
        this.escenario = escenario;
    }

    public void Iniciar()
    {
        if (activo || Instrucciones.Count == 0)
            return;

        activo = true;

        progresoAnterior.Clear(); // Limpieza garantizada
        foreach (var inst in Instrucciones)
        {
            progresoAnterior[inst] = 0f;
        }

        hiloAnimacion = new Thread(() =>
        {
            while (activo)
            {
                foreach (var inst in Instrucciones)
                {
                    if (!escenario.Objetos.TryGetValue(inst.NombreObjeto, out var obj))
                        continue;

                    if (TiempoGlobal < inst.TiempoInicio)
                        continue;

                    float tiempoDesdeInicio = TiempoGlobal - inst.TiempoInicio;
                    float tActual = tiempoDesdeInicio / inst.TiempoFin;
                    tActual = Math.Clamp(tActual, 0f, 1f);

                    if (!progresoAnterior.TryGetValue(inst, out float tAnterior))
                        tAnterior = 0f; // Seguridad extrema adicional por si acaso

                    float deltaT = tActual - tAnterior;

                    if (deltaT <= 0f)
                        continue;

                    switch (inst.Tipo)
                    {
                        case TipoTransformacion.Trasladar:
                            Vector3 desplazamientoTotal = new Vector3(inst.X, inst.Y, inst.Z);
                            Vector3 deltaDesplazamiento = desplazamientoTotal * deltaT;
                            obj.Transform.Transladate(deltaDesplazamiento.X, deltaDesplazamiento.Y, deltaDesplazamiento.Z);
                            break;

                        case TipoTransformacion.Escalar:
                            float escalaDestino = inst.X;
                            float escalaAnterior = MathHelper.Lerp(1f, escalaDestino, tAnterior);
                            float escalaActual = MathHelper.Lerp(1f, escalaDestino, tActual);
                            float escalaDelta = escalaActual / escalaAnterior;
                            obj.Transform.Escalate(escalaDelta);
                            break;

                        case TipoTransformacion.Rotar:
                            Vector3 rotTotal = new Vector3(inst.X, inst.Y, inst.Z);
                            Vector3 deltaRot = rotTotal * deltaT;
                            obj.Transform.Rotate(deltaRot.X, deltaRot.Y, deltaRot.Z);
                            break;
                    }

                    progresoAnterior[inst] = tActual;
                }

                Thread.Sleep(1);
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
    }

    [JsonIgnore] public bool EstaActivo => activo;
}
