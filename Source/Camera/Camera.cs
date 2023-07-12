using OpenTK.Mathematics;

namespace Atmosphere.Source.Camera {

    public class Camera {

        public static Vector3 position, lookAtDirection, oribitingPosition;
        public static Matrix4 projection, lookAt;


        public static void Initialize() {

            position = new Vector3(1);
            oribitingPosition = new Vector3(0);
        }

        public static void Update() {


        }


    }
}