using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Project1NEA;
using static Project1NEA.Shaders;

namespace Project1NEA
{
    public class Game :GameWindow
    {
        private Shader shader;
        int VertexBufferObject;
        int VertexArrayObject;
        int ElementBufferObject;
        float[] vertices =
            {
             0.5f,  0.5f, 0.0f, //Top Right vertex
             0.5f, -0.5f, 0.0f, //Bottom Right vertex
             -0.5f,  -0.5f, 0.0f,  //Bottom Left vertex
             -0.5f,  0.5f, 0.0f   // Top Left vertex
             };
        uint[] indices = 
            { 
            0, 1, 3,  
            1, 2, 3   
        };

        public Game(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title }) 
        { }

        public static void Main(string[] args)
        {
            using (Game game = new Game(1440, 1080, "GameWindow")) //1440,1080
            {
                game.Run();
            }
        }

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
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            shader = new Shader("shader.vert", "Rshader.frag");

            // VAO
            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            // VBO
            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer,vertices.Length * sizeof(float),vertices,BufferUsageHint.StaticDraw);

            // Link vertex attributes
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);



            //someOpenGLFunctionThatDrawsOurTriangle();
        }

    #endregion

    #region RenderFrame
    protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            
            GL.Clear(ClearBufferMask.ColorBufferBit);


            GL.UseProgram(shader.Handle);
            GL.BindVertexArray(VertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
    

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

        
    }

}