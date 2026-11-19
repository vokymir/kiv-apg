using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK_tutorial;

namespace Excercises
{
    internal class Exercise08 : BasicExercise
    {
        public Vector3 camPos;

        GameObject plane;
        GameObject volumeBox;

        ShaderProgram colorShader;

        // 3D texture
        Texture3D headVolumeData;

        // Frame buffers
        FrameBuffer firstPass;
        FrameBuffer secondPass;

        public Exercise08(int width, int height) : base(width, height)
        {
            plane = new Plane();
            plane.Initialize();
            volumeBox = new VolumeBox(new Vector3(0, 0, 0), new Vector3(), 1);
            volumeBox.Initialize();

            shader = new VolumeShader("shaders/volume_shader.vert", "shaders/volume_shader.frag");
            colorShader = new ShaderProgram("shaders/color_shader.vert", "shaders/color_shader.frag");

            firstPass = new FrameBuffer(width, height);
            secondPass = new FrameBuffer(width, height);

            headVolumeData = new Texture3D("Models/head256x256x109", 256, 256, 109);
        }

        /// <summary>
        /// If window size is changed, then frame buffers must change size
        /// </summary>
        public override void ChangeSize(int x, int y)
        {
            base.ChangeSize(x, y);

            firstPass = new FrameBuffer(x, y);
            secondPass = new FrameBuffer(x, y);
        }

        /// <summary>
        /// Rotate volume box
        /// </summary>
        public void Rotate(float x, float y)
        {
            volumeBox.SetRotation(new Vector3(x, y, 0));
        }

        /// <summary>
        /// Render front faces of the volume box
        /// </summary>
        public void RenderFrontFaces(Matrix4 view, Matrix4 projection)
        {
            // Render front faces to firstPass frame buffer
            // render volumeBox using colorShader
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, firstPass.ID);
            ClearImage();

            colorShader.Use();
            colorShader.SetUniforms(view, projection);
            volumeBox.Draw(colorShader);

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        /// <summary>
        /// Render back faces of the volume box
        /// </summary>
        public void RenderBackFaces(Matrix4 view, Matrix4 projection)
        {
            // Render back faces to secondPass frame buffer
            // render volumeBox using colorShader
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, secondPass.ID);
            ClearImage();

            // cull front faces
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Front);

            colorShader.Use();
            colorShader.SetUniforms(view, projection);
            volumeBox.Draw(colorShader);
            
            GL.Disable(EnableCap.CullFace);

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        /// <summary>
        /// Render the volume
        /// - use rendered front and back faces for color information
        /// - set 3D texture
        /// </summary>
        public void RenderDisplayPlane(Matrix4 view, Matrix4 projection)
        {
            ClearImage();

            // TODO

            // render data from framebuffers
            // render plane using volumeShader
            // - do not forget to set uniforms !! - textures and view and projection matrices
            // drawing on a display plane
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            ClearImage();

            shader.Use();
            ((VolumeShader)shader).SetUniforms(view, projection,firstPass.colorBuffer, secondPass.colorBuffer, headVolumeData.ID);
            plane.Draw(shader);

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

        }

    }
}
