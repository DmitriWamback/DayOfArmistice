using OpenTK.Graphics.OpenGL4;

namespace Atmosphere.Source {


    public class Renderbuffer {

        public int vertexArrayObject, vertexBufferObject, indexBufferObject;


        public static Renderbuffer Create() {
            Renderbuffer buf = new Renderbuffer();

            buf.vertexArrayObject   = GL.GenVertexArray();
            buf.vertexBufferObject  = GL.GenBuffer();
            buf.indexBufferObject   = GL.GenBuffer();

            return buf;
        }
    }

    public class FrameCapture {

        public int framebufferObject, renderbufferObject;
    }










    public class Shader {

        public int program;

        public static Shader CreateShader(string shaderFolderPath) {

            Shader shader = new Shader();

            string vertexShaderSource   = LoadShaderSource("Shaders/" + shaderFolderPath + "/vMain.glsl"),
                   fragmentShaderSource = LoadShaderSource("Shaders/" + shaderFolderPath + "/fMain.glsl");

            int vertex      = CreateAndCompileShaderWithSource(vertexShaderSource,   ShaderType.VertexShader),
                fragment    = CreateAndCompileShaderWithSource(fragmentShaderSource, ShaderType.FragmentShader);
            
            GetShaderInfoLog(vertex);
            GetShaderInfoLog(fragment);

            shader.program = GL.CreateProgram();
            GL.AttachShader(shader.program, vertex);
            GL.AttachShader(shader.program, fragment);
            GL.LinkProgram(shader.program);
            Console.WriteLine(GL.GetProgramInfoLog(shader.program));
            return shader;
        }

        public void BindShaderProgram() {
            GL.UseProgram(program);
        }
        public static void UnbindShaderProgram() {
            GL.UseProgram(0);
        }

        private static string LoadShaderSource(string shaderFolderPath) {

            return new StreamReader(shaderFolderPath).ReadToEnd();
        }
        private static int CreateAndCompileShaderWithSource(string shaderSource, ShaderType _type) {
            
            int shaderProgram = GL.CreateShader(_type);
            GL.ShaderSource(shaderProgram, shaderSource);
            GL.CompileShader(shaderProgram);
        
            return shaderProgram;
        }
        private static void GetShaderInfoLog(int shader) {

            Console.WriteLine(GL.GetShaderInfoLog(shader));
        }
    }
}