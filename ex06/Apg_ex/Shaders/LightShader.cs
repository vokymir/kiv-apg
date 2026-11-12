using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace OpenTK_tutorial.Shaders
{
    internal class LightShader : ShaderProgram
    {
        public LightShader(string vertexPath, string fragmentPath) : base(vertexPath, fragmentPath)
        {
        }

        internal virtual void SetUniforms(Matrix4 view, Matrix4 projection, Vector3 lightColor, Vector3 lightPosition, Vector3 viewPosition)
        {
            int location = GL.GetUniformLocation(Handle, "lightCol");
            GL.Uniform3(location, lightColor);
            location = GL.GetUniformLocation(Handle, "lightPos");
            GL.Uniform3(location, lightPosition);
            location = GL.GetUniformLocation(Handle, "viewPos");
            GL.Uniform3(location, viewPosition);

            base.SetUniforms(view, projection);
        }
    }
}
