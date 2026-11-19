using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Drawing;

namespace OpenTK_tutorial
{
    internal class VolumeShader : ShaderProgram
    {
        public VolumeShader(string vertexPath, string fragmentPath) : base(vertexPath, fragmentPath)
        {
        }

        public void SetUniforms(Matrix4 view, Matrix4 projection, int frontFace, int backFace, int volume)
        {
            base.SetUniforms(view, projection);

            GL.Enable(EnableCap.Texture2D);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, frontFace);
            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, backFace);
            GL.ActiveTexture(TextureUnit.Texture2);
            GL.BindTexture(TextureTarget.Texture3D, volume);

            float secs = (int)((t.ElapsedMilliseconds / 100) % 190);

            // set textures
            int loc0 = GL.GetUniformLocation(Handle, "texture0");
            GL.Uniform1(loc0, 0);

            int loc1 = GL.GetUniformLocation(Handle, "texture1");
            GL.Uniform1(loc1, 1);

            int loc2 = GL.GetUniformLocation(Handle, "texture2");
            GL.Uniform1(loc2, 2);

            int loc3 = GL.GetUniformLocation(Handle, "t");
            GL.Uniform1(loc3, secs);
        }
    }
}
