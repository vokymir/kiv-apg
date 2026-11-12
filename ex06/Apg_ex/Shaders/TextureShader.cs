using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace OpenTK_tutorial.Shaders
{
    internal class TextureShader : ShaderProgram
    {
        public TextureShader(string vertexPath, string fragmentPath) : base(vertexPath, fragmentPath)
        {
        }

        internal virtual void SetUniforms(Matrix4 view, Matrix4 projection)
        {
            int location = GL.GetUniformLocation(Handle, "texture0");
            GL.Uniform1(location, 0);

            base.SetUniforms(view, projection);
        }
    }
}
