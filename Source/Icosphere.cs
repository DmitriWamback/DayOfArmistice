using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using Atmosphere.Source.Util.aMath;

namespace Atmosphere.Source {

    public class RenderableObject {

        public Renderbuffer? renderbuffer;
        public float[] vertices = {};
        public int[] indices = {};
        public virtual void Update(Shader shader) {}
    }

    public class Icosphere: RenderableObject {

    }
}