using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenTKCubo3D
{
    class Program : GameWindow
    {
        private float _angleY; 
        private float _angleX; 
        
        private Matrix4 _view;
        private Matrix4 _projection;
        private Escenario? _escenario;
        private Escenario? _escenario2;
        Serialicer serialicer = new Serialicer();

        public static int ShaderProgram { get; private set; }

        public Program(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }


        public Action<int?> OnLoadCustom;
        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(0.1f, 0.1f, 0.2f, 1.0f);
            GL.Enable(EnableCap.DepthTest);        
            InicializarShaders();
            _view = Matrix4.LookAt(new Vector3(5, 5, 7), Vector3.Zero, Vector3.UnitY);
            _projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, Size.X / (float)Size.Y, 0.1f, 100f);

            OnLoadCustom?.Invoke(ShaderProgram);

             //_escenario = new Escenario(Vector3.Zero, ShaderProgram);
             _escenario2 = serialicer.CargarDesdeJson<Escenario>("escenario.json");
             _escenario2.SetShaderProgram(ShaderProgram);
            Console.WriteLine($"Objetos cargados: {_escenario2._objetos.Count}");
            foreach (var obj in _escenario2._objetos){
                Console.WriteLine($" Partes: {obj._partes.Count}");
            }
                
                
            
            // //1) Definición del array de vértices (igual que antes)
            // var vertices = new Vertice[]
            // {
            //     // Base
            //     new Vertice(new Vector3(0,    0,   0),   new Vector3(1,0,0)), // 0
            //     new Vertice(new Vector3(1.5f, 0,   0),   new Vector3(0,1,0)), // 1
            //     new Vertice(new Vector3(1.5f, 0,  -0.5f),new Vector3(0,0,1)), // 2
            //     new Vertice(new Vector3(0,    0,  -0.5f),new Vector3(1,0,0)), // 3

            //     // Primer Pilar
            //     new Vertice(new Vector3(0,    2.0f,   0),   new Vector3(0,1,0)), // 4
            //     new Vertice(new Vector3(0,    2.0f,  -0.5f),new Vector3(0,0,1)), // 5
            //     new Vertice(new Vector3(0.35f,0.35f,  0),   new Vector3(1,0,0)), // 6
            //     new Vertice(new Vector3(0.35f,2.0f,  0),   new Vector3(0,0,1)), // 7
            //     new Vertice(new Vector3(0.35f,0.35f,-0.5f),new Vector3(1,0,0)), // 8
            //     new Vertice(new Vector3(0.35f,2.0f, -0.5f),new Vector3(0,0,1)), // 9

            //     // Segundo Pilar
            //     new Vertice(new Vector3(1.5f,2.0f,   0),   new Vector3(1,0,0)), // 10
            //     new Vertice(new Vector3(1.5f,2.0f,  -0.5f),new Vector3(0,1,0)), // 11
            //     new Vertice(new Vector3(1.15f,0.35f, 0),   new Vector3(1,0,0)), // 12
            //     new Vertice(new Vector3(1.15f,2.0f,  0),   new Vector3(0,0,1)), // 13
            //     new Vertice(new Vector3(1.15f,0.35f,-0.5f),new Vector3(1,0,0)), // 14
            //     new Vertice(new Vector3(1.15f,2.0f, -0.5f),new Vector3(0,0,1)), // 15
            //     new Vertice(new Vector3(1.15f,2.0f,   0),   new Vector3(1,0.647f,0)), // 16
            // };

            // // 2) Caras

            // // 2a) Cara de la base
            // var caraBase = new Cara(new List<Vertice>
            // {
            //     vertices[0], vertices[1],
            //     vertices[1], vertices[2],
            //     vertices[2], vertices[3],
            //     vertices[3], vertices[0],
            // });

            // // 2b) Cara del primer pilar
            // var caraPilar1 = new Cara(new List<Vertice>
            // {
            //     vertices[0], vertices[4],  // vertical frontal
            //     vertices[3], vertices[5],  // vertical trasera
            //     vertices[6], vertices[7],  // diagonal frontal
            //     vertices[6], vertices[8],  // horizontal interior
            //     vertices[8], vertices[9],  // diagonal trasera
            // });

            // // 2c) Cara del segundo pilar
            // var caraPilar2 = new Cara(new List<Vertice>
            // {
            //     vertices[1], vertices[10],
            //     vertices[2], vertices[11],
            //     vertices[12], vertices[13],
            //     vertices[12], vertices[14],
            //     vertices[14], vertices[15],
            // });

            // // 2d) Cara de las uniones entre pilares (la “U” superior)
            // var caraUniones = new Cara(new List<Vertice>
            // {
            //     vertices[6], vertices[12], // frontal
            //     vertices[8], vertices[14], // trasera
            // });

            // // 2e) Cara de las tapas (bases superiores de los pilares)
            // var caraTapas = new Cara(new List<Vertice>
            // {
            //     // tapa primer pilar
            //     vertices[4], vertices[7],
            //     vertices[7], vertices[9],
            //     vertices[9], vertices[5],
            //     vertices[5], vertices[4],

            //     // tapa segundo pilar
            //     vertices[10], vertices[11],
            //     vertices[11], vertices[15],
            //     vertices[15], vertices[16],
            //     vertices[16], vertices[10],
            // });

            // // 3) Creamos cada Parte y le agregamos su(s) Cara(s)
            // var parteBase    = new Parte(Vector3.Zero);
            // parteBase.AgregarCara(caraBase);

            // var partePilar1  = new Parte(Vector3.Zero);
            // partePilar1.AgregarCara(caraPilar1);

            // var partePilar2  = new Parte(Vector3.Zero);
            // partePilar2.AgregarCara(caraPilar2);

            // var parteUniones = new Parte(Vector3.Zero);
            // parteUniones.AgregarCara(caraUniones);

            // var parteTapas   = new Parte(Vector3.Zero);
            // parteTapas.AgregarCara(caraTapas);

            // // 4) Creamos el Objeto y le agregamos todas las Partes
            // var objeto = new Objeto(Vector3.Zero);
            // objeto.AgregarParte(parteBase);
            // objeto.AgregarParte(partePilar1);
            // objeto.AgregarParte(partePilar2);
            // objeto.AgregarParte(parteUniones);
            // objeto.AgregarParte(parteTapas);

            // _escenario.AgregarObjeto(objeto);

            
            // serialicer.GuardarAJson(_escenario, "escenario.json");
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);
            _projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, Size.X / (float)Size.Y, 0.1f, 100f);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

        }

        private void InicializarShaders()
        {
            string vertexShader = @"#version 330 core
                layout(location=0) in vec3 aPosition;
                layout(location=1) in vec3 aColor;
                out vec3 fragColor;
                uniform mat4 view;
                uniform mat4 projection;
                void main() {
                    gl_Position = projection * view * vec4(aPosition, 1.0);
                    fragColor = aColor;
                }";

            string fragmentShader = @"#version 330 core
                in vec3 fragColor;
                out vec4 color;
                void main() {
                    color = vec4(fragColor, 1.0);
                }";

            int vs = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vs, vertexShader);
            GL.CompileShader(vs);

            int fs = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fs, fragmentShader);
            GL.CompileShader(fs);

            ShaderProgram = GL.CreateProgram();
            GL.AttachShader(ShaderProgram, vs);
            GL.AttachShader(ShaderProgram, fs);
            GL.LinkProgram(ShaderProgram);

            GL.DeleteShader(vs);
            GL.DeleteShader(fs);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.UseProgram(ShaderProgram);
            
            var model = Matrix4.CreateRotationX(_angleX) * Matrix4.CreateRotationY(_angleY);
            
            GL.UniformMatrix4(GL.GetUniformLocation(ShaderProgram, "view"), false, ref _view);
            GL.UniformMatrix4(GL.GetUniformLocation(ShaderProgram, "projection"), false, ref _projection);
            GL.UniformMatrix4(GL.GetUniformLocation(ShaderProgram, "model"), false, ref model);
            
            //_escenario?.Dibujar();
            _escenario2?.Dibujar();
            SwapBuffers();
        }

        static void Main(string[] args)
        {   
            var nativeWindowSettings = new NativeWindowSettings()
            {
                ClientSize = new Vector2i(800, 600),
                Title = "Cubo 3D con OpenTK y Shaders",
                Flags = ContextFlags.Default,
                Profile = ContextProfile.Core,
                APIVersion = new Version(3, 3)
            };

            using (var window = new Program(GameWindowSettings.Default, nativeWindowSettings))
            {   
                window.Run();
            }
        }
    }
}