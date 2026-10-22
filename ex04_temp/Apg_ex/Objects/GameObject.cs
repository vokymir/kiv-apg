using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK_tutorial.Shaders;

namespace OpenTK_tutorial.Objects
{
    public class GameObject
    {
        // object data
        public float[] vertices;
        public uint[] indices;

        // buffer objects
        internal int VertexBufferObject;
        internal int VertexArrayObject;
        internal int ElementBufferObject;

        // model matrix
        public Matrix4 model = Matrix4.Identity;

        // translation, rotation and scale of the object
        private Vector3 translation;
        private Vector3 rotation;
        private Vector3 scale = Vector3.One;
        public Vector3 Translation { get => translation; set { translation = value; CreateModelMatrix(); } }
        public Vector3 Rotation { get => rotation; set { rotation = value; CreateModelMatrix(); } }
        public Vector3 Scale { get => scale; set { scale = value; CreateModelMatrix(); } }

        /// <summary>
        /// Initialize the game object
        /// - send data to GPU
        /// </summary>
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

        /// <summary>
        /// Draw the game object using shader
        /// </summary>
        /// <param name="shader"></param>
        public virtual void Draw(ShaderProgram shader)
        {
            shader.SetModel(model);
            GL.BindVertexArray(VertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
        }

        /// <summary>
        /// Create model matrix
        /// </summary>
        internal void CreateModelMatrix()
        {
            model = Matrix4.CreateScale(Scale);
            model = model * Matrix4.CreateRotationX((float)(Rotation.X * Math.PI / 180));
            model = model * Matrix4.CreateRotationY((float)(Rotation.Y * Math.PI / 180));
            model = model * Matrix4.CreateRotationZ((float)(Rotation.Z * Math.PI / 180));
            model = model * Matrix4.CreateTranslation(Translation);
        }
    }
}
