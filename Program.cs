using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenTK.Mathematics;

namespace Atmosphere {

    class Program {


        public void Begin() {
            GameWindowSettings gameWindowSettings = new GameWindowSettings();

            NativeWindowSettings nativeWindowSettings = new NativeWindowSettings() {
                APIVersion      = new Version(4, 1),
                Profile         = ContextProfile.Core,
                Flags           = ContextFlags.ForwardCompatible,
                Title           = "Atmosphere",
                Size            = new Vector2i(1200, 800),
                NumberOfSamples = 14
            };

            new Atmosphere(gameWindowSettings, nativeWindowSettings);
        }

        public static void Main() {
            new Program().Begin();
        }
    }
}
