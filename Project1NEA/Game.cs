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
        private Vector2 _lastPos;
        private Vector2 mouse;
        private float pitch = 0.1f;
        private float yaw = 0.1f;
        private float sensitivity = 0.1f;
        private bool firstMove = true;
        Vector3 front = new Vector3(0.0f, 0.0f, -1.0f);
        private int _vao;
        private int _vbo;
        private int _ebo;
        private float[] _vertices;
        private uint[] _indices;


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
    -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,

    -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
     0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
     0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
     0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
    -0.5f,  0.5f,  0.5f,  0.0f, 1.0f,
    -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,

    -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,
     0.5f, -0.5f, -0.5f,  1.0f, 0.0f,
     0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
     0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
    -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
    -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,

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

    -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
     0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
     0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
     0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
    -0.5f,  0.5f,  0.5f,  0.0f, 0.0f,
    -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,

    -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
     0.5f, -0.5f, -0.5f,  1.0f, 1.0f,
     0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
     0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
    -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
    -0.5f, -0.5f, -0.5f,  0.0f, 1.0f

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


                front = Vector3.Normalize(front);
                var mouse = MouseState.Position;

                if (firstMove)
                {
                    _lastPos = new Vector2(mouse.X, mouse.Y);
                    firstMove = false;
                }
                else
                {
                    float deltaX = mouse.X - _lastPos.X;
                    float deltaY = mouse.Y - _lastPos.Y;
                    _lastPos = new Vector2(mouse.X, mouse.Y);

                    Yaw += deltaX * sensitivity;
                    Pitch -= deltaY * sensitivity;

                    // Clamp the pitch
                    if (Pitch > 89f) Pitch = 89f;
                    if (Pitch < -89f) Pitch = -89f;

                    front.X = (float)Math.Cos(MathHelper.DegreesToRadians(Pitch)) * (float)Math.Cos(MathHelper.DegreesToRadians(Yaw));
                    front.Y = (float)Math.Sin(MathHelper.DegreesToRadians(Pitch));
                    front.Z = (float)Math.Cos(MathHelper.DegreesToRadians(Pitch)) * (float)Math.Sin(MathHelper.DegreesToRadians(Yaw));
                }

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

            _vertices = CreateSphere(0.5f, 40, 40);
            _indices = CreateSphereIndices(40, 40);


            shader = new Shader("shader.vert", "Rshader.frag");

            _timer = Stopwatch.StartNew();

            _vao = GL.GenVertexArray();
            _vbo = GL.GenBuffer();
            _ebo = GL.GenBuffer();

            GL.BindVertexArray(_vao);

            // VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            // EBO
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

            // Position
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // Texture
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);


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



            CursorState = CursorState.Grabbed;


            GL.Enable(EnableCap.DepthTest);

            //someOpenGLFunctionThatDrawsOurTriangle();
        }

        #endregion
        #region OnMouseMove
        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);

        }
#endregion
        #region RenderFrame
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);


            Matrix4 rotation = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(90f));
            Matrix4 translation = Matrix4.CreateTranslation(0.5f, 0.0f, 0.0f);
            Matrix4 scale = Matrix4.CreateScale(1.0f);
            Matrix4 transform = rotation * scale;

            //Matrix4 model = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(-55.0f));
            Matrix4 model = Matrix4.Identity;
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), Size.X / (float)Size.Y, 0.1f, 100.0f);
            shader.Use();

            Matrix4 view = Matrix4.LookAt(position, position + front, Up);


            shader.SetMatrix4("model", model);
            shader.SetMatrix4("view", view);
            shader.SetMatrix4("projection", projection);

            shader.SetMatrix4("transform", transform);

            texture.Use(TextureUnit.Texture0);
            texture2.Use(TextureUnit.Texture1);

            GL.Clear(ClearBufferMask.ColorBufferBit |
            ClearBufferMask.DepthBufferBit);

            GL.BindVertexArray(_vao);

            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);



            //GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
            

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
        #region CreateSphere
        private float[] CreateSphere(float radius, int stacks, int sectors)
        {
            List<float> vertices = new List<float>();

            for (int i = 0; i <= stacks; i++)
            {
                float stackAngle = MathF.PI / 2 - i * MathF.PI / stacks;
                float xy = radius * MathF.Cos(stackAngle);
                float z = radius * MathF.Sin(stackAngle);

                for (int j = 0; j <= sectors; j++)
                {
                    float sectorAngle = j * 2 * MathF.PI / sectors;

                    float x = xy * MathF.Cos(sectorAngle);
                    float y = xy * MathF.Sin(sectorAngle);

                    // position
                    vertices.Add(x);
                    vertices.Add(y);
                    vertices.Add(z);

                    // texture coordinates
                    float u = (float)j / sectors;
                    float v = (float)i / stacks;

                    vertices.Add(u);
                    vertices.Add(v);
                }
            }

            return vertices.ToArray();
        }


        private uint[] CreateSphereIndices(int stacks, int sectors)
        {
            List<uint> indices = new List<uint>();

            for (int i = 0; i < stacks; i++)
            {
                int k1 = i * (sectors + 1);
                int k2 = k1 + sectors + 1;

                for (int j = 0; j < sectors; j++, k1++, k2++)
                {
                    indices.Add((uint)k1);
                    indices.Add((uint)k2);
                    indices.Add((uint)(k1 + 1));

                    indices.Add((uint)(k1 + 1));
                    indices.Add((uint)k2);
                    indices.Add((uint)(k2 + 1));
                }
            }

            return indices.ToArray();
        }

        #endregion
    }

}