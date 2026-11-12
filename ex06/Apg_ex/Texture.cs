using OpenTK.Graphics.OpenGL4;
using System.Drawing;

namespace Apg_ex
{
    internal class Texture
    {
        public int Handle;
        Bitmap b;

        public Texture(Bitmap b)
        {
            this.b = b;
            Handle = GL.GenTexture();
        }

        /// <summary>
        /// Set up texture
        /// </summary>
        /// <param name="unit"></param>
        public void Initialize(TextureUnit unit)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, Handle);

            System.Drawing.Imaging.BitmapData bpData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, b.Width, b.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, bpData.Scan0);
            b.UnlockBits(bpData);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        }
    }
}
