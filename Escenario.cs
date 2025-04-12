using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
public class Escenario
{
    [JsonPropertyName("objetos")]
    public List<Objeto> _objetos { get; set; } = new List<Objeto>();
    [JsonPropertyName("centroMasa")]
    public Vector3 CentroMasa { get; set; }


    [JsonIgnore]
    private int _shaderProgram;

    [JsonConstructor]
    public Escenario()
    {
        _objetos = new List<Objeto>();
        CentroMasa = Vector3.Zero;
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

    public void Dibujar()
    {
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

    public void GuardarAJson(string filePath)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters = { new Vector3Converter() },
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
        
        string jsonString = JsonSerializer.Serialize(this, options);
        File.WriteAllText(filePath, jsonString);
    }

    public static Escenario CargarDesdeJson(string filePath, int shaderProgram)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException("Archivo no encontrado", filePath);

        string jsonString = File.ReadAllText(filePath);

        var options = new JsonSerializerOptions
        {
            Converters = { new Vector3Converter() },
            PropertyNameCaseInsensitive = true
        };

        var escenario = JsonSerializer.Deserialize<Escenario>(jsonString, options);

        if (escenario == null)
            throw new InvalidOperationException("Error al deserializar el archivo");

        escenario.SetShaderProgram(shaderProgram);
        escenario.ReconstruirBuffers();

        return escenario;
    }

    public void ReconstruirBuffers()
    {
        foreach (var obj in _objetos)
        {   
            obj.ReconstruirBuffers();
        }
    }

    public void SetShaderProgram(int shaderProgram)
    {
        _shaderProgram = shaderProgram;
    }
}