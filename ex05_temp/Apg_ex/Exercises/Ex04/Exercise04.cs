using Apg_ex.Objects;
using KPGBase.Exercises;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK_tutorial.Objects;
using OpenTK_tutorial.Shaders;

namespace Apg_ex.Excercises.Ex03
{
    internal class Exercise04 : BasicExercise
    {
        private MeshObject dillo;
        private Cube light;
        public Vector3 camPos;

        ShaderProgram flatShader;

        public Exercise04(int width, int height) : base(width, height)
        {
            shader = new LightShader("Shaders/simple_shader.vert", "Shaders/simple_shader.frag");
            flatShader = new FlatShader("Shaders/flat_shader.vert", "Shaders/flat_shader.frag");
            
            dillo = new MeshObject(new Vector3(30), new Vector3(-90, 0, 0), "Models/model.x", "Models/modelcolormap.png");
            dillo.Initialize();
            light = new Cube(new Vector3(10, 0, 0), new Vector3(), new Vector3(2));
            light.Initialize();

            Console.WriteLine(GL.GetError());
        }

        /// <summary>
        /// Main render call
        /// </summary>
        internal void Draw(Matrix4 view, Matrix4 projection)
        {
            ClearImage();
            
            shader.Use();
            ((LightShader)shader).SetUniforms(view, projection, new Vector3(1, 1, 1), light.Translation, new Vector3(camPos));
            dillo.Draw(shader);

            flatShader.Use();
            ((FlatShader)flatShader).SetUniforms(view, projection, new Vector3(1, 1, 0));
            light.Draw(flatShader);
        }
    }
}
