using Apg_ex.Excercises.Ex03;
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
        // camera rotation
        private float angle;
        private Matrix4 rot = Matrix4.Identity;
        // camera position
        Vector3 camPos;
        // camera up
        Vector3 up;


        // ---------------------------
        //          Excercises
        Excercise01 ex01;
        Excercise02 ex02;
        Excercise03 ex03;
        // ---------------------------

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

            // create excercieses
            ex01 = new Excercise01(ClientSize.X, ClientSize.Y);
            ex02 = new Excercise02(ClientSize.X, ClientSize.Y);
            ex03 = new Excercise03(ClientSize.X, ClientSize.Y);

            // perspective projection
            camPos = new Vector3(0.01f, 45.0f, 0.0f); 
            up = Vector3.UnitY;

            view = Matrix4.LookAt(camPos, Vector3.Zero, up);
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(55f), (float)ClientSize.X / ClientSize.Y, 1.0f, 100.0f);
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
                ex02.Escape();
                ex03.Escape();
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
            {
                ex02.MoveCam();
                RotateCamera((float)args.Time);
            }

            ex03.RotateFakeCamera((float)args.Time); // TODO: uncomment

        }

        void RotateCamera(float deltaTime)
        {
            angle += deltaTime * 0.5f;
            angle %= (float)(2 * Math.PI);
            rot = Matrix4.CreateRotationZ(angle); // rotate around z

            Vector3 camPosRotated = (rot * new Vector4(camPos, 1)).Xyz;
            Vector3 upRotated = (rot * new Vector4(up, 1)).Xyz;

            view = Matrix4.LookAt(camPosRotated, Vector3.Zero, upRotated);
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(55f), (float)ClientSize.X / ClientSize.Y, 1.0f, 100.0f);
        }

        /// <summary>
        /// Render one frame
        /// </summary>
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
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
            // ex02.Draw(view, projection);
            ex03.Draw(view, projection);
        }

    }
}