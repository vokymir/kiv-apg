using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace OpenTK_tutorial.Shaders
{
    internal class FlatShader : ShaderProgram
    {
        public FlatShader(string vertexPath, string fragmentPath) : base(vertexPath, fragmentPath)
        {
        }

        internal virtual void SetUniforms(Matrix4 view, Matrix4 projection, Vector3 color)
        {
            float time = t.ElapsedMilliseconds;

            int location = GL.GetUniformLocation(Handle, "baseColor");
            GL.Uniform3(location, color);

            base.SetUniforms(view, projection);
        }
    }
}
