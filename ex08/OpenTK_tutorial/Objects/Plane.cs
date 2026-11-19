using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTK_tutorial
{
    internal class Plane : GameObject
    {
        public float[] uvs;

        public Plane()
        {
            vertices = [
                1f,  1f, 0.0f, 0, 0, 1,  // top right
                1f, -1f, 0.0f, 0, 0, 1,  // bottom right
                -1f, -1f, 0.0f, 0, 0, 1,  // bottom left
                -1f,  1f, 0.0f, 0, 0, 1   // top left
            ];

            indices = [ // note that we start from 0!
                0, 1, 3,   // first triangle
                1, 2, 3    // second triangle
            ];

            uvs = [
                1f,  1f,
                1f, 0f,
                0f, 0f,
                0f,  1f
            ];
        }

        public override void Initialize()
        {
            VertexArrayObject = GL.GenVertexArray();
            VertexBufferObject = GL.GenBuffer();
            ElementBufferObject = GL.GenBuffer();

            GL.BindVertexArray(VertexArrayObject);

            // data
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, (vertices.Length * sizeof(float)) + uvs.Length * sizeof(float), 0, BufferUsageHint.StaticDraw);
            GL.BufferSubData(BufferTarget.ArrayBuffer, 0, vertices.Length * sizeof(float), vertices);
            GL.BufferSubData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), uvs.Length * sizeof(float), uvs);

            // verts
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // normals
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(2);

            // uvs
            GL.VertexAttribPointer(8, 2, VertexAttribPointerType.Float, false, 0, (vertices.Length * sizeof(float)));
            GL.EnableVertexAttribArray(8);

            // indices
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            // unbind
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public override void Draw(ShaderProgram shader)
        {
            int location = GL.GetUniformLocation(shader.Handle, "texture0");
            GL.Uniform1(location, 0);
            location = GL.GetUniformLocation(shader.Handle, "texture1");
            GL.Uniform1(location, 1);

            base.Draw(shader);
        }
    }

}
