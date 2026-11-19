using OpenTK.Graphics.OpenGL4;

namespace OpenTK_tutorial
{
    internal class Texture3D
    {
        public int ID;

        private int width;
        private int height;
        private int depth;

        public Texture3D(string path, int width, int height, int depth)
        {
            this.width = width;
            this.height = height;
            this.depth = depth;

            Load3DTexture(path);
        }

        private void Load3DTexture(string path)
        {
            ID = GL.GenTexture();
            GL.ActiveTexture(TextureUnit.Texture2);
            GL.BindTexture(TextureTarget.Texture3D, ID);

            // data contains only luminance for each pixel
            byte[] head = File.ReadAllBytes(path);
            // convert to rgba
            byte[] head2 = new byte[head.Length * 4];
            for (int i = 0; i < head.Length; i++)
            {
                head2[4 * i + 0] = head[i];
                head2[4 * i + 1] = head[i];
                head2[4 * i + 2] = head[i];
                head2[4 * i + 3] = head[i];
            }

            // set up texture data
            GL.TexImage3D(TextureTarget.Texture3D, 0,
                          PixelInternalFormat.Rgba,
                          width, height, depth, 0,
                          PixelFormat.Rgba, PixelType.UnsignedByte, head2);

            GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

            GL.BindTexture(TextureTarget.Texture3D, 0);
        }
    }
}
