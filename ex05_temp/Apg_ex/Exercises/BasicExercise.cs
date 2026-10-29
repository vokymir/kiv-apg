
using OpenTK.Graphics.OpenGL4;
using OpenTK_tutorial.Shaders;

namespace KPGBase.Exercises
{
    internal class BasicExercise
    {
        // Shader used in the excercise
        internal ShaderProgram shader;

        // width and height of window
        internal int width;
        internal int height;

        /// <summary>
        /// Constructor
        /// </summary>
        public BasicExercise(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Paint whole image grey
        /// </summary>
        internal void ClearImage()
        {
            GL.ClearColor(0.22f, 0.22f, 0.22f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        /// <summary>
        /// Paint whole image grey
        /// </summary>
        internal void ClearImage2()
        {
            GL.ClearColor(0f, 0f, 0f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        /// <summary>
        /// Quit
        /// </summary>
        internal void Escape()
        {
            shader.Dispose();
        }

        /// <summary>
        /// Change window size
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public virtual void ChangeSize(int x, int y)
        {
            this.width = x;
            this.height = y;
        }
    }
}
