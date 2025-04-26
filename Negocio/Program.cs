using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.GraphicsLibraryFramework;


namespace OpenTKCubo3D
{
    class Program : GameWindow
    {
        private float _cameraAngleY;
        private float _cameraAngleX;
        private float _cameraAngleZ;
        private float _cameraOffsetZ = 0.0f;
        private float _cameraDistance = 20.0f;
        private int _shaderProgram;
        private Matrix4 _view;
        private Matrix4 _projection;
        private Escenario _escenario = new Escenario();
        UIEditor uiEditor = new UIEditor();
        private ImGuiController _imguiController;
        private Shaders shaders = new Shaders();



        public Program(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(0.1f, 0.1f, 0.2f, 1.0f);
            GL.Enable(EnableCap.DepthTest);
            _imguiController = new ImGuiController(ClientSize.X, ClientSize.Y);
            _escenario.Inicializar();
            _shaderProgram=shaders.inicializarShader(_shaderProgram);
            
            // Configurar la vista y la proyección
            _view = Matrix4.LookAt(new Vector3(5, 5, 12), Vector3.Zero, Vector3.UnitY);
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

            if (input.IsKeyDown(Keys.Escape)) Close();

            _cameraAngleX = MathHelper.Clamp(_cameraAngleX, -MathHelper.PiOver2 + 0.1f, MathHelper.PiOver2 - 0.1f);
            if (_escenario != null && KeyboardState.IsKeyDown(Keys.LeftShift))
            {
                float delta = 0.05f;
                float rot = 2f;

                Vector3 valor = Vector3.Zero;
                if (KeyboardState.IsKeyDown(Keys.Up)) valor.Y += 1;
                if (KeyboardState.IsKeyDown(Keys.Down)) valor.Y -= 1;
                if (KeyboardState.IsKeyDown(Keys.Left)) valor.X -= 1;
                if (KeyboardState.IsKeyDown(Keys.Right)) valor.X += 1;
                if (KeyboardState.IsKeyDown(Keys.Q)) valor.Z += 1;
                if (KeyboardState.IsKeyDown(Keys.E)) valor.Z -= 1;

                if (valor == Vector3.Zero) return;

                int tipo = uiEditor.TipoTransformacion;

                if (uiEditor.SeleccionaEscenario)
                {
                    if (tipo == 0) _escenario.Rotacion(valor.X * rot, valor.Y * rot, valor.Z * rot);
                    else if (tipo == 1) _escenario.Traslacion(valor.X * delta, valor.Y * delta, valor.Z * delta);
                    else if (tipo == 2) _escenario.Escalacion(1 + valor.Y * 0.05f);
                }
                else
                {
                    var objeto = _escenario.Objetos[uiEditor.NombreObjetoSeleccionado!];
                    if (objeto == null) return;

                    if (uiEditor.NombreParteSeleccionada == null || uiEditor.NombreParteSeleccionada == "(Todo el objeto)")
                    {
                        if (tipo == 0) objeto.Rotacion(valor.X * rot, valor.Y * rot, valor.Z * rot);
                        else if (tipo == 1) objeto.Traslacion(valor.X * delta, valor.Y * delta, valor.Z * delta);
                        else if (tipo == 2) objeto.Escalacion(1 + valor.Y * 0.05f);
                    }
                    else
                    {
                        var parte = objeto.Partes[uiEditor.NombreParteSeleccionada];
                        if (parte == null) return;

                        if (tipo == 0) parte.Rotacion(valor.X * rot, valor.Y * rot, valor.Z * rot);
                        else if (tipo == 1) parte.Traslacion(valor.X * delta, valor.Y * delta, valor.Z * delta);
                        else if (tipo == 2) parte.Escalacion(1 + valor.Y * 0.05f);
                    }
                }
            }else if(_escenario != null && !KeyboardState.IsKeyDown(Keys.LeftShift)){
                float rotationSpeed = 0.002f;
                if (input.IsKeyDown(Keys.Left)) _cameraAngleY += rotationSpeed;
                if (input.IsKeyDown(Keys.Right)) _cameraAngleY -= rotationSpeed;
                if (input.IsKeyDown(Keys.Up)) _cameraAngleX -= rotationSpeed;
                if (input.IsKeyDown(Keys.Down)) _cameraAngleX += rotationSpeed;
                if (input.IsKeyDown(Keys.Q)) _cameraOffsetZ += 0.05f;
                if (input.IsKeyDown(Keys.E)) _cameraOffsetZ -= 0.05f;
            }
        }


        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.UseProgram(_shaderProgram);

            Vector3 front = new Vector3(
                (float)(Math.Sin(_cameraAngleY) * Math.Cos(_cameraAngleX)),
                (float)(Math.Sin(_cameraAngleX)),
                (float)(Math.Cos(_cameraAngleY) * Math.Cos(_cameraAngleX))
            );

            Vector3 cameraPos = front * _cameraDistance;

            cameraPos.Z += _cameraOffsetZ;  
            _view = Matrix4.LookAt(cameraPos, Vector3.Zero, Vector3.UnitY);

            GL.UniformMatrix4(GL.GetUniformLocation(_shaderProgram, "view"), false, ref _view);
            GL.UniformMatrix4(GL.GetUniformLocation(_shaderProgram, "projection"), false, ref _projection);

            _escenario.Dibujar(_shaderProgram);

            _imguiController.Update(this, (float)args.Time);
            uiEditor.Dibujar(_escenario!);
            _imguiController.Render();

            SwapBuffers();
        }



        static void Main(string[] args)
        {
            Serializer _serializer = new Serializer();
            CrearFigura fig = new CrearFigura();


            List<Vertice> v1 = new List<Vertice>();
            List<Vertice> v2 = new List<Vertice>();
            List<Vertice> v3 = new List<Vertice>();

            v1.Add(new Vertice(-5, 0, 0, 1, 1, 1));
            v1.Add(new Vertice(5, 0, 0, 1, 1, 1));
            v2.Add(new Vertice(0, -5, 0, 1, 1, 1));
            v2.Add(new Vertice(0, 5, 0, 1, 1, 1));
            v3.Add(new Vertice(0, 0, -5, 1, 1, 1));
            v3.Add(new Vertice(0, 0, 5, 1, 1, 1));

            Dictionary<string, Cara> Cara1 = new Dictionary<string, Cara>
            {
                { "ejeX", new Cara("ejeX", v1, 0f, 0f, 0f) }
            };

            Dictionary<string, Cara> Cara2 = new Dictionary<string, Cara>
            {
                { "ejeY", new Cara("ejeY", v2, 0f, 0f, 0f) }
            };

            Dictionary<string, Cara> Cara3 = new Dictionary<string, Cara>
            {
                { "ejeZ", new Cara("ejeZ", v3, 0f, 0f, 0f) }
            };

            Dictionary<string, Parte> P1 = new Dictionary<string, Parte>
            {
                { "parte_X", new Parte(Cara1, 0f, 0f, 0f) },
                { "parte_Y", new Parte(Cara2, 0f, 0f, 0f) },
                { "parte_Z", new Parte(Cara3, 0f, 0f, 0f) }
            };

            Objeto Ejes = new Objeto(P1, 5f, 2f, 2f);

            Dictionary<string, Objeto> Dic_objetos = new Dictionary<string, Objeto>();

            Objeto U1 = fig.CrearFiguraU(5f, 2f, 2f);
            Objeto U2 = fig.CrearFiguraU(3f, 0f, 0f);
            Objeto U3 = fig.CrearFiguraU(7f, 0f, 0f);

            Dic_objetos.Add("U1", U1);
            Dic_objetos.Add("U2", U2);
            Dic_objetos.Add("U3", U3);
            Dic_objetos.Add("ejes", Ejes);

            Escenario escenario = new Escenario(Dic_objetos, 0, 0, 0);

            Escenario escenario2;


            _serializer.GuardarAJson(escenario,"escenario.json");
            //escenario2 = (_serializer.CargarDesdeJson<Escenario>("escenario.json"))!;
            //escenario2.RecalcularTransformaciones();

            var nativeWindowSettings = new NativeWindowSettings()
            {
                ClientSize = new Vector2i(800, 600),
                Title = "Cubo 3D con OpenTK y Shaders",
                Flags = ContextFlags.Default,
                Profile = ContextProfile.Core,
            };

            using (var window = new Program(GameWindowSettings.Default, nativeWindowSettings))
            {

                window._escenario = escenario;
                window.Run();
            }
        }

    }
}