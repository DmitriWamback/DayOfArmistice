using Atmosphere.Source;
using Atmosphere.Source.Camera;

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

        public static Vector2 windowSize;

        //RenderPass firstPass = new RenderPass();

        public Atmosphere(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            windowSize = nativeWindowSettings.Size;
            Run();
        }

        RenderableObject? quad, sphere;
        Shader? MainShader, FrameCaptureShader;
        FrameCapture? capture;

        protected override void OnLoad() {
            base.OnLoad();
            GL.Enable(EnableCap.DepthTest);

            MainShader = Shader.CreateShader("Planet");
            FrameCaptureShader = Shader.CreateShader("Capture");
            sphere = new DebugSphere();
            quad = new DebugQuad();

            capture = FrameCapture.Create();
            Camera.Initialize();
        }

        protected override void OnUpdateFrame(FrameEventArgs args) {
            base.OnUpdateFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(0f, 0f, 0f, 1f);

            Camera.Move(Vector2.Zero);

            MainShader!.BindShaderProgram();
            MainShader!.SetMatrix4("projection", Camera.projection);
            MainShader!.SetMatrix4("lookAt", Camera.lookAt);

            //capture!.Bind();
            //GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            //GL.ClearColor(0f, 0f, 0f, 0f);
            sphere!.Update(MainShader!);
            //FrameCapture.Unbind();

            GL.BindTexture(TextureTarget.Texture2D, capture!.frameCaptureRenderTexture);
            //GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            //GL.ClearColor(0f, 0f, 0f, 1f);
            //quad!.Update(FrameCaptureShader!);
            GL.BindTexture(TextureTarget.Texture2D, 0);

            SwapBuffers();
        }


        bool isRightButtonDown;
        Vector2 currentMousePosition = Vector2.Zero, 
                lastMousePosition = Vector2.Zero;

        protected override void OnMouseMove(MouseMoveEventArgs e) {
            base.OnMouseMove(e);

            currentMousePosition = e.Position;
            if (isRightButtonDown) {
                float xDelta, yDelta;
                xDelta = lastMousePosition.X - currentMousePosition.X;
                yDelta = lastMousePosition.Y - currentMousePosition.Y;

                Camera.Update(new Vector2(xDelta, yDelta));
            }

            lastMousePosition = currentMousePosition;
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            windowSize = e.Size;
            base.OnResize(e);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e) {
            base.OnMouseDown(e);

            if (e.Button == MouseButton.Right) isRightButtonDown = true;
        }

        protected override void OnMouseUp(MouseButtonEventArgs e) {
            base.OnMouseDown(e);

            if (e.Button == MouseButton.Right) isRightButtonDown = false;
        }
    }
}