using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

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

        public int framebufferObject, renderbufferObject, frameCaptureRenderTexture;


        public static FrameCapture Create() {

            FrameCapture capture = new FrameCapture();


            capture.framebufferObject = GL.GenFramebuffer();
            capture.renderbufferObject = GL.GenRenderbuffer();
            capture.frameCaptureRenderTexture = GL.GenTexture();

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, capture.framebufferObject);
            GL.BindTexture(TextureTarget.Texture2D, capture.frameCaptureRenderTexture);

            GL.TexImage2D(TextureTarget.Texture2D, 
                          0, 
                          PixelInternalFormat.Rgb, 
                          (int)Atmosphere.windowSize.X, 
                          (int)Atmosphere.windowSize.Y, 
                          0, 
                          PixelFormat.Rgb, 
                          PixelType.UnsignedByte, 0);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)ArbTextureMirroredRepeat.MirroredRepeatArb);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)ArbTextureMirroredRepeat.MirroredRepeatArb);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, capture.frameCaptureRenderTexture, 0);

            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, capture.renderbufferObject);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.Depth24Stencil8, (int)Atmosphere.windowSize.X, (int)Atmosphere.windowSize.Y);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, RenderbufferTarget.Renderbuffer, capture.renderbufferObject);

            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

            return capture;
        }


        public void Bind() {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, framebufferObject);
        }

        public static void Unbind() {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }
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


        public void SetMatrix4(string name, Matrix4 matrix) {
            GL.UniformMatrix4(GL.GetUniformLocation(program, name), false, ref matrix);
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