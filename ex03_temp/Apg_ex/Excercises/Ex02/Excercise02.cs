using KPGBase.Excercises;
using OpenTK.Mathematics;
using OpenTK_tutorial.Objects;
using OpenTK_tutorial.Shaders;

namespace OpenTK_tutorial.Excercises.Ex02
{
    /// <summary>
    /// GPU Raytraced Sphere & Torus rendered onto a canvas quad
    /// </summary>
    internal class Excercise02 : BasicExcercise
    {
        // Textured rectangle covering the entire canvas
        private Square canvasSquare;

        // Camera rotation matrix and state
        private float angle;
        private Matrix4 rot = Matrix4.Identity;

        public Excercise02(int width, int height) : base(width, height)
        {
            // Quad positioned at center and scaled to match canvas dimensions
            canvasSquare = new Square(new Vector3(-width / 2f, -height / 2f, 0), new Vector3(0, 0, 0), new Vector3(width, height, 1));
            canvasSquare.Initialize();

            // Load GPU Raytracing Shader
            shader = new IntersectionShader("Shaders/intersection_shader.vert", "Shaders/intersection_shader.frag");
        }

        /// <summary>
        /// Rotate camera around X-axis
        /// </summary>
        internal void MoveCam()
        {
            angle += 0.1f;
            angle %= 360f;
            rot = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(angle));
        }

        /// <summary>
        /// Render full-screen quad using the GPU raytracing shader
        /// </summary>
        /// <param name="view">View matrix</param>
        /// <param name="projection">Projection matrix</param>
        internal void Draw(Matrix4 view, Matrix4 projection)
        {
            ClearImage();

            shader.Use();

            // Set matrix uniforms for quad rendering
            ((IntersectionShader)shader).SetUniforms(view, projection, new Vector2(width, height), rot);

            // Draw full-canvas rectangle
            canvasSquare.Draw(shader);
        }
    }
}