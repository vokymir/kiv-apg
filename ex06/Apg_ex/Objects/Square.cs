using Apg_ex;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK_tutorial.Shaders;
using System.Drawing;

namespace OpenTK_tutorial.Objects
{
    internal class Square : GameObject
    {
        // texture
        public Texture Texture { get; private set; }

        public Square(Vector3 translation, Vector3 rotation, Vector3 scale)
        {
            vertices = [
                0f, 0f, 0.0f, 0, 1f, //left bottom vertex
                1, 0f, 0.0f, 1f, 1f, //right vertex
                
                0f, 1, 0.0f, 0, 0, //left vertex
                1, 1, 0.0f, 1f, 0f, //right vertex
            ];

            indices = [
                0, 1, 2, 2, 1, 3
            ];

            this.Scale = scale;
            this.Translation = translation;
            this.Rotation = rotation;
            CreateModelMatrix();
        }

        public override void Initialize()
        {
            VertexArrayObject = GL.GenVertexArray();
            VertexBufferObject = GL.GenBuffer();
            ElementBufferObject = GL.GenBuffer();

            GL.BindVertexArray(VertexArrayObject);

            // data
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            // verts
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // uvs
            GL.VertexAttribPointer(8, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(8);

            // indices
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            // unbind
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        /// <summary>
        /// Loads image file and initializes texture for this game object
        /// </summary>
        public void SetTexture(string pathToTexture)
        {
            Bitmap bmp = new Bitmap(pathToTexture);
            Texture = new Texture(bmp);
            Texture.Initialize(TextureUnit.Texture0);
        }

        public override void Draw(ShaderProgram shader)
        {
            if (Texture != null)
            {
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, Texture.Handle);
            }

            base.Draw(shader);
        }
    }
}
