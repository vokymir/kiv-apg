using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace OpenTK_tutorial
{
    internal class VolumeBox : GameObject
    {
        float[] color;

        public VolumeBox(Vector3 translation, Vector3 rotation, float scale)
        {
            vertices = [
                -1f, -1f, 1f, 0, 0, 1,
                1f, -1f, 1f, 0, 0, 1,
                -1f, 1f, 1f, 0, 0, 1,
                1f, 1f, 1f, 0, 0, 1,

                -1f, -1f, -1f, 0, 0, -1,
                1f, -1f, -1f, 0, 0, -1,
                -1f, 1f, -1f, 0, 0, -1,
                1f, 1f, -1f, 0, 0, -1
            ];

            color = [
                1f, 1f, 0f,
                0f, 1f, 0f,
                1f, 0f, 0f,
                0f, 0f, 0f,

                1f, 1f, 1f,
                0f, 1f, 1f,
                1f, 0f, 1f,
                0f, 0f, 1f
            ];

            indices = [
                0, 1, 2,
                1, 3, 2,

                4, 6, 5,
                5, 6, 7,

                1, 5, 3,
                5, 7, 3,

                0, 2, 4,
                4, 2, 6,

                2, 3, 6,
                3, 7, 6,

                0, 4, 1,
                1, 4, 5
            ];

            this.translation = translation;
            this.rotation = rotation;
            this.scale = new Vector3(scale, scale, scale / 2.0f);
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
            GL.BufferData(BufferTarget.ArrayBuffer, (vertices.Length * sizeof(float)) + color.Length * sizeof(float), 0, BufferUsageHint.StaticDraw);
            GL.BufferSubData(BufferTarget.ArrayBuffer, 0, vertices.Length * sizeof(float), vertices);
            GL.BufferSubData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), color.Length * sizeof(float), color);

            // verts
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // normals
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(2);

            // color
            GL.VertexAttribPointer(3, 3, VertexAttribPointerType.Float, false, 0, (vertices.Length * sizeof(float)));
            GL.EnableVertexAttribArray(3);

            // indices
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            // unbind
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

    }
}
