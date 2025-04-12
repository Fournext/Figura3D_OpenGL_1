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

        public static int ShaderProgram { get; private set; }

        public Program(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }


        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(0.1f, 0.1f, 0.2f, 1.0f);
            GL.Enable(EnableCap.DepthTest);        
            InicializarShaders();
            _view = Matrix4.LookAt(new Vector3(5, 5, 7), Vector3.Zero, Vector3.UnitY);
            _projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, Size.X / (float)Size.Y, 0.1f, 100f);


            _escenario = new Escenario(Vector3.Zero, ShaderProgram);
            
            //_escenario2.SetShaderProgram(ShaderProgram);
            
            var objeto = new Objeto(Vector3.Zero);
            var parte = new Parte(Vector3.Zero);

            var vertices = new Vertice[]
            {
                // Base
                new Vertice(new Vector3(0, 0, 0), new Vector3(1.0f, 0.0f, 0.0f)),       // 0 - rojo
                new Vertice(new Vector3(+1.5f, 0, 0), new Vector3(0.0f, 1.0f, 0.0f)),    // 1 - verde
                new Vertice(new Vector3(+1.5f, 0, -0.5f), new Vector3(0.0f, 0.0f, 1.0f)), // 2 - azul
                new Vertice(new Vector3(0, 0, -0.5f), new Vector3(1.0f, 0.0f, 0.0f)),     // 3 - rojo
                
                // Primer Pilar
                new Vertice(new Vector3(0, +2.0f, 0), new Vector3(0.0f, 1.0f, 0.0f)),     // 4 - verde
                new Vertice(new Vector3(0, +2.0f, -0.5f), new Vector3(0.0f, 0.0f, 1.0f)), // 5 - azul
                new Vertice(new Vector3(+0.35f, +0.35f, 0), new Vector3(1.0f, 0.0f, 0.0f)), // 6 - rojo
                new Vertice(new Vector3(+0.35f, +2.0f, 0), new Vector3(0.0f, 0.0f, 1.0f)), // 7 - azul
                new Vertice(new Vector3(+0.35f, +0.35f, -0.5f), new Vector3(1.0f, 0.0f, 0.0f)), // 8 - rojo
                new Vertice(new Vector3(+0.35f, +2.0f, -0.5f), new Vector3(0.0f, 0.0f, 1.0f)), // 9 - azul
                
                // Segundo Pilar
                new Vertice(new Vector3(+1.5f, +2.0f, 0), new Vector3(1.0f, 0.0f, 0.0f)), // 10 - rojo
                new Vertice(new Vector3(+1.5f, +2.0f, -0.5f), new Vector3(0.0f, 1.0f, 0.0f)), // 11 - verde
                new Vertice(new Vector3(+1.15f, +0.35f, 0), new Vector3(1.0f, 0.0f, 0.0f)), // 12 - rojo
                new Vertice(new Vector3(+1.15f, +2.0f, 0), new Vector3(0.0f, 0.0f, 1.0f)), // 13 - azul
                new Vertice(new Vector3(+1.15f, +0.35f, -0.5f), new Vector3(1.0f, 0.0f, 0.0f)), // 14 - rojo
                new Vertice(new Vector3(+1.15f, +2.0f, -0.5f), new Vector3(0.0f, 0.0f, 1.0f)), // 15 - azul
                new Vertice(new Vector3(+1.15f, +2.0f, 0), new Vector3(1.0f, 0.647f, 0.0f)) // 16 - naranja
            };

            var caraAristas = new Cara(new List<Vertice>
            {
                // Base
                vertices[0], vertices[1],  // línea inferior
                vertices[1], vertices[2],  // línea derecha
                vertices[2], vertices[3],  // línea superior
                vertices[3], vertices[0],  // línea izquierda
                
                // Primer Pilar
                vertices[0], vertices[4],  // línea vertical izquierda frontal
                vertices[3], vertices[5],  // línea vertical izquierda trasera
                vertices[6], vertices[7],  // línea diagonal frontal
                vertices[6], vertices[8],  // línea horizontal diagonal
                vertices[8], vertices[9],  // línea diagonal trasera
                
                // Segundo Pilar
                vertices[1], vertices[10], // línea vertical derecha frontal
                vertices[2], vertices[11], // línea vertical derecha trasera
                vertices[12], vertices[13], // línea diagonal frontal
                vertices[12], vertices[14], // línea horizontal diagonal
                vertices[14], vertices[15], // línea diagonal trasera
                
                // Uniones entre pilares
                vertices[6], vertices[12], // línea frontal entre pilares
                vertices[8], vertices[14], // línea trasera entre pilares
                
                // Tapas de pilares
                // Primer pilar
                vertices[4], vertices[7],
                vertices[7], vertices[9],
                vertices[9], vertices[5],
                vertices[5], vertices[4],
                
                // Segundo pilar
                vertices[10], vertices[11],
                vertices[11], vertices[15],
                vertices[15], vertices[16],
                vertices[16], vertices[10]
            });

            parte.AgregarCara(caraAristas);
            objeto.AgregarParte(parte);
            _escenario.AgregarObjeto(objeto);
            //_escenario.GuardarAJson("escenario.json");
            //_escenario2.CargarDesdeJson("cubo.json");
            //_escenario2 = Escenario.CargarDesdeJson("escenario.json", ShaderProgram);
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
            
            _escenario?.Dibujar();
            //_escenario2?.Dibujar();
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