using OpenTK.Mathematics;

namespace Atmosphere.Source.Camera {

    public class Camera {

        public static Vector3 position, lookAtDirection, oribitingPosition;
        public static Matrix4 projection, lookAt;
        public static float speed = 10, radius = 1.3f, xRotation = 0, yRotation = 0;

        public static void Initialize() {

            position = new Vector3(1);
            oribitingPosition = new Vector3(0, 0, 0);

            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(110), 1f, 0.1f, 1000f);
            lookAt = Matrix4.Identity;
        }

        public static void Update(Vector2 deltaRotation) {
            
            xRotation += deltaRotation.X * 0.005f;
            yRotation -= deltaRotation.Y * 0.005f;

            if (yRotation > 1.54362f) yRotation = 1.54362f;
            if (yRotation < -1.54362f) yRotation = -1.54362f;

            position = new Vector3(MathF.Sin(xRotation) * MathF.Cos(yRotation) * radius,
                                   MathF.Sin(yRotation) * radius,
                                   MathF.Cos(xRotation) * MathF.Cos(yRotation) * radius);

            oribitingPosition = new Vector3(0f, 0f, 0f);
        }

        public static void Move(Vector2 frontRightMotion) {

            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(110), (float)Atmosphere.windowSize.X / (float)Atmosphere.windowSize.Y, 0.1f, 1000f);

            Vector2 normalizedMotion = frontRightMotion.Normalized();
            float frontSpeed = normalizedMotion.X * speed,
                  rightSpeed = normalizedMotion.Y * speed;

            Vector3 rightMotion = Vector3.Cross(lookAtDirection, Vector3.UnitY).Normalized();
            //oribitingPosition += lookAtDirection * frontSpeed + rightMotion * rightSpeed;
            lookAt = Matrix4.LookAt(position, oribitingPosition, Vector3.UnitY);
        }
    }
}