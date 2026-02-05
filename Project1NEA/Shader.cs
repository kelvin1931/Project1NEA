using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Reflection.Metadata;


namespace Project1NEA
{
    internal class Shaders
    {
        public class Shader
        {
            public int Handle { get; private set; }
            private int VertexShader;
            private int FragmentShader;

            public Shader(string vertexPath, string fragmentPath)
            {
                string VertexShaderSource = File.ReadAllText(vertexPath);
                string FragmentShaderSource = File.ReadAllText(fragmentPath);
                
                VertexShader = GL.CreateShader(ShaderType.VertexShader);
                GL.ShaderSource(VertexShader, VertexShaderSource);

                FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
                GL.ShaderSource(FragmentShader, FragmentShaderSource);

                GL.CompileShader(VertexShader);

                GL.GetShader(VertexShader, ShaderParameter.CompileStatus, out int success);
                if (success == 0)
                {
                    string infoLog = GL.GetShaderInfoLog(VertexShader);
                    Console.WriteLine(infoLog);
                }

                GL.CompileShader(FragmentShader);

                GL.GetShader(FragmentShader, ShaderParameter.CompileStatus, out success);
                if (success == 0)
                {
                    string infoLog = GL.GetShaderInfoLog(FragmentShader);
                    Console.WriteLine(infoLog);
                }
                Handle = GL.CreateProgram();

                GL.AttachShader(Handle, VertexShader);
                GL.AttachShader(Handle, FragmentShader);

                GL.LinkProgram(Handle);

                GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out success);
                if (success == 0)
                {
                    string infoLog = GL.GetProgramInfoLog(Handle);
                    Console.WriteLine(infoLog);
                }
                
                GL.DetachShader(Handle, VertexShader);
                GL.DetachShader(Handle, FragmentShader);
                GL.DeleteShader(FragmentShader);
                GL.DeleteShader(VertexShader);
            }
            public void Use()
            {
                GL.UseProgram(Handle);
            }
            private bool disposedValue = false;

            protected virtual void Dispose(bool disposing)
            {
                if (!disposedValue)
                {
                    GL.DeleteProgram(Handle);

                    disposedValue = true;
                }
            }

            ~Shader()
            {
                if (disposedValue == false)
                {
                    Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
                }
            }

            public void SetMatrix4(string name, Matrix4 matrix)
            {
                int location = GL.GetUniformLocation(Handle, name);
                GL.UniformMatrix4(location, false, ref matrix);
            }


            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            public int GetAttribLocation(string attribName)
            {
                return GL.GetAttribLocation(Handle, attribName);
            }

            public void SetInt(string name, int value)
            {
                int location = GL.GetUniformLocation(Handle, name);
                GL.Uniform1(location, value);
            }

        }
    }
}
