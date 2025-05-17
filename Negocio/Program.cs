using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.GraphicsLibraryFramework;


namespace OpenTKCubo3D
{
    public class Program : GameWindow
    {
        private float _cameraAngleY;
        private float _cameraAngleX;
        private Vector3 _cameraPosition = new Vector3(-0f, 20, -50f);
        private Vector3 _cameraFront =  Vector3.UnitY; 
        private Vector3 _cameraUp = Vector3.UnitY;
        private float _cameraSpeed = 10.0f;
        private int _shaderProgram;
        private Matrix4 _view;
        private Matrix4 _projection;
        private Escenario _escenario = new Escenario();
        private LibretoAnimacion libreto;
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
            GL.Disable(EnableCap.CullFace);

            _imguiController = new ImGuiController(ClientSize.X, ClientSize.Y);
            _escenario.Inicializar();
            _shaderProgram=shaders.inicializarShader(_shaderProgram);

            _projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, Size.X / (float)Size.Y, 0.1f, 100f);
   
            this.libreto?.Detener();
            if(libreto != null){
                libreto.CargarEscenario(_escenario);
                libreto.Iniciar();
            }
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

            base.OnUpdateFrame(args);

            if (libreto.EstaActivo)
            {
                libreto.TiempoGlobal += (float)args.Time;
            }

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
            }else if (_escenario != null && !KeyboardState.IsKeyDown(Keys.LeftShift))
            {
                float speed = _cameraSpeed * (float)args.Time;

                if (input.IsKeyDown(Keys.W)) _cameraPosition += _cameraFront * speed;
                if (input.IsKeyDown(Keys.S)) _cameraPosition -= _cameraFront * speed;
                if (input.IsKeyDown(Keys.A)) _cameraPosition -= Vector3.Normalize(Vector3.Cross(_cameraFront, _cameraUp)) * speed;
                if (input.IsKeyDown(Keys.D)) _cameraPosition += Vector3.Normalize(Vector3.Cross(_cameraFront, _cameraUp)) * speed;
                if (input.IsKeyDown(Keys.Q)) _cameraPosition += _cameraUp * speed;
                if (input.IsKeyDown(Keys.E)) _cameraPosition -= _cameraUp * speed;

                float rotationSpeed = 1.5f * (float)args.Time;

                if (input.IsKeyDown(Keys.Left)) _cameraAngleY += rotationSpeed;
                if (input.IsKeyDown(Keys.Right)) _cameraAngleY -= rotationSpeed;
                if (input.IsKeyDown(Keys.Up)) _cameraAngleX -= rotationSpeed;
                if (input.IsKeyDown(Keys.Down)) _cameraAngleX += rotationSpeed;

                // Calcular nueva dirección de cámara
                _cameraFront = new Vector3(
                    (float)(Math.Cos(_cameraAngleX) * Math.Sin(_cameraAngleY)),
                    (float)Math.Sin(_cameraAngleX),
                    (float)(Math.Cos(_cameraAngleX) * Math.Cos(_cameraAngleY))
                ).Normalized();
            }
            
        }


        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Enable(EnableCap.DepthTest);
            GL.Disable(EnableCap.CullFace);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.UseProgram(_shaderProgram);

            _view = Matrix4.LookAt(_cameraPosition, _cameraPosition + _cameraFront, _cameraUp);

            GL.UniformMatrix4(GL.GetUniformLocation(_shaderProgram, "view"), false, ref _view);
            GL.UniformMatrix4(GL.GetUniformLocation(_shaderProgram, "projection"), false, ref _projection);

            _escenario.Dibujar(_shaderProgram,Matrix4.Identity);

            _imguiController.Update(this, (float)args.Time);
            uiEditor.Dibujar(_escenario!);
            _imguiController.Render();

            SwapBuffers();
        }



        static void Main(string[] args)
        {
            Serializer _serializer = new Serializer();
            // CrearFigura fig = new CrearFigura();


            // List<Vertice> v1 = new List<Vertice>();
            // List<Vertice> v2 = new List<Vertice>();
            // List<Vertice> v3 = new List<Vertice>();

            // v1.Add(new Vertice(-5, 0, 0, 1, 1, 1));
            // v1.Add(new Vertice(5, 0, 0, 1, 1, 1));
            // v2.Add(new Vertice(0, -5, 0, 1, 1, 1));
            // v2.Add(new Vertice(0, 5, 0, 1, 1, 1));
            // v3.Add(new Vertice(0, 0, -5, 1, 1, 1));
            // v3.Add(new Vertice(0, 0, 5, 1, 1, 1));

            // Dictionary<string, Cara> Cara1 = new Dictionary<string, Cara>
            // {
            //     { "ejeX", new Cara("ejeX", v1, 0f, 0f, 0f) }
            // };

            // Dictionary<string, Cara> Cara2 = new Dictionary<string, Cara>
            // {
            //     { "ejeY", new Cara("ejeY", v2, 0f, 0f, 0f) }
            // };

            // Dictionary<string, Cara> Cara3 = new Dictionary<string, Cara>
            // {
            //     { "ejeZ", new Cara("ejeZ", v3, 0f, 0f, 0f) }
            // };

            // Dictionary<string, Parte> P1 = new Dictionary<string, Parte>
            // {
            //     { "parte_X", new Parte(Cara1, 0f, 0f, 0f) },
            //     { "parte_Y", new Parte(Cara2, 0f, 0f, 0f) },
            //     { "parte_Z", new Parte(Cara3, 0f, 0f, 0f) }
            // };

            // Objeto Ejes = new Objeto(P1, 5f, 2f, 2f);

            // Dictionary<string, Objeto> Dic_objetos = new Dictionary<string, Objeto>();

            // Objeto U1 = fig.CrearFiguraU(5f, 2f, 2f);
            // Objeto U2 = fig.CrearFiguraU(3f, 0f, 0f);
            // Objeto U3 = fig.CrearFiguraU(7f, 0f, 0f);

            // Dic_objetos.Add("U1", U1);
            // Dic_objetos.Add("U2", U2);
            // Dic_objetos.Add("U3", U3);
            // Dic_objetos.Add("ejes", Ejes);


            //Escenario escenario = new Escenario(Dic_objetos, 0, 0, 0);

            /*-------------------------------------------------------------------------------------*/

            Objeto AutoImportado = LectorModeloObj.ImportarOBJConMaterial("Modelos/CarV5/Chebroletobj.obj",-20.7f,4.3f,40);

            Objeto Arbol1 = LectorModeloObj.ImportarOBJConMaterial("Modelos/Tree/Tree_1.obj",60f,12f,40);
            Objeto Arbol2 = LectorModeloObj.ImportarOBJConMaterial("Modelos/Tree/Tree_2.obj",-10f,12f,-20);
            Objeto Arbol3 = LectorModeloObj.ImportarOBJConMaterial("Modelos/Tree/Tree_3.obj",20f,12f,-10);
            Objeto Arbol4 = LectorModeloObj.ImportarOBJConMaterial("Modelos/Tree/Tree_2.obj",60f,12f,-20);
            Objeto Arbol5 = LectorModeloObj.ImportarOBJConMaterial("Modelos/Tree/Tree_3.obj",20f,12f,-80);



            Objeto Carretera = LectorModeloObj.ImportarOBJConMaterial("Modelos/CarV5/Carretera.obj",0,0,0);
            Dictionary<string, Objeto> Dic_objetos = new Dictionary<string, Objeto>
            {
                { "Car", AutoImportado },
                { "Carretera", Carretera },
                { "Arbol1", Arbol1 },
                { "Arbol2", Arbol2 },
                { "Arbol3", Arbol3 },
                { "Arbol4", Arbol4 },
                { "Arbol5", Arbol5 }
            };
            Escenario escenario = new Escenario(Dic_objetos, 0, -5, 0);
            escenario.Objetos["Carretera"].Escalacion(5);
            escenario.Objetos["Arbol1"].Escalacion(5);
            escenario.Objetos["Arbol2"].Escalacion(5);
            escenario.Objetos["Arbol3"].Escalacion(5);
            escenario.Objetos["Arbol4"].Escalacion(5);
            escenario.Objetos["Arbol5"].Escalacion(5);
            escenario.Objetos["Car"].Rotacion(0,180,0);

            /*---------------------------------------------------------------------------------------*/

            // Escenario escenario;

            _serializer.GuardarAJson(escenario,"escenario.json");
            // escenario = (_serializer.CargarDesdeJson<Escenario>("escenario.json"))!;

            // escenario.Objetos["Carretera"].Traslacion(-23f,-1.5f,-24);
            // escenario.Objetos["Arbol1"].Traslacion(60f,-1.5f,40);
            // escenario.Objetos["Arbol2"].Traslacion(-10f,-1.5f,-20);
            // escenario.Objetos["Arbol3"].Traslacion(20f,-1.5f,-10);
            // escenario.Objetos["Arbol4"].Traslacion(60f,-1.5f,-20);
            // escenario.Objetos["Arbol5"].Traslacion(20f,-1.5f,-80);
            // escenario.Objetos["Car"].Traslacion(-20.7f,0f,40);



            // escenario.Objetos["Carretera"].Escalacion(5);
            // escenario.Objetos["Arbol1"].Escalacion(5);
            // escenario.Objetos["Arbol2"].Escalacion(5);
            // escenario.Objetos["Arbol3"].Escalacion(5);
            // escenario.Objetos["Arbol4"].Escalacion(5);
            // escenario.Objetos["Arbol5"].Escalacion(5);
            // escenario.Objetos["Car"].Rotacion(0,180,0);

            /*----------------------------------------------------------------------------------------------------*/

            LibretoAnimacion libreto;
            libreto = new LibretoAnimacion(escenario);
            // Ir recto 0-5
            libreto.AgregarInstruccion(new InstruccionAnimacion("Car", TipoTransformacion.Trasladar, 0f, 0, -60f, 0f, 5f)); //0-5

            // Ir a la derecha 5-10

            libreto.AgregarInstruccion(new InstruccionAnimacion("Car", TipoTransformacion.Trasladar, 3f, 0, -18f, 5f, 1.75f)); 
            libreto.AgregarInstruccion(new InstruccionAnimacion("Car", TipoTransformacion.Trasladar, 7f, 0, -9f, 6.75f, 1.33f)); 
            libreto.AgregarInstruccion(new InstruccionAnimacion("Car", TipoTransformacion.Trasladar, 11f, 0, -6f, 8.08f, 1.42f)); 
            libreto.AgregarInstruccion(new InstruccionAnimacion("Car", TipoTransformacion.Trasladar, 9f, 0, -1f, 9.5f, 1f)); 

            libreto.AgregarInstruccion(new InstruccionAnimacion("Car", TipoTransformacion.Trasladar, 9f, 0, 1f, 10.5f, 1f)); 
            libreto.AgregarInstruccion(new InstruccionAnimacion("Car", TipoTransformacion.Trasladar, 11f, 0, 6f, 11.5f, 1.42f)); 
            libreto.AgregarInstruccion(new InstruccionAnimacion("Car", TipoTransformacion.Trasladar, 7f, 0, 9f, 12.92f, 1.33f)); 
            libreto.AgregarInstruccion(new InstruccionAnimacion("Car", TipoTransformacion.Trasladar, 3f, 0, 18f, 14.25f, 1.75f)); 




             // rotar 
            libreto.AgregarInstruccion(new InstruccionAnimacion("Car", TipoTransformacion.Rotar, 0, -28f, 0, 5f, 1.75f));
            libreto.AgregarInstruccion(new InstruccionAnimacion("Car", TipoTransformacion.Rotar, 0, -25f, 0, 6.75f, 1.33f));
            libreto.AgregarInstruccion(new InstruccionAnimacion("Car", TipoTransformacion.Rotar, 0, -25f, 0, 8.08f, 1.42f));
            libreto.AgregarInstruccion(new InstruccionAnimacion("Car", TipoTransformacion.Rotar, 0, -12f, 0, 9.5f, 1f));

            libreto.AgregarInstruccion(new InstruccionAnimacion("Car", TipoTransformacion.Rotar, 0, -12f, 0, 10.5f, 1f));
            libreto.AgregarInstruccion(new InstruccionAnimacion("Car", TipoTransformacion.Rotar, 0, -25f, 0, 11.5f, 1.42f));
            libreto.AgregarInstruccion(new InstruccionAnimacion("Car", TipoTransformacion.Rotar, 0, -25f, 0, 12.92f, 1.33f));
            libreto.AgregarInstruccion(new InstruccionAnimacion("Car", TipoTransformacion.Rotar, 0, -28f, 0, 14.25f, 1.75f));




            // Ir abajo 10-15
            libreto.AgregarInstruccion(new InstruccionAnimacion("Car", TipoTransformacion.Trasladar, 0f, 0, 60f, 16f, 5f));



            _serializer.GuardarAJson(libreto, "Negocio/Animacion/libreto.json");
            
            /*-------------------------------------------------------------------------------------------------------*/

            //libreto = _serializer.CargarDesdeJson<LibretoAnimacion>("Negocio/Animacion/libreto.json")!;


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
                window.libreto = libreto;
                window.Run();
            }
        }

    }
}