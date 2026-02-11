using Microsoft.VisualBasic;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Project1NEA;
using System.Diagnostics;
using System.Reflection;
using static Project1NEA.Shaders;
using static System.Net.Mime.MediaTypeNames;

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
        private Texture texture;
        private Texture texture2;
        private Vector3 cameraPos;
        private Vector3 cameraTarget;
        private Vector3 cameraDirection;
        private Vector3 cameraRight;
        private Vector3 cameraUp;
        float speed = 5f;
        Vector3 position = new Vector3(0.0f, 0.0f, 3.0f);
        Vector3 Up = new Vector3(0.0f, 1.0f, 0.0f);
        float Yaw = -90f;   // start facing forward
        float Pitch = 0f;
        Vector3 front = new Vector3(0.0f, 0.0f, -1.0f);

        /* float[] vertices =
            {
            //Position          Texture coordinates
            0.5f,  0.5f, 0.0f, 1.0f, 1.0f, // top right
            0.5f, -0.5f, 0.0f, 1.0f, 0.0f, // bottom right
            -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, // bottom left
            -0.5f,  0.5f, 0.0f, 0.0f, 1.0f  // top left
             }; */
        uint[] indices =
            { 0, 1, 3,1 ,2 ,3};

        public float[] texCoords =
            {
            0.0f, 0.0f, //lower left vertex
            1.0f, 0.0f, // lower right vertex
            0.5f, 1.0f // top centre vertex
            };

        #endregion
        #region CUBE VETEX
        float[] vertice = {
    -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,
     0.5f, -0.5f, -0.5f,  1.0f, 0.0f,
     0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
     0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
    -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
    -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,

    -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
     0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
     0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
     0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
    -0.5f,  0.5f,  0.5f,  0.0f, 1.0f,
    -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,

    -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
    -0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
    -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
    -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
    -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
    -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

     0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
     0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
     0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
     0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
     0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
     0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

    -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
     0.5f, -0.5f, -0.5f,  1.0f, 1.0f,
     0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
     0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
    -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
    -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,

    -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
     0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
     0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
     0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
    -0.5f,  0.5f,  0.5f,  0.0f, 0.0f,
    -0.5f,  0.5f, -0.5f,  0.0f, 1.0f
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

            {
                if (!IsFocused) //checks to see if the window is focusd
                {
                    return;
                }

                KeyboardState input = KeyboardState;



                if (input.IsKeyDown(Keys.W))
                {
                    position += front * speed * (float)args.Time; //Forward 
                }

                if (input.IsKeyDown(Keys.S))
                {
                    position -= front * speed * (float)args.Time; //Backwards
                }

                if (input.IsKeyDown(Keys.A))
                {
                    position -= Vector3.Normalize(Vector3.Cross(front, Up)) * speed * (float)args.Time; //Left
                }

                if (input.IsKeyDown(Keys.D))
                {
                    position += Vector3.Normalize(Vector3.Cross(front, Up)) * speed * (float)args.Time; //Right
                }

                if (input.IsKeyDown(Keys.Space))
                {
                    position += Up * speed * (float)args.Time; //Up 
                }

                if (input.IsKeyDown(Keys.LeftShift))
                {
                    position -= Up * speed * (float)args.Time; //Down
                }


                front.Y = (float)Math.Sin(MathHelper.DegreesToRadians(Pitch)); // Note that we convert the angle to radians first
                front.X = (float)Math.Cos(MathHelper.DegreesToRadians(Pitch));
                front.Z = (float)Math.Cos(MathHelper.DegreesToRadians(Pitch));


                front.X = (float)Math.Cos(MathHelper.DegreesToRadians(Pitch)) * (float)Math.Cos(MathHelper.DegreesToRadians(Yaw));
                front.Y = (float)Math.Sin(MathHelper.DegreesToRadians(Pitch));
                front.Z = (float)Math.Cos(MathHelper.DegreesToRadians(Pitch)) * (float)Math.Sin(MathHelper.DegreesToRadians(Yaw));

                front = Vector3.Normalize(front);

            }
        }
        #endregion

        #region OnLoad
        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(0.3f, 0.0f, 0.5f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

            shader = new Shader("shader.vert", "Rshader.frag");

            _timer = Stopwatch.StartNew();

            // VAO
            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            // VBO
            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer,vertice.Length * sizeof(float),vertice,BufferUsageHint.StaticDraw);

            // Position attribute
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // Texture coordinate attribute
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);
            

            //EBO
            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            GL.GetInteger(GetPName.MaxVertexAttribs, out int maxAttributeCount);
            Debug.WriteLine($"Maximum number of vertex attributes supported: {maxAttributeCount}");

            shader.Use();

            shader.SetInt("texture1", 0); // TextureUnit.Texture0
            shader.SetInt("texture2", 1); // TextureUnit.Texture1

            texture = new Texture("walling - Copy (3).png");
            texture2 = new Texture("awesomeface - Copy (3).png");

            Vector3 cameraPos = new Vector3(0.0f, 0.0f, 3.0f);
            Vector3 cameraTarget = Vector3.Zero;
            Vector3 cameraDirection = Vector3.Normalize(cameraPos - cameraTarget);
            Vector3 up = Vector3.UnitY;
            Vector3 cameraRight = Vector3.Normalize(Vector3.Cross(up, cameraDirection));
            Vector3 cameraUp = Vector3.Cross(cameraDirection, cameraRight);


           






            //someOpenGLFunctionThatDrawsOurTriangle();
        }

        #endregion

        #region RenderFrame
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.BindVertexArray(VertexArrayObject);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);


            Matrix4 rotation = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(90f));
            Matrix4 translation = Matrix4.CreateTranslation(0.5f, 0.0f, 0.0f);
            Matrix4 scale = Matrix4.CreateScale(1.0f);
            Matrix4 transform = rotation * scale;

            Matrix4 model = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(-55.0f));
            Matrix4 view = Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f);
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), Size.X / (float)Size.Y, 0.1f, 100.0f);
            shader.Use();




            shader.SetMatrix4("model", model);
            shader.SetMatrix4("view", view);
            shader.SetMatrix4("projection", projection);

            shader.SetMatrix4("transform", transform);

            texture.Use(TextureUnit.Texture0);
            texture2.Use(TextureUnit.Texture1);

            view = Matrix4.LookAt(position, position + front, Up);

            //GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

            Context.SwapBuffers();

            base.OnRenderFrame(e);
        }
        #endregion

        #region FrameBuffer
        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertice.Length * sizeof(float), vertice, BufferUsageHint.StaticDraw);

 
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