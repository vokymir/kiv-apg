using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK_tutorial.Shaders;

namespace Apg_ex.Shaders
{
    internal class PointcloudShader : ShaderProgram
    {
        public PointcloudShader(string vertexPath, string fragmentPath) : base(vertexPath, fragmentPath)
        {
        }

        internal virtual void SetUniforms(Matrix4 view, Matrix4 projection, Vector3 viewPosition)
        {
            int location = GL.GetUniformLocation(Handle, "viewPos");
            GL.Uniform3(location, viewPosition);

            base.SetUniforms(view, projection);
        }
    }
}
