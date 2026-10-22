using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Drawing.Drawing2D;

namespace OpenTK_tutorial.Shaders
{
    internal class IntersectionShader : ShaderProgram
    {
        public IntersectionShader(string vertexPath, string fragmentPath) : base(vertexPath, fragmentPath)
        {
        }

        internal virtual void SetUniforms(Matrix4 view, Matrix4 projection, Vector3 color)
        {
            float time = t.ElapsedMilliseconds;

            int location = GL.GetUniformLocation(Handle, "baseColor");
            GL.Uniform3(location, color);

            base.SetUniforms(view, projection);
        }

        internal virtual void SetUniforms(Matrix4 view, Matrix4 projection, Vector2 resolution, Matrix4 rotation)
        {
            // Set raytracing uniforms for GLSL fragment calculation
            int location = GL.GetUniformLocation(Handle, "resolution");
            GL.Uniform2(location, resolution);
            
            location = GL.GetUniformLocation(Handle, "rot");
            GL.UniformMatrix4(location, false, ref rotation);

            base.SetUniforms(view, projection);
        }
    }
}
