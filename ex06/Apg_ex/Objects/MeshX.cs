using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

namespace Apg_ex.Objects
{
    class MeshX
    {
        XFile xFile;

        int vao;
        int vb, ib;
        int indexcount;
        int texture, texture2;

        string meshPath;

        // z tohodle textury
        string colorMap;
        string lightMap;

        public MeshX(string meshPath, string colorMap)
        {
            this.colorMap = colorMap;
            this.meshPath = meshPath;

            XFile = new XFile();
            XFile.Load(meshPath);
        }

        internal XFile XFile { get => xFile; set => xFile = value; }

        public void Init()
        {
            indexcount = XFile.Index.Length;

            vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);

            vb = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vb);
            GL.BufferData(BufferTarget.ArrayBuffer, XFile.Vertex.Length * 8 * sizeof(float), 0, BufferUsageHint.StaticDraw);

            // 0 - vertices
            // 2 - normals
            // 3 - color
            // 8 - uv0

            // vertices
            GL.BufferSubData(BufferTarget.ArrayBuffer, 0, XFile.Vertex.Length * 3 * sizeof(float), XFile.Vertex);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            
            // normals
            GL.BufferSubData(BufferTarget.ArrayBuffer, (XFile.Vertex.Length * 3 * sizeof(float)), XFile.Vertex.Length * 3 * sizeof(float), XFile.Normal);
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 0, (XFile.Vertex.Length * 3 * sizeof(float)));
            
            // uvs
            GL.BufferSubData(BufferTarget.ArrayBuffer, (XFile.Vertex.Length * 6 * sizeof(float)), XFile.Vertex.Length * 2 * sizeof(float), XFile.TexCoord);
            GL.EnableVertexAttribArray(8);
            GL.VertexAttribPointer(8, 2, VertexAttribPointerType.Float, false, 0, (XFile.Vertex.Length * 6 * sizeof(float)));

            ib = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ib);
            GL.BufferData(BufferTarget.ElementArrayBuffer, XFile.Index.Length * 3 * sizeof(uint), XFile.Index, BufferUsageHint.StaticDraw);

            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            GL.DeleteBuffer(vb);
            GL.DeleteBuffer(ib);

            // load texture to texture0
            texture = GL.GenTexture();
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texture);

            //StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult image = ImageResult.FromStream(File.OpenRead(colorMap), ColorComponents.RedGreenBlueAlpha);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);

            // texture2 = GL.GenTexture();
            // GL.ActiveTexture(TextureUnit.Texture1);
            // GL.BindTexture(TextureTarget.Texture2D, texture2);

            //StbImage.stbi_set_flip_vertically_on_load(1);
            // image = ImageResult.FromStream(File.OpenRead("Models/hatch.jpg"), ColorComponents.RedGreenBlueAlpha);
            // GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);

            GL.BindTexture(TextureTarget.Texture2D, 0);
        }



        public void Render()
        {
            GL.BindVertexArray(vao);

            // color texture
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, texture2);

            GL.DrawElements(PrimitiveType.Triangles, indexcount * 3, DrawElementsType.UnsignedInt, 0);
            
            GL.BindVertexArray(0);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }
    }
}
