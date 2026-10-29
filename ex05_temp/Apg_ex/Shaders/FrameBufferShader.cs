using OpenTK.Graphics.OpenGL4;

namespace OpenTK_tutorial.Shaders
{
    internal class FrameBufferShader : ShaderProgram
    {
        public FrameBufferShader(string vertexPath, string fragmentPath) : base(vertexPath, fragmentPath)
        {
        }

        internal void SetTextures(int tex0, int tex1)
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, tex0);
            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, tex1);

            int loc0 = GL.GetUniformLocation(Handle, "texture0");
            GL.Uniform1(loc0, 0);
            int loc1 = GL.GetUniformLocation(Handle, "texture1");
            GL.Uniform1(loc1, 1);

        }
    }
}
