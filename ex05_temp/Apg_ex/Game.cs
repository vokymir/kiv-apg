using Apg_ex.Excercises.Ex03;
using Apg_ex.Excercises.Ex05;
using OpenTK.Graphics.OpenGL4;
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
        Exercise01 ex01;
        Exercise02 ex02;
        Exercise03 ex03;
        Exercise04 ex04;
        Exercise05 ex05;
        // ---------------------------

        public Game(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = (width, height), Title = title })
        {
            Console.WriteLine(GL.GetError());
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(0.22f, 0.22f, 0.22f, 1.0f);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            // create excercieses
            // ex01 = new Exercise01(ClientSize.X, ClientSize.Y);
            // ex02 = new Exercise02(ClientSize.X, ClientSize.Y);
            // ex03 = new Exercise03(ClientSize.X, ClientSize.Y);
            // ex04 = new Exercise04(ClientSize.X, ClientSize.Y);
            ex05 = new Exercise05(ClientSize.X, ClientSize.Y);

            // perspective projection
            camPos = new Vector3(0f, 0f, 45.0f); 
            up = Vector3.UnitY;

            ex05.camPos = camPos;

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
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(55f), (float)ClientSize.X / ClientSize.Y, 1.0f, 100.0f);
            // changes that happen in excercises based on client size go here >>
            ex05.ChangeSize(ClientSize.X, ClientSize.Y);
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
                ex05.Escape();
                Close();
            }

            if (KeyboardState.IsKeyDown(Keys.R))
            {
                RotateCamera((float)args.Time);
            }
        }

        void RotateCamera(float deltaTime)
        {
            angle += deltaTime * 0.5f;
            angle %= (float)(2 * Math.PI);
            rot = Matrix4.CreateRotationY(-angle); 

            Vector3 camPosRotated = (rot * new Vector4(camPos, 1)).Xyz;
            Vector3 upRotated = (rot * new Vector4(up, 1)).Xyz;

            ex05.camPos = camPosRotated;

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
            // view direction outlines
            ex05.Draw(view, projection);

            // frame buffer outlines
            // ex05.RenderFirstPass(view, projection);
            // ex05.RenderSecondPass(view, projection);
            // ex05.RenderThirdPass();
        }

    }
}