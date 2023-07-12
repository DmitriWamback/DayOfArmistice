using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using Atmosphere.Source.Util.Math;

namespace Atmosphere.Source {

    public class RenderableObject {

        public Renderbuffer? renderbuffer;
        public float[] vertices = {};
        public int[] indices = {};
        public virtual void Update(Shader shader) {}
    }

    public class Icosphere: RenderableObject {

    }

    public class DebugQuad: RenderableObject {

        public DebugQuad() {

            vertices = new float[] {
                -0.5f, -0.5f, 0.0f,
                 0.5f,  0.5f, 0.0f,
                 0.5f, -0.5f, 0.0f,

                -0.5f, -0.5f, 0.0f,
                -0.5f,  0.5f, 0.0f,
                 0.5f,  0.5f, 0.0f,
            };
            vertices = AtmUtilMath.RecalculateNormals(vertices, 3, 3);

            renderbuffer = Renderbuffer.Create();

            GL.BindVertexArray(renderbuffer.vertexArrayObject);
            GL.BindBuffer(BufferTarget.ArrayBuffer, renderbuffer.vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.BindVertexArray(0);
        }

        public override void Update(Shader shader) {
            base.Update(shader);

            shader.BindShaderProgram();
            GL.BindVertexArray(renderbuffer!.vertexArrayObject);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
        }
    }
}