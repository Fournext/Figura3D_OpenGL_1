using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Runtime.CompilerServices;
using fig;
using System.Runtime.Serialization;

namespace OpenTKCubo3D
{
    class Program : GameWindow
    {
        
        private float _angleY; // Ángulo de rotación en el eje Y (izquierda/derecha)
        private float _angleX; // Ángulo de rotación en el eje X (arriba/abajo)
        
        private Matrix4 _view;
        private Matrix4 _projection;

        //Vertices modificables

        public Program(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        public Action<Matrix4,Matrix4>? OnLoadCustom;
        protected override void OnLoad()
        {
           base.OnLoad();

            GL.ClearColor(0.1f, 0.1f, 0.2f, 1.0f);
            GL.Enable(EnableCap.DepthTest);
            
            // Configurar la vista y la proyección
            _view = Matrix4.LookAt(new Vector3(5, 5, 7), Vector3.Zero, Vector3.UnitY);
            _projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, Size.X / (float)Size.Y, 0.1f, 100f);

            OnLoadCustom?.Invoke(_view, _projection);
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

        public Action<float,float>? OnRenderCuston;
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            OnRenderCuston?.Invoke(_angleX,_angleY);

            SwapBuffers();
        }

        static void Main(string[] args)
        {   
            float Vx = 1.0f;
            float Vy = 0.1f;
            float Vz = 0.1f;
            Figura fig = new Figura(Vx,Vy,Vz);
            Figura fig2 = new Figura(0f,0f,0f);
            Figura fig3 = new Figura(0.5f,0.5f,0.5f);
            // Dibujar desde las posiciones 0 , 0 ,0
            float[] vertices = {
                //Sentido Horario
                // Base
                //Punto Inicial->Der
                 0,      0, 0,  1.0f, 0.0f, 0.0f, // Punto Inicial, rojo
                 +1.5f, 0, 0, 0.0f, 1.0f, 0.0f, // verde 
                
                //Der->Arri
                 +1.5f, 0, 0,    0.0f, 1.0f, 0.0f, // verde
                 +1.5f, 0, -0.5f, 0.0f, 0.0f, 1.0f, // azul
                
                //Arri->Izq
                 +1.5f, 0, -0.5f, 0.0f, 0.0f, 1.0f, // azul
                 0,      0, -0.5f, 1.0f, 0.0f, 0.0f, // rojo

                //Izq->Abajo
                 0,      0, -0.5f, 1.0f, 0.0f, 0.0f, // rojo
                 0,      0, 0, 0.0f, 1.0f, 0.0f, // Verde


                //1° Pilar
                //Abajo->Arri
                 0,   0,      0, 0.0f, 1.0f, 0.0f, // Punto Inicial, Verde
                 0,   +2.0f, 0, 0.0f, 1.0f, 0.0f, // Verde

                //Base.Izq->Arri
                 0,   0,      -0.5f, 1.0f, 0.0f, 0.0f, // rojo
                 0,    +2.0f,  -0.5f, 0.0f, 0.0f, 1.0f, // Verde
                
                //Base.Ini: x+0.35 y+0.35 -> Arriba
                +0.35f, +0.35f,  0, 1.0f, 0.0f, 0.0f, // rojo
                +0.35f, +2.0f,   0, 0.0f, 0.0f, 1.0f, // azul

                //Base.Ini: x+0.35 y+0.35 -> Base.Ini: x+0.35 y+0.35 z-0.5
                +0.35f, +0.35f,  0,      0.0f, 0.0f, 1.0f, // azul
                +0.35f, +0.35f,  -0.5f, 1.0f, 0.0f, 0.0f, // rojo

                //Base.Ini: x+0.35 y+0.35 z-0.5 -> Arriba
                +0.35f, +0.35f,  -0.5f, 1.0f, 0.0f, 0.0f, // rojo
                +0.35f, +2.0f,   -0.5f, 0.0f, 0.0f, 1.0f, // azul


                //2° Pilar
                //Base.Der->Arri
                 +1.5f, 0,      0, 0.0f, 1.0f, 0.0f, // verde
                 +1.5f, +2.0f, 0, 1.0f, 0.0f, 0.0f, // Rojo

                //Base.Arriba->Arri
                 +1.5f, 0,      -0.5f, 0.0f, 0.0f, 1.0f, // azul
                 +1.5f, +2.0f, -0.5f, 0.0f, 1.0f, 0.0f, // verde
                
                //Base.Der: x-0.35 y+0.35 -> Arriba
                 +1.15f, +0.35f, 0, 1.0f, 0.0f, 0.0f, // rojo
                 +1.15f, +2.0f,  0, 0.0f, 0.0f, 1.0f, // azul

                //Base.Der: x-0.35 y+0.35 -> Base.Ini: -0.35 y+0.35 z-0.5
                 +1.15f, +0.35f, 0,      0.0f, 0.0f, 1.0f, // azul
                 +1.15f, +0.35f, -0.5f, 1.0f, 0.0f, 0.0f, // rojo

                //Base.Der: x-0.35 y+0.35 z-0.5 -> Arriba
                 +1.15f, +0.35f, -0.5f, 1.0f, 0.0f, 0.0f, // rojo
                 +1.15f, +2.00f, -0.5f, 0.0f, 0.0f, 1.0f, // azul


                //Union de los Pilares
                //Base.Ini: x+0.35 y+0.35 -> Base.Der: x-0.35 y+0.35
                 +0.35f, +0.35f,  0, 0.0f, 1.0f, 0.0f, // Verde
                 +1.15f, +0.35f,  0, 0.0f, 1.0f, 0.0f, // Verde
                
                //Base.Ini: x+0.35 y+0.35 z-0.5 -> Base.Der: x-0.35 y+0.35 z-0.5
                 +0.35f, +0.35f, -0.5f, 0.0f, 0.0f, 1.0f, // azul
                 +1.15f, +0.35f, -0.5f, 0.0f, 0.0f, 1.0f, // azul


                //Tapa del Primer Pilar
                //Pilar 1.Arriba.Punto Inicial -> Base.Ini: x+0.35 y+0.35.Arriba
                0,       +2.00f, 0, 0.0f, 1.0f, 0.0f, // Verde
                +0.35f, +2.00f, 0, 0.0f, 0.0f, 1.0f, // azul
                 
                //Base.Ini: x+0.35 y+0.35.Arriba -> Base.Ini: x+0.35 y+0.35 z-0.5 .Arriba
                +0.35f, +2.00f, 0,      0.0f, 0.0f, 1.0f, // azul
                +0.35f, +2.00f, -0.5f, 1.0f, 0.0f, 0.0f, // rojo
                
                //Base.Ini: x+0.35 y+0.35 z-0.5 .Arriba -> Base.Izq.Arri
                +0.35f, +2.00f, -0.5f, 1.0f, 0.0f, 0.0f, // rojo
                0,       +2.00f, -0.5f, 0.0f, 1.0f, 0.0f, // verde

                //Base.Izq.Arri -> Base.Arriba
                0,       +2.00f, -0.5f, 0.0f, 1.0f, 0.0f, // verde
                0,       +2.00f, 0,      1.0f, 0.0f, 0.0f, // Rojo


                //Tapa del Segundo Pilar
                //Base.Der.Arri -> Base.Arriba.Arri
                +1.50f,  +2.00f,  0,      1.0f, 0.0f, 0.0f, // Rojo
                +1.50f,  +2.00f,  -0.5f, 0.0f, 1.0f, 0.0f, // verde

                //Base.Arriba.Arri -> Base.Der: x-0.35 y+0.35 z-0.5 .Arriba
                +1.50f,  +2.00f,  -0.5f, 0.0f, 1.0f, 0.0f, // verde
                +1.15f,  +2.00f,  -0.5f, 0.0f, 0.0f, 1.0f, // azul

                //Base.Der: x-0.35 y+0.35 z-0.5 .Arriba -> Base.Der: x-0.35 y+0.35 .Arriba
                +1.15f,  +2.00f,  -0.5f, 0.0f, 0.0f, 1.0f, // azul
                +1.15f,  +2.00f,  0     , 1.0f, 0.647f, 0.0f, // naranja

                //Base.Der: x-0.35 y+0.35 .Arriba -> Base.Der.Arri
                +1.15f,  +2.00f,  0, 1.0f, 0.647f, 0.0f, // naranja
                +1.50f,  +2.00f,  0, 1.0f, 0.0f, 0.0f, // Rojo

                
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

            float[] mtr3 = {

            };

            
            
            var nativeWindowSettings = new NativeWindowSettings()
            {
                ClientSize = new Vector2i(800, 600),
                Title = "Cubo 3D con OpenTK y Shaders",
                Flags = ContextFlags.Default,
                Profile = ContextProfile.Core,
            };

            using (var window = new Program(GameWindowSettings.Default, nativeWindowSettings))
            {   
                window.OnLoadCustom = (_view, _projection)=>{
                    fig.CargarVertices(vertices, _view, _projection);
                    fig2.CargarVertices(Eje_Vertices, _view, _projection);
                };

                window.OnRenderCuston = (_angleX, _angleY) => {
                    fig.Dibujar_ConRotacion(_angleX, _angleY); 
                    fig2.Dibujar_SinRotacion();    
                };
                window.Run();
            }
        }
    }
}