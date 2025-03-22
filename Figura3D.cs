using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Runtime.CompilerServices;

namespace OpenTKCubo3D
{
    class Program : GameWindow
    {
        
        private float _angleY; // Ángulo de rotación en el eje Y (izquierda/derecha)
        private float _angleX; // Ángulo de rotación en el eje X (arriba/abajo)
        private int _vertexBufferObject;
        private int _vertexArrayObject;
        //Para los ejes
        private int _EjeVertexBufferObject;
        private int _EjeVertexArrayObject;

        private int _shaderProgram;
        private Matrix4 _view;
        private Matrix4 _projection;

        //Vertices modificables
        public float Vx;
        public float Vy;
        public float Vz; 

        public Program(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
           base.OnLoad();
            GL.ClearColor(0.1f, 0.1f, 0.2f, 1.0f);
            GL.Enable(EnableCap.DepthTest);
 
            // Configurar los vértices del cubo
            float[] vertices = {
                //Sentido Horario
                // Base
                //Punto Inicial->Der
                 Vx,      Vy, Vz,  1.0f, 0.0f, 0.0f, // Punto Inicial, rojo
                 Vx+1.5f, Vy, Vz, 0.0f, 1.0f, 0.0f, // verde 
                
                //Der->Arri
                 Vx+1.5f, Vy, Vz,    0.0f, 1.0f, 0.0f, // verde
                 Vx+1.5f, Vy, Vz-0.5f, 0.0f, 0.0f, 1.0f, // azul
                
                //Arri->Izq
                 Vx+1.5f, Vy, Vz-0.5f, 0.0f, 0.0f, 1.0f, // azul
                 Vx,      Vy, Vz-0.5f, 1.0f, 0.0f, 0.0f, // rojo

                //Izq->Abajo
                 Vx,      Vy, Vz-0.5f, 1.0f, 0.0f, 0.0f, // rojo
                 Vx,      Vy, Vz, 0.0f, 1.0f, 0.0f, // Verde


                //1° Pilar
                //Abajo->Arri
                 Vx,   Vy,      Vz, 0.0f, 1.0f, 0.0f, // Punto Inicial, Verde
                 Vx,   Vy+2.0f, Vz, 0.0f, 1.0f, 0.0f, // Verde

                //Base.Izq->Arri
                 Vx,   Vy,      Vz-0.5f, 1.0f, 0.0f, 0.0f, // rojo
                 Vx,    Vy+2.0f, Vz -0.5f, 0.0f, 0.0f, 1.0f, // Verde
                
                //Base.Ini: x+0.35 y+0.35 -> Arriba
                Vx+0.35f, Vy+0.35f,  Vz, 1.0f, 0.0f, 0.0f, // rojo
                Vx+0.35f, Vy+2.0f,   Vz, 0.0f, 0.0f, 1.0f, // azul

                //Base.Ini: x+0.35 y+0.35 -> Base.Ini: x+0.35 y+0.35 z-0.5
                Vx+0.35f, Vy+0.35f,  Vz,      0.0f, 0.0f, 1.0f, // azul
                Vx+0.35f, Vy+0.35f,  Vz-0.5f, 1.0f, 0.0f, 0.0f, // rojo

                //Base.Ini: x+0.35 y+0.35 z-0.5 -> Arriba
                Vx+0.35f, Vy+0.35f,  Vz-0.5f, 1.0f, 0.0f, 0.0f, // rojo
                Vx+0.35f, Vy+2.0f,   Vz-0.5f, 0.0f, 0.0f, 1.0f, // azul


                //2° Pilar
                //Base.Der->Arri
                 Vx+1.5f, Vy,      Vz, 0.0f, 1.0f, 0.0f, // verde
                 Vx+1.5f, Vy+2.0f, Vz, 1.0f, 0.0f, 0.0f, // Rojo

                //Base.Arriba->Arri
                 Vx+1.5f, Vy,      Vz-0.5f, 0.0f, 0.0f, 1.0f, // azul
                 Vx+1.5f, Vy+2.0f, Vz-0.5f, 0.0f, 1.0f, 0.0f, // verde
                
                //Base.Der: x-0.35 y+0.35 -> Arriba
                 Vx+1.15f, Vy+0.35f, Vz, 1.0f, 0.0f, 0.0f, // rojo
                 Vx+1.15f, Vy+2.0f,  Vz, 0.0f, 0.0f, 1.0f, // azul

                //Base.Der: x-0.35 y+0.35 -> Base.Ini: -0.35 y+0.35 z-0.5
                 Vx+1.15f, Vy+0.35f, Vz,      0.0f, 0.0f, 1.0f, // azul
                 Vx+1.15f, Vy+0.35f, Vz-0.5f, 1.0f, 0.0f, 0.0f, // rojo

                //Base.Der: x-0.35 y+0.35 z-0.5 -> Arriba
                 Vx+1.15f, Vy+0.35f, Vz-0.5f, 1.0f, 0.0f, 0.0f, // rojo
                 Vx+1.15f, Vy+2.00f, Vz-0.5f, 0.0f, 0.0f, 1.0f, // azul


                //Union de los Pilares
                //Base.Ini: x+0.35 y+0.35 -> Base.Der: x-0.35 y+0.35
                 Vx+0.35f, Vy+0.35f,  Vz, 0.0f, 1.0f, 0.0f, // Verde
                 Vx+1.15f, Vy+0.35f,  Vz, 0.0f, 1.0f, 0.0f, // Verde
                
                //Base.Ini: x+0.35 y+0.35 z-0.5 -> Base.Der: x-0.35 y+0.35 z-0.5
                 Vx+0.35f, Vy+0.35f, Vz-0.5f, 0.0f, 0.0f, 1.0f, // azul
                 Vx+1.15f, Vy+0.35f, Vz-0.5f, 0.0f, 0.0f, 1.0f, // azul


                //Tapa del Primer Pilar
                //Pilar 1.Arriba.Punto Inicial -> Base.Ini: x+0.35 y+0.35.Arriba
                Vx,       Vy+2.00f, Vz, 0.0f, 1.0f, 0.0f, // Verde
                Vx+0.35f, Vy+2.00f, Vz, 0.0f, 0.0f, 1.0f, // azul
                 
                //Base.Ini: x+0.35 y+0.35.Arriba -> Base.Ini: x+0.35 y+0.35 z-0.5 .Arriba
                Vx+0.35f, Vy+2.00f, Vz,      0.0f, 0.0f, 1.0f, // azul
                Vx+0.35f, Vy+2.00f, Vz-0.5f, 1.0f, 0.0f, 0.0f, // rojo
                
                //Base.Ini: x+0.35 y+0.35 z-0.5 .Arriba -> Base.Izq.Arri
                Vx+0.35f, Vy+2.00f, Vz-0.5f, 1.0f, 0.0f, 0.0f, // rojo
                Vx,       Vy+2.00f, Vz-0.5f, 0.0f, 1.0f, 0.0f, // verde

                //Base.Izq.Arri -> Base.Arriba
                Vx,       Vy+2.00f, Vz-0.5f, 0.0f, 1.0f, 0.0f, // verde
                Vx,       Vy+2.00f, Vz,      1.0f, 0.0f, 0.0f, // Rojo


                //Tapa del Segundo Pilar
                //Base.Der.Arri -> Base.Arriba.Arri
                Vx+1.50f,  Vy+2.00f,  Vz,      1.0f, 0.0f, 0.0f, // Rojo
                Vx+1.50f,  Vy+2.00f,  Vz-0.5f, 0.0f, 1.0f, 0.0f, // verde

                //Base.Arriba.Arri -> Base.Der: x-0.35 y+0.35 z-0.5 .Arriba
                Vx+1.50f,  Vy+2.00f,  Vz-0.5f, 0.0f, 1.0f, 0.0f, // verde
                Vx+1.15f,  Vy+2.00f,  Vz-0.5f, 0.0f, 0.0f, 1.0f, // azul

                //Base.Der: x-0.35 y+0.35 z-0.5 .Arriba -> Base.Der: x-0.35 y+0.35 .Arriba
                Vx+1.15f,  Vy+2.00f,  Vz-0.5f, 0.0f, 0.0f, 1.0f, // azul
                Vx+1.15f,  Vy+2.00f,  Vz     , 1.0f, 0.647f, 0.0f, // naranja

                //Base.Der: x-0.35 y+0.35 .Arriba -> Base.Der.Arri
                Vx+1.15f,  Vy+2.00f,  Vz, 1.0f, 0.647f, 0.0f, // naranja
                Vx+1.50f,  Vy+2.00f,  Vz, 1.0f, 0.0f, 0.0f, // Rojo

                
            };
            float[] Eje_Vertices = {
                ///MAPA CARTESIANO
                //Eje X
                 2.00f, 0.0f ,0.0f, 1.0f, 1.0f, 1.0f,
                -2.00f, 0.0f ,0.0f, 1.0f, 1.0f, 1.0f,
                
                //Eje Y
                0.0f, 2.0f, 0.0f, 1.0f,1.0f,1.0f,
                0.0f,-2.0f, 0.0f, 1.0f,1.0f,1.0f,

                //Eje Z
                0.0f, 0.0f, 2.0f, 1.0f,1.0f,1.0f,
                0.0f, 0.0f,-2.0f, 1.0f,1.0f,1.0f,
            };

            // Crear y enlazar el VAO
            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            // Crear y enlazar el VBO
            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            // Configurar el atributo de posición (posición y color están intercalados)
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            // Crear y enlazar el VAO para los ejes
            _EjeVertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_EjeVertexArrayObject);

            // Crear y enlazar el VBO para los ejes
            _EjeVertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _EjeVertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, Eje_Vertices.Length * sizeof(float), Eje_Vertices, BufferUsageHint.StaticDraw);

            // Configurar el atributo de posición para los ejes
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            // Compilar shaders
            string vertexShaderSource = @"
                #version 330 core
                layout(location = 0) in vec3 aPosition;
                layout(location = 1) in vec3 aColor;
                out vec3 fragColor;
                uniform mat4 model;
                uniform mat4 view;
                uniform mat4 projection;
                void main()
                {
                    gl_Position = projection * view * model * vec4(aPosition, 1.0);
                    fragColor = aColor;
                }
            ";

            string fragmentShaderSource = @"
                #version 330 core
                in vec3 fragColor;
                out vec4 color;
                void main()
                {
                    color = vec4(fragColor, 1.0);
                }
            ";

            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaderSource);
            GL.CompileShader(vertexShader);

            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentShaderSource);
            GL.CompileShader(fragmentShader);

            _shaderProgram = GL.CreateProgram();
            GL.AttachShader(_shaderProgram, vertexShader);
            GL.AttachShader(_shaderProgram, fragmentShader);
            GL.LinkProgram(_shaderProgram);

            GL.DetachShader(_shaderProgram, vertexShader);
            GL.DetachShader(_shaderProgram, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);

            // Configurar la vista y la proyección
            _view = Matrix4.LookAt(new Vector3(5, 5, 7), Vector3.Zero, Vector3.UnitY);
            _projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, Size.X / (float)Size.Y, 0.1f, 100f);
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

            var input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            // Rotar el cubo con las flechas del teclado
            if (input.IsKeyDown(Keys.Left))
            {
                _angleY -= 0.02f; // Rotación en el eje Y (izquierda)
            }
            if (input.IsKeyDown(Keys.Right))
            {
                _angleY += 0.02f; // Rotación en el eje Y (derecha)
            }
            if (input.IsKeyDown(Keys.Up))
            {
                _angleX -= 0.02f; // Rotación en el eje X (arriba)
            }
            if (input.IsKeyDown(Keys.Down))
            {
                _angleX += 0.02f; // Rotación en el eje X (abajo)
            }
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.UseProgram(_shaderProgram);

            // Configurar las matrices de transformación
            Matrix4 model = Matrix4.CreateRotationY(_angleY) * Matrix4.CreateRotationX(_angleX);
            GL.UniformMatrix4(GL.GetUniformLocation(_shaderProgram, "model"), false, ref model);
            GL.UniformMatrix4(GL.GetUniformLocation(_shaderProgram, "view"), false, ref _view);
            GL.UniformMatrix4(GL.GetUniformLocation(_shaderProgram, "projection"), false, ref _projection);

            // Dibujar el cubo
            GL.BindVertexArray(_vertexArrayObject);
            GL.DrawArrays(PrimitiveType.Lines, 0, 48); 

            // Dibujar los ejes sin rotación
            Matrix4 identityMatrix = Matrix4.Identity; // Crear una copia local de la matriz identidad
            GL.UniformMatrix4(GL.GetUniformLocation(_shaderProgram, "model"), false, ref identityMatrix);
            GL.BindVertexArray(_EjeVertexArrayObject);
            GL.DrawArrays(PrimitiveType.Lines, 0, 6); // Dibujar los ejes

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
            };

            using (var window = new Program(GameWindowSettings.Default, nativeWindowSettings))
            {   
                window.Vx = -0.75f;
                window.Vy = 0.0f;
                window.Vz = 0.25f;
                window.Run();
            }
        }
    }
}