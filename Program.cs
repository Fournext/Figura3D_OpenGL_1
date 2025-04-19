using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Linq;

namespace OpenTKCubo3D
{
    class Program : GameWindow
    {
        private float _angleY; 
        private float _angleX; 
        
        private Matrix4 _view;
        private Matrix4 _projection;
        public Escenario? _escenario;
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
            // En InicializarShaders(), cambia el vertex shader a:
            string vertexShader = @"#version 330 core
                layout(location=0) in vec3 aPosition;
                layout(location=1) in vec3 aColor;
                out vec3 fragColor;
                uniform mat4 view;
                uniform mat4 projection;
                uniform mat4 rotacionGlobal;
                
                void main() {
                    vec4 pos = rotacionGlobal * vec4(aPosition, 1.0); 
                    gl_Position = projection * view * pos;
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
            
            Matrix4 view = Matrix4.LookAt(new Vector3(5, 5, 7), _escenario.CentroMasa, Vector3.UnitY);
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, Size.X / (float)Size.Y, 0.1f, 100f);
            
            GL.UseProgram(ShaderProgram);
            GL.UniformMatrix4(GL.GetUniformLocation(ShaderProgram, "view"), true, ref view);
            GL.UniformMatrix4(GL.GetUniformLocation(ShaderProgram, "projection"), false, ref projection);
            
            _escenario?.Dibujar();
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

            var serialicer = new Serialicer();

            using (var window = new Program(GameWindowSettings.Default, nativeWindowSettings))
            {   
                window.OnLoadCustom = shaderProgram =>
                {   
                    // var _escenario = new Escenario(Vector3.Zero, ShaderProgram);

                    // // 1) Definición del diccionario de vértices
                    // var vertices = new Dictionary<string, Vertice>
                    // {
                    //     // Base
                    //     //Abajo
                    //     {"v0", new Vertice(new Vector3(0,    0,   0),   new Vector3(1,0,0))},
                    //     {"v1", new Vertice(new Vector3(2.0f, 0,   0),   new Vector3(0,1,0))},
                    //     {"v2", new Vertice(new Vector3(2.0f, 0,  -0.5f),new Vector3(0,0,1))},
                    //     {"v3", new Vertice(new Vector3(0,    0,  -0.5f),new Vector3(1,0,0))},
                        
                    //     //Izquierda
                    //     {"v4", new Vertice(new Vector3(0,    0.5f,   0),   new Vector3(1,0,0))},
                    //     {"v5", new Vertice(new Vector3(0,    0.5f,  -0.5f),   new Vector3(1,0,0))},
                    //     {"v6", new Vertice(new Vector3(0,    0,      -0.5f),   new Vector3(1,0,0))},
                    //     {"v7", new Vertice(new Vector3(0,    0,   0),   new Vector3(1,0,0))},

                    //     //En frente
                    //     {"v8", new Vertice(new Vector3(0,    0.5f,   0),   new Vector3(1,0,0))},
                    //     {"v9", new Vertice(new Vector3(2.0f,    0.5f,   0),   new Vector3(1,1,0))},
                    //     {"v10", new Vertice(new Vector3(2.0f,    0,   0),   new Vector3(1,0,0))},
                    //     {"v11", new Vertice(new Vector3(0,    0,   0),   new Vector3(1,0,0))},

                    //     //Derecha
                    //     {"v12", new Vertice(new Vector3(2f,    0,   0),   new Vector3(1,0,0))},
                    //     {"v13", new Vertice(new Vector3(2f,    0.5f,   0),   new Vector3(1,0,0))},
                    //     {"v14", new Vertice(new Vector3(2.0f,    0.5f,   -0.5f),   new Vector3(1,0,0))},
                    //     {"v15", new Vertice(new Vector3(2.0f,    0,   -0.5f),   new Vector3(1,0,0))},

                    //     //Atras
                    //     {"v16", new Vertice(new Vector3(0,    0.5f,   -0.5f),   new Vector3(1,0,0))},
                    //     {"v17", new Vertice(new Vector3(2.0f,    0.5f,   -0.5f),   new Vector3(1,0,0))},
                    //     {"v18", new Vertice(new Vector3(2.0f,    0,   -0.5f),   new Vector3(1,0,0))},
                    //     {"v19", new Vertice(new Vector3(0,    0,   -0.5f),   new Vector3(1,0,0))},



                    //     // Primer Pilar
                    //     //Enfrente
                    //     {"v20", new Vertice(new Vector3(0,    0.5f,   0),   new Vector3(0,1,0))},
                    //     {"v21", new Vertice(new Vector3(0,    2.0f,  0),new Vector3(0,0,1))},
                    //     {"v22", new Vertice(new Vector3(0.35f,2.0f,  0),   new Vector3(1,0,0))},
                    //     {"v23", new Vertice(new Vector3(0.35f,0.5f,  0),   new Vector3(0,0,1))},

                    //     //Izquierda
                    //     {"v24", new Vertice(new Vector3(0,   0.5f,   0),   new Vector3(0,1,0))},
                    //     {"v25", new Vertice(new Vector3(0,  2.0f,  0),new Vector3(0,0,1))},
                    //     {"v26", new Vertice(new Vector3(0,  2.0f,  -0.5f),   new Vector3(1,0,0))},
                    //     {"v27", new Vertice(new Vector3(0   ,0.5f,  -0.5f),   new Vector3(0,0,1))},


                    //     //Atras
                    //     {"v28", new Vertice(new Vector3(0,    0.5f,   -0.5f),   new Vector3(0,1,0))},
                    //     {"v29", new Vertice(new Vector3(0,    2.0f,  -0.5f),new Vector3(0,0,1))},
                    //     {"v30", new Vertice(new Vector3(0.35f,2.0f,  -0.5f),   new Vector3(1,0,0))},
                    //     {"v31", new Vertice(new Vector3(0.35f,0.5f,  -0.5f),   new Vector3(0,0,1))},


                    //     //Derecha
                    //     {"v32", new Vertice(new Vector3(0.35f,  0.5f,   0),   new Vector3(0,1,0))},
                    //     {"v33", new Vertice(new Vector3(0.35f,  2.0f,  0),new Vector3(0,0,1))},
                    //     {"v34", new Vertice(new Vector3(0.35f,  2.0f,  -0.5f),   new Vector3(1,0,0))},
                    //     {"v35", new Vertice(new Vector3(0.35f,  0.5f,  -0.5f),   new Vector3(0,0,1))},

                    //     //Arriba
                    //     {"v36", new Vertice(new Vector3(0,    2.0f,   0),   new Vector3(0,1,0))},
                    //     {"v37", new Vertice(new Vector3(0,    2.0f,  -0.5f),new Vector3(0,0,1))},
                    //     {"v38", new Vertice(new Vector3(0.35f,   2.0f,  -0.5f),   new Vector3(1,0,0))},
                    //     {"v39", new Vertice(new Vector3(0.35f, 2.0f,  0),   new Vector3(0,0,1))},


                    //     // Segundo Pilar
                    //     //Enfrente
                    //     {"v40", new Vertice(new Vector3(1.65f,    0.5f,   0),   new Vector3(0,1,0))},
                    //     {"v41", new Vertice(new Vector3(1.65f,    2.0f,  0),new Vector3(0,0,1))},
                    //     {"v42", new Vertice(new Vector3(2f,     2.0f,  0),   new Vector3(1,0,0))},
                    //     {"v43", new Vertice(new Vector3(2f,      0.5f,  0),   new Vector3(0,0,1))},

                    //     //Izquierda
                    //     {"v44", new Vertice(new Vector3(1.65f,  0.50f,   0),   new Vector3(0,1,0))},
                    //     {"v45", new Vertice(new Vector3(1.65f,  2.0f,  0),new Vector3(0,0,1))},
                    //     {"v46", new Vertice(new Vector3(1.65f,  2.0f,  -0.5f),   new Vector3(1,0,0))},
                    //     {"v47", new Vertice(new Vector3(1.65f,  0.5f,  -0.5f),   new Vector3(0,0,1))},


                    //     //Atras
                    //     {"v48", new Vertice(new Vector3(1.65f,    0.5f,   -0.5f),   new Vector3(0,1,0))},
                    //     {"v49", new Vertice(new Vector3(1.65f,    2.0f,  -0.5f),new Vector3(0,0,1))},
                    //     {"v50", new Vertice(new Vector3(2f,2.0f,  -0.5f),   new Vector3(1,0,0))},
                    //     {"v51", new Vertice(new Vector3(2f,0.5f,  -0.5f),   new Vector3(0,0,1))},


                    //     //Derecha
                    //     {"v52", new Vertice(new Vector3(2f,  0.5f,   0),   new Vector3(0,1,0))},
                    //     {"v53", new Vertice(new Vector3(2f,  2.0f,  0),new Vector3(0,0,1))},
                    //     {"v54", new Vertice(new Vector3(2f,  2.0f,  -0.5f),   new Vector3(1,0,0))},
                    //     {"v55", new Vertice(new Vector3(2f,  0.5f,  -0.5f),   new Vector3(0,0,1))},

                    //     //Arriba
                    //     {"v56", new Vertice(new Vector3(1.65f,    2.0f,   0),   new Vector3(0,1,0))},
                    //     {"v57", new Vertice(new Vector3(1.65f,    2.0f,  -0.5f),new Vector3(0,0,1))},
                    //     {"v58", new Vertice(new Vector3(2f,   2.0f,  -0.5f),   new Vector3(1,0,0))},
                    //     {"v59", new Vertice(new Vector3(2f, 2.0f,  0),   new Vector3(0,0,1))},

                    // };

                    // var parteBase = new Parte(Vector3.Zero);

                    // var verticesBase1 = new List<Vertice>
                    // {
                    //     vertices["v0"], vertices["v1"],
                    //     vertices["v1"], vertices["v2"],
                    //     vertices["v2"], vertices["v3"],
                    //     vertices["v3"], vertices["v0"]
                    // };
                    // var caraBase1 = new Cara(verticesBase1.Select((v, i) => new KeyValuePair<string, Vertice>($"Abajo{i}", v)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
                    // parteBase.AgregarCara("Abajo", caraBase1);
                   
                    // var verticesBase2 = new List<Vertice>
                    // {
                    //     vertices["v4"], vertices["v5"],
                    //     vertices["v5"], vertices["v6"],
                    //     vertices["v6"], vertices["v7"],
                    //     vertices["v7"], vertices["v4"]
                    // };
                    // var caraBase2 = new Cara(verticesBase2.Select((v, i) => new KeyValuePair<string, Vertice>($"Izquierda{i}", v)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
                    //  parteBase.AgregarCara("Izquierda", caraBase2);

                    // var verticesBase3 = new List<Vertice>
                    // {
                    //     vertices["v8"], vertices["v9"],
                    //     vertices["v9"], vertices["v10"],
                    //     vertices["v10"], vertices["v11"],
                    //     vertices["v11"], vertices["v8"]
                    // };
                    // var caraBase3 = new Cara(verticesBase3.Select((v, i) => new KeyValuePair<string, Vertice>($"Frente{i}", v)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
                    // parteBase.AgregarCara("Frente", caraBase2);

                    // var verticesBase4 = new List<Vertice>
                    // {
                    //     vertices["v12"], vertices["v13"],
                    //     vertices["v13"], vertices["v14"],
                    //     vertices["v14"], vertices["v15"],
                    //     vertices["v15"], vertices["v12"]
                    // };
                    // var caraBase4 = new Cara(verticesBase4.Select((v, i) => new KeyValuePair<string, Vertice>($"Derecha{i}", v)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
                    // parteBase.AgregarCara("Derecha", caraBase4);

                    // var verticesBase5 = new List<Vertice>
                    // {
                    //     vertices["v16"], vertices["v17"],
                    //     vertices["v17"], vertices["v18"],
                    //     vertices["v18"], vertices["v19"],
                    //     vertices["v19"], vertices["v16"]
                    // };
                    // var caraBase5 = new Cara(verticesBase5.Select((v, i) => new KeyValuePair<string, Vertice>($"Atras{i}", v)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
                    // parteBase.AgregarCara("Atras", caraBase5);                    
                    
                    
                    // // Pilar1
                    // var partePilar1 = new Parte(new Vector3(0f, 0.0f, 0f));

                    // var verticesPilar1_1 = new List<Vertice>
                    // {
                    //     vertices["v20"], vertices["v21"],  
                    //     vertices["v21"], vertices["v22"],  
                    //     vertices["v22"], vertices["v23"],  
                    //     vertices["v23"], vertices["v20"],  
                    // };
                    // var caraPilar1_1 = new Cara(verticesPilar1_1.Select((v, i) => new KeyValuePair<string, Vertice>($"pilar1_Frente{i}", v)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
                    // partePilar1.AgregarCara("Frente", caraPilar1_1); 

                    // var verticesPilar1_2 = new List<Vertice>
                    // {
                    //     vertices["v24"], vertices["v25"],  
                    //     vertices["v25"], vertices["v26"],  
                    //     vertices["v26"], vertices["v27"],  
                    //     vertices["v27"], vertices["v24"],  
                    // };
                    // var caraPilar1_2 = new Cara(verticesPilar1_2.Select((v, i) => new KeyValuePair<string, Vertice>($"pilar1_Izquierda{i}", v)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
                    // partePilar1.AgregarCara("Izquierda", caraPilar1_2); 

                    // var verticesPilar1_3 = new List<Vertice>
                    // {
                    //     vertices["v28"], vertices["v29"],  
                    //     vertices["v29"], vertices["v30"],  
                    //     vertices["v30"], vertices["v31"],  
                    //     vertices["v31"], vertices["v28"],  
                    // };
                    // var caraPilar1_3 = new Cara(verticesPilar1_3.Select((v, i) => new KeyValuePair<string, Vertice>($"pilar1_Atras{i}", v)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
                    // partePilar1.AgregarCara("Atras", caraPilar1_3); 


                    // var verticesPilar1_4 = new List<Vertice>
                    // {
                    //     vertices["v32"], vertices["v33"],  
                    //     vertices["v33"], vertices["v34"],  
                    //     vertices["v34"], vertices["v35"],  
                    //     vertices["v35"], vertices["v32"],  
                    // };
                    // var caraPilar1_4 = new Cara(verticesPilar1_4.Select((v, i) => new KeyValuePair<string, Vertice>($"pilar1_Derecha{i}", v)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
                    // partePilar1.AgregarCara("Derecha", caraPilar1_4); 

                    // var verticesPilar1_5 = new List<Vertice>
                    // {
                    //     vertices["v36"], vertices["v37"],  
                    //     vertices["v37"], vertices["v38"],  
                    //     vertices["v38"], vertices["v39"],  
                    //     vertices["v39"], vertices["v36"],  
                    // };
                    // var caraPilar1_5 = new Cara(verticesPilar1_5.Select((v, i) => new KeyValuePair<string, Vertice>($"pilar1_Arriba{i}", v)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
                    // partePilar1.AgregarCara("Arriba", caraPilar1_5); 



                    // //Pilar 2


                    // var partePilar2 = new Parte(new Vector3(0f, 0f, 0f));

                    // var verticesPilar2_1 = new List<Vertice>
                    // {
                    //     vertices["v40"], vertices["v41"],  
                    //     vertices["v41"], vertices["v42"],  
                    //     vertices["v42"], vertices["v43"],  
                    //     vertices["v43"], vertices["v40"],  
                    // };
                    // var caraPilar2_1 = new Cara(verticesPilar2_1.Select((v, i) => new KeyValuePair<string, Vertice>($"pilar2_Frente{i}", v)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
                    // partePilar2.AgregarCara("Frente", caraPilar2_1); 

                    // var verticesPilar2_2 = new List<Vertice>
                    // {
                    //     vertices["v44"], vertices["v45"],  
                    //     vertices["v45"], vertices["v46"],  
                    //     vertices["v46"], vertices["v47"],  
                    //     vertices["v47"], vertices["v44"],  
                    // };
                    // var caraPilar2_2 = new Cara(verticesPilar2_2.Select((v, i) => new KeyValuePair<string, Vertice>($"pilar2_Izquierda{i}", v)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
                    // partePilar2.AgregarCara("Izquierda", caraPilar2_2); 

                    // var verticesPilar2_3 = new List<Vertice>
                    // {
                    //     vertices["v48"], vertices["v49"],  
                    //     vertices["v49"], vertices["v50"],  
                    //     vertices["v50"], vertices["v51"],  
                    //     vertices["v51"], vertices["v48"],  
                    // };
                    // var caraPilar2_3 = new Cara(verticesPilar2_3.Select((v, i) => new KeyValuePair<string, Vertice>($"pilar2_Atras{i}", v)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
                    // partePilar2.AgregarCara("Atras", caraPilar2_3); 


                    // var verticesPilar2_4 = new List<Vertice>
                    // {
                    //     vertices["v52"], vertices["v53"],  
                    //     vertices["v53"], vertices["v54"],  
                    //     vertices["v54"], vertices["v55"],  
                    //     vertices["v55"], vertices["v52"],  
                    // };
                    // var caraPilar2_4 = new Cara(verticesPilar2_4.Select((v, i) => new KeyValuePair<string, Vertice>($"pilar2_Derecha{i}", v)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
                    // partePilar2.AgregarCara("Derecha", caraPilar2_4); 

                    // var verticesPilar2_5 = new List<Vertice>
                    // {
                    //     vertices["v56"], vertices["v57"],  
                    //     vertices["v57"], vertices["v58"],  
                    //     vertices["v58"], vertices["v59"],  
                    //     vertices["v59"], vertices["v56"],  
                    // };
                    // var caraPilar2_5 = new Cara(verticesPilar2_5.Select((v, i) => new KeyValuePair<string, Vertice>($"pilar2_Arriba{i}", v)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
                    // partePilar2.AgregarCara("Arriba", caraPilar2_5); 

                    // var ObjetoU2 = new Objeto(new Vector3(2.5f,0,0));
                    // var objetoU = new Objeto(Vector3.Zero);
                    // objetoU.AgregarParte("base", parteBase);
                    // objetoU.AgregarParte("pilar1", partePilar1);
                    // objetoU.AgregarParte("pilar2", partePilar2);
                    
                    
                    // ObjetoU2.AgregarParte("base", parteBase);
                    // ObjetoU2.AgregarParte("pilar1", partePilar1);
                    // ObjetoU2.AgregarParte("pilar2", partePilar2);


                    // _escenario.AgregarObjeto("U", objetoU);
                    // _escenario.AgregarObjeto("U2", ObjetoU2);

                    // serialicer.GuardarAJson(_escenario, "escenario.json");
                    

                    var escenario2 = serialicer.CargarDesdeJson<Escenario>("escenario.json");
                    escenario2.Rotar(MathHelper.DegreesToRadians(45),0,0);
                    //escenario2.Objeto("U").Rotar(MathHelper.DegreesToRadians(30),0,0);
                    //escenario2.Objeto("U").Parte("pilar1").Rotar(0,MathHelper.DegreesToRadians(30),0);
                    //escenario2.Objeto("U").Trasladar(MathHelper.DegreesToRadians(-30),0,0);
                    //escenario2.Objeto("U").Parte("pilar1").Cara("Izquierda").Escalar(2);
                    //escenario2.Objeto("U").Rotar(MathHelper.DegreesToRadians(30),MathHelper.DegreesToRadians(30),0);
                    //_escenario2.Objeto("U").Parte("pilar1").Rotar(MathHelper.DegreesToRadians(15), 0, 0);
                    //_escenario2.Objeto("U").Parte("pilar1").Cara("pilar1").Rotar(MathHelper.DegreesToRadians(15), 0, 0);
                    escenario2.SetShaderProgram(shaderProgram ?? 0);

                    // Asigna al campo de instancia para que se use en render
                    window._escenario = escenario2;
                };
                window.Run();
            }
        }
    }
}