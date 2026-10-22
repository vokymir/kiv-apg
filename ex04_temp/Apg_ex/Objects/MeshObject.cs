using Apg_ex.Objects;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK_tutorial.Shaders;

namespace OpenTK_tutorial.Objects
{
    class MeshObject : GameObject
    {
        MeshX mesh;

        public MeshObject(Vector3 scale, Vector3 rotation, string meshPath, string texturePath)
        {
            this.Scale = scale;
            this.Rotation = rotation;

            mesh = new MeshX(meshPath, texturePath);
        }

        public override void Initialize()
        {
            mesh.Init();
        }

        public override void Draw(ShaderProgram shader)
        {
            int location = GL.GetUniformLocation(shader.Handle, "texture0");
            GL.Uniform1(location, 0);

            shader.SetModel(model);
            mesh.Render();
        }

    }
}
