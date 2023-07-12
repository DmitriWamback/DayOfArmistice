using Atmosphere.Source;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Collections.Generic;

namespace Atmosphere {

    class Atmosphere: GameWindow {

        struct RenderPass {
            Dictionary<string, FrameCapture> captures;
            Dictionary<string, Shader> shaders;
        }

        //RenderPass firstPass = new RenderPass();

        public Atmosphere(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            Run();
        }

        DebugQuad? quad;
        Shader? shader;

        protected override void OnLoad() {
            base.OnLoad();
            GL.Enable(EnableCap.DepthTest);

            shader = Shader.CreateShader("Planet");
            quad = new DebugQuad();
        }

        protected override void OnUpdateFrame(FrameEventArgs args) {
            base.OnUpdateFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(0f, 0f, 0f, 1f);

            quad!.Update(shader!);
            SwapBuffers();
        }
    }
}