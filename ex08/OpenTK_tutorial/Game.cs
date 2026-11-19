using Excercises;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Drawing;

namespace OpenTK_tutorial
{
    public class Game : GameWindow
    {
        float azimuth, zenith;

        Matrix4 projection;
        Matrix4 view;

        Exercise08 ex08;

        Vector3 camPos;
        Vector3 up;

        public Game(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = (width, height), Title = title })
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.22f, 0.22f, 0.22f, 1.0f);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            ex08 = new Exercise08(ClientSize.X, ClientSize.Y);

            // perspective projection
            view = Matrix4.CreateTranslation(camPos);
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(55.0f), (float)ClientSize.X / (float)ClientSize.Y, 0.1f, 100.0f);
            camPos = new Vector3(0f, 0f, -2);
            up = Vector3.UnitY;

            ex08.camPos = camPos;
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);

            // client size changed! view/projection matrices need to reflect that
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(55f), (float)ClientSize.X / ClientSize.Y, 1.0f, 100.0f);
            // changes that happen in excercises based on client size go here >>
            ex08.ChangeSize(ClientSize.X, ClientSize.Y);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                ex08.Escape();
                Close();
            }

            // rotate around y axis
            if (MouseState.IsButtonDown(MouseButton.Left))
            {
                azimuth += MouseState.X - MouseState.PreviousX;
                zenith += MouseState.Y - MouseState.PreviousY;
                
                ex08.Rotate(zenith, -azimuth);
            }

            // scroll in/out
            Vector2 scroll = MouseState.Scroll - MouseState.PreviousScroll;
            camPos = new Vector3(0, 0, camPos.Z + 0.1f * scroll.Y);
            view = Matrix4.CreateTranslation(camPos);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            //Code goes here.
            ex08.RenderFrontFaces(view, projection);
            ex08.RenderBackFaces(view, projection);
            ex08.RenderDisplayPlane(view, projection);

            SwapBuffers();
        }
    }
}