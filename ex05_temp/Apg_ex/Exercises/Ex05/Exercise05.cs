using Apg_ex.Objects;
using KPGBase.Exercises;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK_tutorial.Objects;
using OpenTK_tutorial.Shaders;

namespace Apg_ex.Excercises.Ex05
{
    internal class Exercise05 : BasicExercise
    {
        public Vector3 camPos;

        // Object
        private MeshObject dillo;
        // Light
        private Cube light;
        // Render plane
        private Square plane;

        // Shaders
        ShaderProgram flatShader;
        ShaderProgram texShader;
        ShaderProgram fbShader;
       
        // Frame buffers
        FrameBuffer firstPass;
        FrameBuffer secondPass;
        
        Texture tex;

        public Exercise05(int width, int height) : base(width, height)
        {
            shader = new LightShader("Shaders/simple_shader.vert", "Shaders/simple_shader.frag");
            flatShader = new FlatShader("Shaders/flat_shader.vert", "Shaders/flat_shader.frag");
            texShader = new TextureShader("Shaders/tex_shader.vert", "Shaders/tex_shader.frag");
            fbShader = new FrameBufferShader("Shaders/frameBuffer_shader.vert", "Shaders/frameBuffer_shader.frag");

            firstPass = new FrameBuffer(width, height);
            secondPass = new FrameBuffer(width, height);

            // transparent empty bitmap
            tex = new Texture(new System.Drawing.Bitmap(5,5));
            tex.Initialize(TextureUnit.Texture1);

            dillo = new MeshObject(new Vector3(30f), new Vector3(-90, 0, 0), "Models/model.x", "Models/modelcolormap.png");
            // dillo = new MeshObject(new Vector3(0.5f), new Vector3(-90, 0, 0), "Models/utah.x", "Models/utahColor.png");
            dillo.Initialize();
            light = new Cube(new Vector3(10, 0, 0), new Vector3(), new Vector3(2));
            light.Initialize();

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
        }

        /// <summary>
        /// Main render call
        /// </summary>
        internal void Draw(Matrix4 view, Matrix4 projection)
        {
            ClearImage();

            shader.Use();
            ((LightShader)shader).SetUniforms(view, projection, new Vector3(1, 1, 1), light.Translation, new Vector3(camPos));
            dillo.Draw(shader);

            flatShader.Use();
            ((FlatShader)flatShader).SetUniforms(view, projection, new Vector3(1, 1, 0));
            light.Draw(flatShader);
        }

        /// <summary>
        /// First render pass - render simple textures only
        /// </summary>
        /// <param name="view"></param>
        /// <param name="projection"></param>
        public void RenderFirstPass(Matrix4 view, Matrix4 projection)
        {
            // set first pass framebuffer
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, firstPass.ID);
            ClearImage2();

            // TODO
            // render objects
            // - render textures only, this will be used for edge detection
            texShader.Use();
            ((TextureShader)texShader).SetUniforms(view, projection);
            dillo.Draw(texShader);
         
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        /// <summary>
        /// Second render pass - render shading
        /// </summary>
        /// <param name="view"></param>
        /// <param name="projection"></param>
        public void RenderSecondPass(Matrix4 view, Matrix4 projection)
        {
            // set second pass framebuffer
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, secondPass.ID);
            ClearImage();

            // TODO
            // render objects
            // - render lights, this image will then be displayed
            shader.Use();
            ((LightShader)shader).SetUniforms(view, projection, new Vector3(1, 1, 1), light.Translation, new Vector3(camPos));
            dillo.Draw(shader);

            flatShader.Use();
            ((FlatShader)flatShader).SetUniforms(view, projection, new Vector3(1, 1, 0));
            light.Draw(flatShader);


            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        /// <summary>
        /// Third render pass - combine results of first two passes, render onto the plane
        /// </summary>
        public void RenderThirdPass()
        {
            ClearImage();
            
            // OPTION ONE
            // render plane (in normalized coordinates) 
            // set its textures to use the correct textures from pass one and two
            //fbShader.Use();
            //fbShader.SetUniforms(Matrix4.Identity, Matrix4.Identity);
            //((FrameBufferShader)fbShader).SetTextures(firstPass.colorBuffer, secondPass.colorBuffer);
            //plane.Draw(fbShader);

            // OPTION TWO: COMPARISON
            // scale the plane down
            // draw it 4 times
            // each time with different translation and with use of different textures
            fbShader.Use();
            fbShader.SetUniforms(Matrix4.CreateScale(0.25f) * Matrix4.CreateTranslation(-1f, -1f, 0f), Matrix4.Identity);
            ((FrameBufferShader)fbShader).SetTextures(firstPass.colorBuffer, secondPass.colorBuffer);

            

            plane.Draw(fbShader);
        }
    }
}
