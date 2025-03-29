using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.GraphicsLibraryFramework;


namespace fig
{
    class Figura
    {
        private int _vertexBufferObject;
        private int _vertexArrayObject;

        private int _shaderProgram;
        private Matrix4 _view;
        private Matrix4 _projection;

        //Vertices modificables
        private float Vx;
        private float Vy;
        private float Vz; 

        private int longVer;

        public Figura(float Punto_X, float Punto_Y, float Punto_Z){
            Vx = Punto_X;
            Vy = Punto_Y;
            Vz = Punto_Z;
        }

        public Figura(){}

        public void CargarVertices(float[] vrt,Matrix4 view, Matrix4 projection){

            longVer = vrt.Length / 6;
            
            float[] vertices = new float[vrt.Length];
            Array.Copy(vrt, vertices, vertices.Length);

            for (int i = 0; i < vertices.Length ; i+=6)
            {
                vertices[i] =vertices[i] + Vx;     // X
                vertices[i + 1] = vertices[i + 1] + Vy; // Y
                vertices[i + 2] = vertices[i + 2] + Vz; // Z

            }
            
            

            _view = view;
            _projection = projection;

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

            // Enlazar las matrices al shader
            GL.UseProgram(_shaderProgram);

            int viewLocation = GL.GetUniformLocation(_shaderProgram, "view");
            GL.UniformMatrix4(viewLocation, false, ref view);

            int projectionLocation = GL.GetUniformLocation(_shaderProgram, "projection");
            GL.UniformMatrix4(projectionLocation, false, ref projection);
            
        }


        public void Dibujar_ConRotacion(float _angleX, float _angleY){
            GL.UseProgram(_shaderProgram);
            GL.BindVertexArray(_vertexArrayObject);
            
            Matrix4 model = Matrix4.CreateRotationY(_angleY) * Matrix4.CreateRotationX(_angleX);
            int modelLocation = GL.GetUniformLocation(_shaderProgram, "model");
            GL.UniformMatrix4(modelLocation, false, ref model);
            
            int viewLocation = GL.GetUniformLocation(_shaderProgram, "view");
            GL.UniformMatrix4(viewLocation, false, ref _view);
            
            int projectionLocation = GL.GetUniformLocation(_shaderProgram, "projection");
            GL.UniformMatrix4(projectionLocation, false, ref _projection);
            
            GL.DrawArrays(PrimitiveType.Lines, 0, longVer);
        }

        public void Dibujar_SinRotacion(){
            GL.UseProgram(_shaderProgram);
            Matrix4 identityMatrix = Matrix4.Identity;
            GL.UniformMatrix4(GL.GetUniformLocation(_shaderProgram, "model"), false, ref identityMatrix);
            GL.BindVertexArray(_vertexArrayObject);
            GL.DrawArrays(PrimitiveType.Lines, 0, longVer);
        }


    }

}