using Apg_ex.Exercises.Ex06;
using Apg_ex.Shaders;
using KPGBase.Exercises;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK_tutorial.Objects;
using OpenTK_tutorial.Shaders;

namespace Apg_ex.Excercises.Ex05
{
    internal class Exercise06 : BasicExercise
    {
        // Camera position
        public Vector3 camPos;

        // Object
        private PointCloud cloud;
        // Render plane
        private Square plane;

        // Shaders
        ShaderProgram occlusionShader;
        ShaderProgram holefillingShader;
       
        // Frame buffers
        FrameBuffer firstPass;
        FrameBuffer secondPass;
        FrameBuffer swapPass;

        public Exercise06(int width, int height) : base(width, height)
        {
            shader = new PointcloudShader("Shaders/pointcloud_shader.vert", "Shaders/pointcloud_shader.frag");
            occlusionShader = new PostProcessingPCShader("Shaders/frameBuffer_shader.vert", "Shaders/occlusionFilter_shader.frag");
            holefillingShader = new PostProcessingPCShader("Shaders/frameBuffer_shader.vert", "Shaders/holeFilling_shader.frag");

            firstPass = new FrameBuffer(width, height);
            secondPass = new FrameBuffer(width, height);
            swapPass = new FrameBuffer(width, height);

            cloud = new PointCloud(new Vector3(30f), new Vector3(-90, 0, 0), new Vector3(0), "Models/model.x", "Models/modelcolormap.png");
            // cloud = new PointCloud(new Vector3(0.25f), new Vector3(-20, -10, -10), "Models/GreenBelt.txt");
            // cloud = new PointCloud(new Vector3(40), new Vector3(0), new Vector3(0, 0, 0), "Models/Bunny.txt");
            cloud.Initialize();

            // display square
            plane = new Square(new Vector3(-1, -1, 0), new Vector3(0, 0, 0), new Vector3(2, 2, 1));
            plane.Initialize();

        }

        /// <summary>
        /// If window size is changed, then frame buffers must change size
        /// </summary>
        public override void ChangeSize(int x, int y)
        {
            base.ChangeSize(x, y);

            firstPass = new FrameBuffer(x, y);
            secondPass = new FrameBuffer(x, y);
            swapPass = new FrameBuffer(x, y);
        }

        /// <summary>
        /// Main render call
        /// </summary>
        internal void Draw(Matrix4 view, Matrix4 projection)
        {
            ClearImage();

            shader.Use();
            ((PointcloudShader)shader).SetUniforms(view, projection, camPos);
            cloud.Draw(shader);
        }

        /// <summary>
        /// First render pass - render pointcloud
        /// </summary>
        /// <param name="view"></param>
        /// <param name="projection"></param>
        public void RenderFirstPass(Matrix4 view, Matrix4 projection)
        {
            // set first pass framebuffer
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, firstPass.ID);
            ClearImageBlack(); // !! this means that the background will be BLACK !!

            // render
            shader.Use();
            ((PointcloudShader)shader).SetUniforms(view, projection, camPos);
            cloud.Draw(shader);

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        /// <summary>
        /// Second render pass - occlusion filtering
        /// </summary>
        public void PostProcessOcclusion()
        {
            // set second pass framebuffer
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, secondPass.ID);
            ClearImageBlack();

            // use occlusion filter shader
            occlusionShader.Use();
            occlusionShader.SetUniforms(Matrix4.Identity, Matrix4.Identity);
            ((PostProcessingPCShader)occlusionShader).SetOcclusionUniforms(
                firstPass.colorBuffer,
                firstPass.depthBuffer,
                new Vector3(0f, 0f, 0f), // background was cleared with black
                0.00001f, // minD = how sensitive to differences of neighbors we are
                new Vector2(1.0f / width, 1.0f / height));
            plane.Draw(occlusionShader); // draw onto a plane

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        /// <summary>
        /// Third render pass - apply hole filling
        /// (needs more iterations, every time fills just one "layer")
        /// </summary>
        /// <param name="iterations"> number of iterations </param>
        public void PostProcessHoleFill(int iterations = 10)
        {
            ClearImageBlack();

            FrameBuffer srcFB = secondPass; // starting with processing the second pass
            FrameBuffer dstFB = swapPass; // empty for now - will draw into it

            // use hole filling shader
            holefillingShader.Use();
            holefillingShader.SetUniforms(Matrix4.Identity, Matrix4.Identity);
                              
            // iterate !!
            for (int i = 0; i < iterations; i++)
            {
                // TODO
                ((PostProcessingPCShader)holefillingShader).SetHolefillUniforms(
                srcFB.colorBuffer,
                srcFB.depthBuffer,
                new Vector3(0f, 0f, 0f), // background was cleared with black
                new Vector2(1.0f / width, 1.0f / height));


                // on the final iteration render to the screen (bind FBO 0)
                // else bind FBO dstFB
                // - do not forget to clear the screen
                if (i == iterations - 1)
                {
                    GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
                }else
                {
                    GL.BindFramebuffer(FramebufferTarget.Framebuffer, dstFB.ID);
                }
                ClearImageBlack();

                // use hole filling shader
                // draw plane
                plane.Draw(holefillingShader);

                // swap source and destination framebuffers for the next pass
                FrameBuffer tmp = srcFB;
                srcFB = dstFB; dstFB = tmp;
                //srcFB = dstFB;
            }
        }
    }
}
