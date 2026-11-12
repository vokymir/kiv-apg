using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK_tutorial.Objects;
using OpenTK_tutorial.Shaders;

namespace Apg_ex.Excercises.Ex03
{
    /// <summary>
    /// Wireframe object representing a camera's frustrum
    /// </summary>
    internal class FrustumWireframe : GameObject
    {

        public FrustumWireframe()
        {
            vertices = [
                -1, -1, -1, // near bottom left
                 1, -1, -1, // near bottom right
                
                 1,  1, -1, // near top-right
                -1,  1, -1, // near top-left

                -1, -1,  1, // far bottom-left
                 1, -1,  1, // far bottom-right
            
                 1,  1,  1, // far top-right
                -1,  1,  1  // far top-left
            ];

            // 12 Line segments connecting the 8 corners
            indices = [
                0, 1,  1, 2,  2, 3,  3, 0, // near face
                4, 5,  5, 6,  6, 7,  7, 4, // far face
                0, 4,  1, 5,  2, 6,  3, 7  // connecting sides
            ];

        }

        /// <summary>
        /// Initialize game object
        /// </summary>
        public override void Initialize()
        {
            VertexArrayObject = GL.GenVertexArray();
            VertexBufferObject = GL.GenBuffer();
            ElementBufferObject = GL.GenBuffer();

            GL.BindVertexArray(VertexArrayObject);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, 8 * 3 * sizeof(float), IntPtr.Zero, BufferUsageHint.DynamicDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            GL.BindVertexArray(0);
        }



        /// <summary>
        /// Modify frustum corners to world-space and render as line
        /// </summary>
        /// <param name="viewProjection"> View-projection matrix of the camera </param>
        /// <param name="shader"> Shader </param>
        public void Draw(Matrix4 viewProjection, ShaderProgram shader)
        {
            Matrix4 invVP = Matrix4.Invert(viewProjection);
            float[] worldVertices = new float[vertices.Length];

            // Transform each corner back to World Space
            for (int i = 0; i < vertices.Length; i+=3)
            {
                Vector4 worldPt = Vector4.TransformRow(new Vector4(vertices[i+0], vertices[i+1], vertices[i+2], 1.0f), invVP);
                Vector3 pt = worldPt.Xyz / worldPt.W; // Perspective divide
                worldVertices[i + 0] = pt.X;
                worldVertices[i + 1] = pt.Y;
                worldVertices[i + 2] = pt.Z;
            }

            // Upload dynamic vertex positions
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, worldVertices.Length * sizeof(float), worldVertices);

            shader.SetModel(Matrix4.Identity);
            GL.BindVertexArray(VertexArrayObject);
            GL.DrawElements(PrimitiveType.Lines, indices.Length, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);
        }
    }
}