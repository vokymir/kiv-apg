using KPGBase.Excercises;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK_tutorial.Excercises.Ex01;
using OpenTK_tutorial.Excercises.Ex02;

namespace OpenTK_tutorial
{
    /// <summary>
    /// Main game window
    /// </summary>
    public class Game : GameWindow
    {
        // projection matrix
        Matrix4 projection;
        // view matrix
        Matrix4 view;

        // ---------------------------
        //          Excercises
        Excercise01 ex01;
        Excercise02 ex02;
        // ---------------------------

        public Game(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = (width, height), Title = title })
        {
            ex02 = new Excercise02(width, height);
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(0.22f, 0.22f, 0.22f, 1.0f);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            // create excercieses
            ex01 = new Excercise01(ClientSize.X, ClientSize.Y);
            ex02 = new Excercise02(ClientSize.X, ClientSize.Y);

            // orthographic projection, camera is static
            projection = Matrix4.CreateOrthographic((float)ClientSize.X, (float)ClientSize.Y, -0.1f, 100.0f);
            view = Matrix4.CreateTranslation(0.0f, 0.0f, 0);
        }

        /// <summary>
        /// When window is resized
        /// </summary>
        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);

            // client size changed! view/projection matrices need to reflect that
            projection = Matrix4.CreateOrthographic((float)ClientSize.X, (float)ClientSize.Y, -0.1f, 100.0f);
            // changes that happen in excercises based on client size go here >>
            ex01.ChangeSize(ClientSize.X, ClientSize.Y);
        }

        /// <summary>
        /// When frame is updated
        /// - check for input
        /// </summary>
        /// <param name="args"></param>
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            // esape
            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                ex01.Escape();
                Close();
            }

            if (KeyboardState.IsKeyDown(Keys.A))
                ((Excercise01)ex01).MovePlayerSides(true, args.Time);
            if (KeyboardState.IsKeyDown(Keys.D))
                ((Excercise01)ex01).MovePlayerSides(false, args.Time);

            if (KeyboardState.IsKeyDown(Keys.W))
                ((Excercise01)ex01).MovePlayerBack(true, args.Time);
            if (KeyboardState.IsKeyDown(Keys.S))
                ((Excercise01)ex01).MovePlayerBack(false, args.Time);


            if (KeyboardState.IsKeyDown(Keys.R))
                ex02.MoveCam();
        }

        /// <summary>
        /// Render one frame
        /// </summary>
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            //Code goes here.
            RenderScene();

            SwapBuffers();
        }

        /// <summary>
        /// Render the scene
        /// </summary>
        private void RenderScene()
        {
            // ex01.DrawingSandbox(view, projection);
            // ex01.DrawScenery(view, projection);
            ex02.Draw(view, projection);
        }

    }
}