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
            private int ElementBufferObject;
            private int _vao;
            private int _vbo;
            private int _ebo;

            private Stopwatch _timer;

            private Texture texture;
            private Texture texture2;

            private Vector3 cameraPos;
            private Vector3 cameraTarget;
            private Vector3 cameraDirection;
            private Vector3 cameraRight;
            private Vector3 cameraUp;
            private Vector3 cameraFront = new Vector3(0.0f, 0.0f, -1.0f);

            Vector3 position = new Vector3(0.0f, 0.0f, 3.0f);
            Vector3 worldUp = new Vector3(0.0f, 1.0f, 0.0f);

            private Vector2 _lastPos;
            private Vector2 mouse;
            private bool firstMove = true;

            private float pitch = 0f;
            private float yaw = -90f;
            private float movementSpeed = 5f;
            private float sensitivity = 0.1f;

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

            private Vector3[] _planetPositions =
                {

            new Vector3(2.0f,  0.0f,  0.0f),
            new Vector3(5.0f,  0.0f,  2.0f),
            new Vector3(-3.0f, 0.0f, -4.0f),
            new Vector3(8.0f,  0.0f,  0.0f),
            new Vector3(-6.0f, 0.0f, 3.0f),
            new Vector3(10.0f, 0.0f, -2.0f),
            new Vector3(12.0f, 0.0f,  5.0f),
            new Vector3(16.0f, 0.0f,  6.0f)

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
                        position += cameraFront * movementSpeed * (float)args.Time; //forward 
                    }

                    if (input.IsKeyDown(Keys.S))
                    {
                        position -= cameraFront * movementSpeed * (float)args.Time; //backwards
                    }

                    if (input.IsKeyDown(Keys.A))
                    {
                        position -= Vector3.Normalize(Vector3.Cross(cameraFront, worldUp)) * movementSpeed * (float)args.Time; //left
                    }

                    if (input.IsKeyDown(Keys.D))
                    {
                        position += Vector3.Normalize(Vector3.Cross(cameraFront, worldUp)) * movementSpeed * (float)args.Time; //right
                    }

                    if (input.IsKeyDown(Keys.Space))
                    {
                        position += worldUp * movementSpeed * (float)args.Time; //up 
                    }

                    if (input.IsKeyDown(Keys.LeftShift))
                    {
                        position -= worldUp * movementSpeed * (float)args.Time; //down
                    }


                    cameraFront = Vector3.Normalize(cameraFront);
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

                        yaw += deltaX * sensitivity;
                        pitch -= deltaY * sensitivity;

                        //clamping the pitch
                        if (pitch > 89f) pitch = 89f;
                        if (pitch < -89f) pitch = -89f;

                        cameraFront.X = (float)Math.Cos(MathHelper.DegreesToRadians(pitch)) * (float)Math.Cos(MathHelper.DegreesToRadians(yaw));
                        cameraFront.Y = (float)Math.Sin(MathHelper.DegreesToRadians(pitch));
                        cameraFront.Z = (float)Math.Cos(MathHelper.DegreesToRadians(pitch)) * (float)Math.Sin(MathHelper.DegreesToRadians(yaw));
                    }

                    cameraFront = Vector3.Normalize(cameraFront);
                }
            }
            #endregion
            #region OnLoad
            protected override void OnLoad()
            {
                base.OnLoad();
                GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
                GL.Enable(EnableCap.DepthTest);

                _vertices = CreateSphere(0.5f, 40, 40);
                _indices = CreateSphereIndices(40, 40);


                shader = new Shader("shader.vert", "Rshader.frag");

                _timer = Stopwatch.StartNew();

                _vao = GL.GenVertexArray();
                _vbo = GL.GenBuffer();
                _ebo = GL.GenBuffer();

                GL.BindVertexArray(_vao);

                //VBO
                GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
                GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);
                //EBO
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
                GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);
                //position
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
                GL.EnableVertexAttribArray(0);
                //texture
                GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
                GL.EnableVertexAttribArray(1);


                GL.GetInteger(GetPName.MaxVertexAttribs, out int maxAttributeCount);
                Debug.WriteLine($"Maximum number of vertex attributes supported: {maxAttributeCount}");

                shader.Use();

                shader.SetInt("texture1", 0); // TextureUnit.Texture0

                texture = new Texture("Textures/planetTex.png");

                cameraPos = new Vector3(0.0f, 0.0f, 3.0f);
                cameraTarget = Vector3.Zero;
                cameraDirection = Vector3.Normalize(cameraPos - cameraTarget);
                Vector3 up = Vector3.UnitY;
                cameraRight = Vector3.Normalize(Vector3.Cross(up, cameraDirection));
                cameraUp = Vector3.Cross(cameraDirection, cameraRight);



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

                shader.Use();

                //view and projection 
                Matrix4 view = Matrix4.LookAt(position, position + cameraFront, worldUp);
                Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), Size.X / (float)Size.Y, 0.1f, 100.0f);

                shader.SetMatrix4("view", view);
                shader.SetMatrix4("projection", projection);

                GL.BindVertexArray(_vao);
                texture.Use(TextureUnit.Texture0);

                // the SUN
                Matrix4 sunModel = Matrix4.Identity;
                shader.SetMatrix4("model", sunModel);
                GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);

                // Palanet
                for (int planetIndex = 0; planetIndex < _planetPositions.Length; planetIndex++)
                {
                    //rotation
                    float time = (float)_timer.Elapsed.TotalSeconds;
                float rotationAngle = 70.0f * planetIndex + time * 10.0f;
                
                Matrix4 model = Matrix4.Identity;
                
                //orbit around sun
                model *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(rotationAngle));

                //move planet away from sun
                model *= Matrix4.CreateTranslation(_planetPositions[planetIndex]);

                //scale planet
                model *= Matrix4.CreateScale(0.3f + (planetIndex * 0.1f));

               

                //mathy part 
               
                float radius = 3.0f + planetIndex * 2.0f;     // distance from sun
                float angularVelocity = 0.5f + planetIndex * 0.2f; // w

                // actual orbital equation
                float x = radius * MathF.Cos(angularVelocity * time);
                float z = radius * MathF.Sin(angularVelocity * time);

                // model matrix
                model = Matrix4.CreateTranslation(x, 0.0f, z);

                //spin on its own axis
                model *= Matrix4.CreateRotationY(time * 2.0f);

                model *= Matrix4.CreateScale(0.3f + (planetIndex * 0.1f));

                Debug.WriteLine(time);

                    shader.SetMatrix4("model", model);

                    GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
                }

                Context.SwapBuffers();

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

                    for (int sectorIndex = 0; sectorIndex <= sectors; sectorIndex++)
                    {
                        float sectorAngle = sectorIndex * 2 * MathF.PI / sectors;

                        float xPosition = xy * MathF.Cos(sectorAngle);
                        float yPosition = xy * MathF.Sin(sectorAngle);

                        //position 
                        vertices.Add(xPosition);
                        vertices.Add(yPosition);
                        vertices.Add(z);

                        //texture coordinate section 
                        float textureU = (float)sectorIndex / sectors;
                        float textureV = (float)i / stacks;

                        vertices.Add(textureU);
                        vertices.Add(textureV);
                    }
                }

                return vertices.ToArray();
            }


            private uint[] CreateSphereIndices(int stacks, int sectors)
            {
                List<uint> indices = new List<uint>();

                for (int i = 0; i < stacks; i++)
                {
                    int currentStackStart = i * (sectors + 1);
                    int nextStackStart = currentStackStart + sectors + 1;

                    for (int sectorIndex = 0; sectorIndex < sectors; sectorIndex++, currentStackStart++, nextStackStart++)
                    {
                        indices.Add((uint)currentStackStart);
                        indices.Add((uint)nextStackStart);
                        indices.Add((uint)(currentStackStart + 1));

                        indices.Add((uint)(currentStackStart + 1));
                        indices.Add((uint)nextStackStart);
                        indices.Add((uint)(nextStackStart + 1));
                    }
                }

                return indices.ToArray();
            }

            #endregion
        } 
    } 
