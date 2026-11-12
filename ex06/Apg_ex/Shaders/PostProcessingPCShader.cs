using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace OpenTK_tutorial.Shaders
{
    internal class PostProcessingPCShader : ShaderProgram
    {
        public PostProcessingPCShader(string vertexPath, string fragmentPath) : base(vertexPath, fragmentPath)
        {
        }

        /// <summary>
        /// Set uniforms for occlusion filter shader
        /// </summary>
        /// <param name="colorTex"> Color texture </param>
        /// <param name="depthTex"> Depth texture </param>
        /// <param name="bgColor"> Background color (in our case, black) </param>
        /// <param name="minD"> Minimum distance to detect a discontinuity (how sensitive we are to changes in neighbors) </param>
        /// <param name="texelSize"> Size of texel </param>
        internal void SetOcclusionUniforms(int colorTex, int depthTex, Vector3 bgColor, float minD, Vector2 texelSize)
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, colorTex);
            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, depthTex);

            int loc0 = GL.GetUniformLocation(Handle, "uColorTex");
            GL.Uniform1(loc0, 0);

            int loc1 = GL.GetUniformLocation(Handle, "uDepthTex");
            GL.Uniform1(loc1, 1);

            int loc2 = GL.GetUniformLocation(Handle, "uBgColor");
            Vector4 col = new Vector4(bgColor, 1.0f);
            GL.Uniform4(loc2, ref col);

            int loc3 = GL.GetUniformLocation(Handle, "uMinD");
            GL.Uniform1(loc3, minD);

            int loc4 = GL.GetUniformLocation(Handle, "uTexelSize");
            GL.Uniform2(loc4, ref texelSize);
        }

        /// <summary>
        /// Set uniforms for hole filling shader
        /// </summary>
        /// <param name="colorTex"> Color texture </param>
        /// <param name="depthTex"> Depth texture </param>
        /// <param name="bgColor"> Background color (in our case, black) </param>
        /// <param name="texelSize"> Size of texel </param>
        internal void SetHolefillUniforms(int colorTex, int depthTex, Vector3 bgColor, Vector2 texelSize)
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, colorTex);
            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, depthTex);

            int loc0 = GL.GetUniformLocation(Handle, "uColorTex");
            GL.Uniform1(loc0, 0);

            int loc1 = GL.GetUniformLocation(Handle, "uDepthTex");
            GL.Uniform1(loc1, 1);

            int loc2 = GL.GetUniformLocation(Handle, "uBgColor");
            Vector4 col = new Vector4(bgColor, 1.0f);
            GL.Uniform4(loc2, ref col);

            int loc4 = GL.GetUniformLocation(Handle, "uTexelSize");
            GL.Uniform2(loc4, ref texelSize);
        }
    }
}
