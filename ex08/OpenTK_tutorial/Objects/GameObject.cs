using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace OpenTK_tutorial
{
    internal class GameObject
    {
        public float[] vertices;
        public uint[] indices;

        public Matrix4 model = Matrix4.Identity;

        public Vector3 translation;
        public Vector3 rotation;
        public Vector3 scale;

        internal int VertexBufferObject;
        internal int VertexArrayObject;
        internal int ElementBufferObject;

        public virtual void Initialize()
        {
            VertexArrayObject = GL.GenVertexArray();
            VertexBufferObject = GL.GenBuffer();
            ElementBufferObject = GL.GenBuffer();

            GL.BindVertexArray(VertexArrayObject);
            
            // data
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            // verts
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // normals
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(2);

            // indices
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            // unbind
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public virtual void Draw(ShaderProgram shader)
        {
            shader.SetModel(model);
            GL.BindVertexArray(VertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
        }

        public void SetRotation(Vector3 r)
        {
            this.rotation = r;
            CreateModelMatrix();
        }


        internal void CreateModelMatrix()
        {
            model = Matrix4.CreateScale(scale);
            model = model * Matrix4.CreateTranslation(translation);
            model = model * Matrix4.CreateRotationX((float)(rotation.X * Math.PI / 180));
            model = model * Matrix4.CreateRotationY((float)(rotation.Y * Math.PI / 180));
            model = model * Matrix4.CreateRotationZ((float)(rotation.Z * Math.PI / 180));
        }
    }
}
