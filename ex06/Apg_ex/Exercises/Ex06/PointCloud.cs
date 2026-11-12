using Apg_ex.Objects;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK_tutorial.Objects;
using OpenTK_tutorial.Shaders;
using StbImageSharp;

namespace Apg_ex.Exercises.Ex06
{
    internal class PointCloud : GameObject 
    {
        public bool hasTex = false;
        int texture = 0;
        string colorMap;

        float[] normals;
        float[] uvs;
        float[] colors;

        /// <summary>
        /// Create point cloud from file
        /// </summary>
        /// <param name="scale"> Scale </param>
        /// <param name="rotation"> Rotation </param>
        /// <param name="path"> Path to file </param>
        /// <param name="colorPath"> Path to color </param>
        public PointCloud(Vector3 scale, Vector3 rotation, Vector3 translation, string path, string colorPath = "")
        {
            if (colorPath != "")
            {
                this.colorMap = colorPath;
                hasTex = true;
            }

            if (path.EndsWith(".x")) {
                XFile x = new XFile();
                x.Load(path);

                vertices = new float[x.Vertex.Length * 3];
                normals = new float[x.Vertex.Length * 3];
                colors = new float[x.Vertex.Length * 3];
                uvs = new float[x.Vertex.Length * 2];

                for (int i = 0; i < x.Vertex.Length; i++)
                {
                    vertices[3 * i + 0] = x.Vertex[i].x;
                    vertices[3 * i + 1] = x.Vertex[i].y;
                    vertices[3 * i + 2] = x.Vertex[i].z;

                    normals[3 * i + 0] = x.Normal[i].x;
                    normals[3 * i + 1] = x.Normal[i].y;
                    normals[3 * i + 2] = x.Normal[i].z;

                    uvs[2 * i + 0] = x.TexCoord[i].x;
                    uvs[2 * i + 1] = x.TexCoord[i].y;
                }
            }
            else if (path.EndsWith(".obj"))
            {
                ObjFile obj = new ObjFile();
                obj.LoadObj(path);

                vertices = obj.Vertex;
                normals = obj.Normal;
                colors = obj.Color;

                int vertCount = vertices.Length / 3;
                uvs = new float[vertCount * 2];
            }
            else
            {
                CSVFile csv = new CSVFile();
                csv.LoadFile(path);

                vertices = csv.Vertex;
                normals = csv.Normal;
                colors = csv.Color;

                int vertCount = vertices.Length / 3;
                uvs = new float[vertCount * 2];
            }

            this.Scale = scale;
            this.Translation = translation;
            this.Rotation = rotation;
        }

        public override void Initialize()
        {
            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            int vertexCount = vertices.Length / 3;

            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, 3 * vertexCount * 3 * sizeof(float) + vertexCount * 2 * sizeof(float), 0, BufferUsageHint.StaticDraw);

            // 0 - vertices
            // 2 - normals
            // 3 - color
            // 8 - uv0

            // vertices
            GL.BufferSubData(BufferTarget.ArrayBuffer, 0, vertices.Length * sizeof(float), vertices);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

            // normals
            GL.BufferSubData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertexCount * 3 * sizeof(float), normals);
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 0, (vertices.Length * sizeof(float)));

            // color
            GL.BufferSubData(BufferTarget.ArrayBuffer, 2 * vertices.Length * sizeof(float), vertexCount * 3 * sizeof(float), colors);
            GL.EnableVertexAttribArray(3);
            GL.VertexAttribPointer(3, 3, VertexAttribPointerType.Float, false, 0, (2 * vertices.Length * sizeof(float)));

            // uvs
            GL.BufferSubData(BufferTarget.ArrayBuffer, 3 * vertices.Length * sizeof(float), vertexCount * 2 * sizeof(float), uvs);
            GL.EnableVertexAttribArray(8);
            GL.VertexAttribPointer(8, 2, VertexAttribPointerType.Float, false, 0, (3 * vertices.Length * sizeof(float)));

            // indices
            indices = new uint[vertices.Length / 3];
            for (int i = 0; i < indices.Length; i++)
                indices[i] = (uint)i;

            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            GL.DeleteBuffer(VertexBufferObject);
            GL.DeleteBuffer(ElementBufferObject);

            // load texture to texture0
            texture = GL.GenTexture();
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texture);

            if (hasTex)
            {
                //StbImage.stbi_set_flip_vertically_on_load(1);
                ImageResult image = ImageResult.FromStream(File.OpenRead(colorMap), ColorComponents.RedGreenBlueAlpha);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
            }

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);

            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public override void Draw(ShaderProgram shader)
        {
            shader.SetModel(model);
            int location = GL.GetUniformLocation(shader.Handle, "texture0");
            GL.Uniform1(location, 0);
            location = GL.GetUniformLocation(shader.Handle, "isTex");
            GL.Uniform1(location, 0f);
            if (hasTex)
                GL.Uniform1(location, 1f);

            GL.BindVertexArray(VertexArrayObject);

            // color texture
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texture);

            GL.PointSize(5);
            GL.DrawElements(PrimitiveType.Points, indices.Length, DrawElementsType.UnsignedInt, 0);

            GL.BindVertexArray(0);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, 0);

        }
    }
}
