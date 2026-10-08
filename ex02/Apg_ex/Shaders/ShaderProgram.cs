using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Diagnostics;

namespace OpenTK_tutorial.Shaders
{
    internal class  ShaderProgram : IDisposable
    {
        /// <summary> Program handle </summary>
        internal int Handle;

        int VertexShader;
        int FragmentShader;

        private bool disposedValue = false;
        internal Stopwatch t;

        Matrix4 rotate;
        Matrix4 translate;

        public ShaderProgram(string vertexPath, string fragmentPath)
        {
            t = new Stopwatch();
            t.Start();

            rotate = Matrix4.CreateRotationX((float)(0 * 180/Math.PI));
            translate = Matrix4.CreateTranslation(0, 0, 0);

            string VertexShaderSource = File.ReadAllText(vertexPath);
            string FragmentShaderSource = File.ReadAllText(fragmentPath);

            CreateShaders(VertexShaderSource, FragmentShaderSource);
            CompileShaders();
            LinkShaders();

            // now we can clean up
            GL.DetachShader(Handle, VertexShader);
            GL.DetachShader(Handle, FragmentShader);
            GL.DeleteShader(FragmentShader);
            GL.DeleteShader(VertexShader);
        }

        private void CreateShaders(string VertexShaderSource, string FragmentShaderSource)
        {
            VertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(VertexShader, VertexShaderSource);

            FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentShader, FragmentShaderSource);
        }

        void CompileShaders()
        {
            GL.CompileShader(VertexShader);

            GL.GetShader(VertexShader, ShaderParameter.CompileStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(VertexShader);
                Console.WriteLine(infoLog);
            }

            GL.CompileShader(FragmentShader);

            GL.GetShader(FragmentShader, ShaderParameter.CompileStatus, out int success2);
            if (success2 == 0)
            {
                string infoLog = GL.GetShaderInfoLog(FragmentShader);
                Console.WriteLine(infoLog);
            }
        }

        void LinkShaders()
        {
            Handle = GL.CreateProgram();

            GL.AttachShader(Handle, VertexShader);
            GL.AttachShader(Handle, FragmentShader);

            GL.LinkProgram(Handle);

            GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetProgramInfoLog(Handle);
                Console.WriteLine(infoLog);
            }
        }

        public void Use()
        {
            GL.UseProgram(Handle);
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteProgram(Handle);

                disposedValue = true;
            }
        }

        ~ShaderProgram()
        {
            if (disposedValue == false)
            {
                Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            t.Stop();
        }

        internal virtual void SetUniforms(Matrix4 view, Matrix4 projection)
        {
            int location = GL.GetUniformLocation(Handle, "view");
            GL.UniformMatrix4(location, true, ref view);
            location = GL.GetUniformLocation(Handle, "projection");
            GL.UniformMatrix4(location, true, ref projection);

        }

        internal virtual void SetUniforms(Matrix4 view, Matrix4 projection, Vector3 baseColor)
        {
            int location = GL.GetUniformLocation(Handle, "view");
            GL.UniformMatrix4(location, true, ref view);
            location = GL.GetUniformLocation(Handle, "projection");
            GL.UniformMatrix4(location, true, ref projection);
            location = GL.GetUniformLocation(Handle, "baseColor");
            GL.Uniform3(location, baseColor);
        }

        internal virtual void SetModel(Matrix4 model)
        {
            int location = GL.GetUniformLocation(Handle, "model");
            GL.UniformMatrix4(location, true, ref model);
        }
    }
}
