using OpenTK.Mathematics;

namespace Atmosphere.Source.Util.aMath {

    public class AtmUtilMath {

        public static Vector3 CalculateNormal(Vector3 a, Vector3 b, Vector3 c) {

            Vector3 Normal = new Vector3(0),
                    U = b - a,
                    V = c - a;

            Normal = new Vector3(
                (U.Y * V.Z) - (U.Z * V.Y),
                (U.Z * V.X) - (U.X * V.Z),
                (U.X * V.Y) - (U.Y * V.X)
            );
            return Normal.Normalized();
        }


        // Work in Progress....
        public static float[] RecalculateNormalsWithIndices(float[] vertices, int[] indices) {

            int nbVertices = indices.Length;
            Vector3[] triangleVertices = new Vector3[3];
            Vector3[] normals = new Vector3[indices.Length/3];

            float[] t_vertices = new float[(vertices.Length / 3) * 6];
            int index = 0;

            for (int k = 0; k < normals.Length; k++) { for (int i = 0; i < nbVertices/2; i++) {

                    float x = vertices[indices[index]*3  ],
                          y = vertices[indices[index]*3+1],
                          z = vertices[indices[index]*3+2];

                    triangleVertices[i] = new Vector3(x, y, z);
                    Console.WriteLine(triangleVertices[i].ToString());
                    index++;
                }
                normals[k] = AtmUtilMath.CalculateNormal(triangleVertices[0], triangleVertices[1], triangleVertices[2]);
            }
            index = 0;

            for (int k = 0; k < normals.Length; k++) { for (int i = 0; i < triangleVertices.Length; i++) {
                    
                int currentIndex = index;

                int xIndex  = indices[currentIndex]*6,      yIndex  = indices[currentIndex]*6+1,    zIndex  = indices[currentIndex]*6+2,
                    nxIndex = indices[currentIndex]*6+3,    nyIndex = indices[currentIndex]*6+4,    nzIndex = indices[currentIndex]*6+5;

                t_vertices[xIndex] = triangleVertices[i].X;
                t_vertices[yIndex] = triangleVertices[i].Y;
                t_vertices[zIndex] = triangleVertices[i].Z;
                t_vertices[nxIndex] = normals[k].X;
                t_vertices[nyIndex] = normals[k].Y;
                t_vertices[nzIndex] = normals[k].Z;
                index++;
            }}

            for (int i = 0; i < t_vertices.Length; i++) {
                if (i % 6 == 0) Console.WriteLine("Fuck");
                Console.Write(t_vertices[i] + " ");
            }
            return t_vertices;
        }


        // Only works if the vertices are rendered Counter-clockwise
        public static float[] RecalculateNormals(float[] vertices, int vertexArraySize, int vertexCompositionSize) {
            
            // VertexCompositionSize = number of elements making up a vertex
            // VertexArraySize = number of values attributed to the physical position of a vertex

            float collumSize = (float)vertices.Length/(float)vertexCompositionSize;
            if ((int)collumSize != collumSize) throw new FieldAccessException("Invalid number of vertices");

            int vertexCollumnCount = vertices.Length/vertexCompositionSize;
            int triangleCount = vertexCollumnCount / 3;

            List<float> computedVertices = new List<float>();
            Vector3[] normals = new Vector3[triangleCount],
                      vecvertices = new Vector3[vertexCollumnCount];

            // Calculating the surface normals

            int collumnIndex = 0;
            for (int trio = 0; trio < triangleCount; trio++) {
                
                Vector3[] triangle = new Vector3[3];
                int currentTriangleCollum = trio * vertexCompositionSize;

                for (int collumn = 0; collumn < 3; collumn++) {
                    int vertexArrayBeginning = collumn * vertexCompositionSize + trio * 9;
                    float x = vertices[vertexArrayBeginning],
                          y = vertices[vertexArrayBeginning+1],
                          z = vertices[vertexArrayBeginning+2];

                    triangle[collumn] = new Vector3(x, y, z);
                    vecvertices[collumnIndex] = triangle[collumn];
                    collumnIndex++;
                }
                normals[trio] = AtmUtilMath.CalculateNormal(triangle[0], triangle[1], triangle[2]);
            }
            int nbUniqueVertices = vecvertices.Length / normals.Length;

            // Combining the vertices and surface normals

            for (int i = 0; i < normals.Length; i++) {
                for (int k = 0; k < nbUniqueVertices; k++) {

                    Vector3 vertex = vecvertices[k + i * 3],
                            surfaceNormal = normals[i];
                    
                    computedVertices.Add(vertex.X);
                    computedVertices.Add(vertex.Y);
                    computedVertices.Add(vertex.Z);
                    computedVertices.Add(surfaceNormal.X);
                    computedVertices.Add(surfaceNormal.Y);
                    computedVertices.Add(surfaceNormal.Z);
                }
            }

            return computedVertices.ToArray();
        }


        public static void AddUVCoordsToVertexList(float[] vertices, int[] indices, float[] uvInRelationToIndices) {


        }

        public static Matrix4 EulerRotation(Vector3 rotation) {

            float x = rotation.X;
            float y = rotation.Y;
            float z = rotation.Z;

            Matrix4 xRotation = new Matrix4(
                new Vector4(MathF.Cos(x), -MathF.Sin(x), 0, 0),
                new Vector4(MathF.Sin(x),  MathF.Cos(x), 0, 0),
                new Vector4(0,             0,            1, 0),
                new Vector4(0,             0,            0, 1)
            );
            Matrix4 yRotation = new Matrix4(
                new Vector4( MathF.Cos(y), 0, MathF.Sin(y), 0),
                new Vector4( 0,            1, 0,            0),
                new Vector4(-MathF.Sin(y), 0, MathF.Cos(y), 0),
                new Vector4( 0,            0, 0,            1)
            );
            Matrix4 zRotation = new Matrix4(
                new Vector4(1, 0,             0,            0),
                new Vector4(0, MathF.Cos(z), -MathF.Sin(z), 0),
                new Vector4(0, MathF.Sin(z),  MathF.Cos(z), 0),
                new Vector4(0, 0,             0,            1)
            );

            return xRotation * yRotation * zRotation;
        }

        public static Vector3 PointToSphere(float latitude, float longitude) {

            float y =  (float)Math.Sin(latitude * 3.14159265358797f / 180f);
            float r =  (float)Math.Cos(latitude * 3.14159265358797f / 180f);
            float x =  (float)Math.Sin(longitude * 3.14159265358797f / 180f) * r;
            float z = -(float)Math.Cos(longitude * 3.14159265358797f / 180f) * r;

            return new Vector3(z, y, x);
        }
    }
}