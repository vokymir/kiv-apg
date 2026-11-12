using OpenTK.Graphics.OpenGL4;

namespace Apg_ex.Excercises.Ex05
{
    /** Framebuffer used for rendering shadows */
    class FrameBuffer
    {
        /** ID */
        public int ID;
        /** Widht and height */
        public int width, height;

        /** Color and depth buffer locations */
        public int colorBuffer, depthBuffer;

        FramebufferStatus framebufferstatus;

        /** Constructor */
        public FrameBuffer(int width, int height)
        {
            this.width = width;
            this.height = height;

            ID = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, ID);

            // color buffer
            colorBuffer = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, colorBuffer);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, colorBuffer, 0);

            // bind depth buffer
            depthBuffer = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, depthBuffer);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureCompareMode, (int)TextureCompareMode.None);

            Console.WriteLine(GL.GetError());

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent32f, width, height, 0, PixelFormat.DepthComponent, PixelType.Float, IntPtr.Zero);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, depthBuffer, 0);

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

            // write status
            framebufferstatus = GL.CheckNamedFramebufferStatus(ID, FramebufferTarget.Framebuffer);
            Console.WriteLine("Created frambuffer: " + framebufferstatus);

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }
    }
}
