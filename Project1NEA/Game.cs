using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Project1NEA;
using System.Diagnostics;
using static Project1NEA.Shaders;

namespace Project1NEA
{
    public class Game : GameWindow
    {
        #region variables
        private Shader shader;
        private int VertexBufferObject;
        private int VertexArrayObject;
        int ElementBufferObject;
        private Stopwatch _timer;
        float[] vertices =
            {
             // positions        // colors
             0.5f, -0.5f, 0.0f,  0.9f, 0.5f, 0.3f,   // bottom right
            -0.5f, -0.5f, 0.0f,  0.3f, 0.9f, 0.5f,   // bottom left
             0.0f,  0.5f, 0.0f,  0.5f, 0.3f, 0.9f    // top 
             };
        
        uint[] indices =
            { 0, 1, 2};

        float[] texCoords =
            {
            0.0f, 0.0f, //lower left vertex
            1.0f, 0.0f, // lower right vertex
            0.5f, 1.0f // top centre vertex
            };

        #endregion

        public Game(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title }) 
        { }

        #region MAIN
        public static void Main(string[] args)
        {
            using (Game game = new Game(1440, 1080, "GameWindow")) //1440,1080
            {
                game.Run();
            }
        }
        #endregion

        #region UpdateFrame
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }
        #endregion

        #region OnLoad
        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(0.1f, 0.1f, 0.1f, 0.1f);

            shader = new Shader("shader.vert", "Rshader.frag");

            _timer = Stopwatch.StartNew();

            // VAO
            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            // VBO
            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer,vertices.Length * sizeof(float),vertices,BufferUsageHint.StaticDraw);

            // Link vertex attributes
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            GL.GetInteger(GetPName.MaxVertexAttribs, out int maxAttributeCount);
            Debug.WriteLine($"Maximum number of vertex attributes supported: {maxAttributeCount}");

            shader.Use();

            //texture wrapping
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.MirroredRepeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.MirroredRepeat);

            //texture filtering
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            //minimap stuff
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);


            //someOpenGLFunctionThatDrawsOurTriangle();
        }

    #endregion  

    #region RenderFrame
    protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            shader.Use();

            

            double timeValue = _timer.Elapsed.TotalSeconds;
            float greenValue = (float)Math.Sin(timeValue) / 2.0f + 0.5f;

            GL.BindVertexArray(VertexArrayObject);

            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            SwapBuffers();
        }
        #endregion

        #region FrameBuffer
        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

 
        }
        #endregion


        #region OnResize
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Size.X, Size.Y);
        }
        #endregion

        #region onUnload
        protected override void OnUnload()
        {

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            // Delete all the resources.
            GL.DeleteBuffer(VertexBufferObject);
            GL.DeleteVertexArray(VertexArrayObject);

            GL.DeleteProgram(shader.Handle);

            base.OnUnload();
        }
        #endregion

    }

}