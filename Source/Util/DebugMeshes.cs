using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using Atmosphere.Source.Util.aMath;

namespace Atmosphere.Source {    
    
    public class DebugQuad: RenderableObject {

        public DebugQuad() {

            vertices = new float[] {
                -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, 
                 0.5f,  0.5f, 0.0f, 1.0f, 1.0f,
                 0.5f, -0.5f, 0.0f, 1.0f, 0.0f,

                -0.5f, -0.5f, 0.0f, 0.0f, 0.0f,
                -0.5f,  0.5f, 0.0f, 0.0f, 1.0f,
                 0.5f,  0.5f, 0.0f, 1.0f, 1.0f,
            };
            //vertices = AtmUtilMath.RecalculateNormals(vertices, 3, 3);

            renderbuffer = Renderbuffer.Create();

            GL.BindVertexArray(renderbuffer.vertexArrayObject);
            GL.BindBuffer(BufferTarget.ArrayBuffer, renderbuffer.vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
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

    public class DebugSphere: RenderableObject {

        public DebugSphere() {

            List<float> points = new List<float>();

            int[] offsets = {
                1, 1,
                0, 1,
                0, 0,

                0, 0,
                1, 0,
                1, 1,
            };

            for (int i = -180; i <= 180; i++) { for (int j = -90; j <= 90; j++) {
                    
                float longitude = (float)i;
                float latitude  = (float)j;

                for (int o = 0; o < offsets.Length/2; o++) {
                    Vector3 pointOnSphere1 = AtmUtilMath.PointToSphere(latitude + offsets[o * 2 + 1], longitude + offsets[o * 2]);
                    points.Add(pointOnSphere1.X);
                    points.Add(pointOnSphere1.Y);
                    points.Add(pointOnSphere1.Z);
                }
            }
            }
            
            vertices = AtmUtilMath.RecalculateNormals(points.ToArray(), 3, 3);

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
            GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Length/3);
        }
    }
}