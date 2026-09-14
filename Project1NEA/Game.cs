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
            private Menu menu;
            private TextRenderer textRenderer;

            // Earth is index 2 of the orbital data arrays.
            private const int EarthIndex = 2;
            private Stopwatch _timer;
            private GameState gameState = GameState.MainMenu;

            private Texture texture;
            private Texture texture2;
            private Texture texture3;
            private Texture texture4;
            private Texture texture5;
            private Texture texture6;
            private Texture texture7;
            private Texture texture8;
            private Texture texture9;
            private Texture texture10;

            private bool isCTRLdown = false;
    
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

        #region SMaxis,Eccent,Pscale,OrbPeriod.rotationSpeeds



        float[] semiMajorAxes =
            {
            3.0f,   // Mercury
            5.0f,   // Venus
            7.0f,   // Earth
            9.0f,  // Mars
            14.0f,  // Jupiter
            20.0f,  // Saturn
            27.0f,  // Uranus
            34.0f   // Neptune
            };
        
        float[] eccentricities =
            {
            0.2056f,
            0.0067f,
            0.0167f,
            0.0934f,
            0.0489f,
            0.0565f,
            0.0463f,
            0.0097f
            };

        float[] planetScales =
            {
            0.24f, // Mercury
            0.54f, // Venus
            0.6f, // Earth
            0.3f, // Mars
            2.1f, // Jupiter
            1.8f, // Saturn
            1.08f, // Uranus
            1.02f  // Neptune
        };

        float[] orbitalPeriods =
            {
            0.24f,
            0.62f,
            1.0f,
            1.88f,
            11.86f,
            29.46f,
            84.01f,
            164.8f
        };


        // ORBITAL INCLINATION in degrees
        float[] inclinations =
            {
            7.0f,   // Mercury
            3.4f,   // Venus
            0.0f,   // Earth
            1.85f,  // Mars
            1.3f,   // Jupiter
            2.5f,   // Saturn
            0.8f,   // Uranus
            1.8f    // Neptune
        };

        float[] rotationSpeeds =
            {
            1.0f,
            -0.2f,
            1.0f,
            0.97f,
            2.4f,
            2.2f,
            -1.4f,
            1.5f
        };

        #endregion
        public Game(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title })
            { }

            #region MAIN

            public static void Main(string[] args)
            {
                using (Game game = new Game(2560, 1600, "GameWindow")) //1920 x 1200
                {
                    game.Run();
                }
            }
            #endregion

            #region UpdateFrame

            protected override void OnUpdateFrame(FrameEventArgs args)
            {
                base.OnUpdateFrame(args);
            if (gameState != GameState.Playing)
            {
                gameState = menu.Update(KeyboardState, gameState);

                if (menu.ExitRequested)
                {
                    Close();
                    return;
                }

                if (gameState == GameState.Playing)
                {
                    EnterSimulation();
                }

                return;
            }

            // Escape returns to the menu rather than closing, so the user can
            // always get back without losing the simulation.
            if (KeyboardState.IsKeyPressed(Keys.Escape))
            {
                gameState = GameState.MainMenu;
                CursorState = CursorState.Normal;
                return;
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

                if (input.IsKeyDown(Keys.LeftControl) & input.IsKeyDown(Keys.W))
                {
                    position += cameraFront * movementSpeed * 10 * (float)args.Time; //Speed forward 
                }

                if (input.IsKeyDown(Keys.LeftControl) & input.IsKeyDown(Keys.S))
                {
                    position -= cameraFront * movementSpeed * 10 * (float)args.Time; //Speed Backwards 
                }

                if (input.IsKeyPressed(Keys.RightShift))

                {
                    position = new Vector3(0.0f, 0.0f, 3.0f);
                }
                if (input.IsKeyDown(Keys.S))
                {
                    position -= cameraFront * movementSpeed * (float)args.Time; //backwards
                }

                if (input.IsKeyDown(Keys.A))
                {
                    position -= Vector3.Normalize(Vector3.Cross(cameraFront, worldUp)) * movementSpeed * (float)args.Time; //left
                }
                if (input.IsKeyDown(Keys.A) && input.IsKeyDown(Keys.LeftControl))
                {
                    position -= Vector3.Normalize(Vector3.Cross(cameraFront, worldUp)) * movementSpeed * 10 * (float)args.Time; // Speed left
                }

                if (input.IsKeyDown(Keys.D))
                {
                    position += Vector3.Normalize(Vector3.Cross(cameraFront, worldUp)) * movementSpeed * (float)args.Time; //right
                }
                if (input.IsKeyDown(Keys.D) && input.IsKeyDown(Keys.LeftControl))
                {
                    position += Vector3.Normalize(Vector3.Cross(cameraFront, worldUp)) * movementSpeed * 10 * (float)args.Time; //Speed right
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
                GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.DynamicDraw);
                //EBO
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
                GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.DynamicDraw);
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
                
            
            
            
            texture = new Texture("Textures/sunTex.png");
            texture2 = new Texture("Textures/starfinale.png");
            texture3 = new Texture("Textures/Mercury.png");
            texture4 = new Texture("Textures/Venus.png");
            texture5 = new Texture("Textures/Earth (2).png");
            texture6 = new Texture("Textures/Mars.png");
            texture7 = new Texture("Textures/Jupiter.png");
            texture8 = new Texture("Textures/Saturn.png");
            texture9 = new Texture("Textures/Uranus.png");
            texture10 = new Texture("Textures/Neptune.png");

            cameraPos = new Vector3(0.0f, 0.0f, 3.0f);
            cameraTarget = Vector3.Zero;
            cameraDirection = Vector3.Normalize(cameraPos - cameraTarget);
            Vector3 up = Vector3.UnitY;
            cameraRight = Vector3.Normalize(Vector3.Cross(up, cameraDirection));
            cameraUp = Vector3.Cross(cameraDirection, cameraRight);

            textRenderer = new TextRenderer("Textures/font.png");
            textRenderer.Resize(Size.X, Size.Y);

            menu = new Menu();
            menu.Load(textRenderer);

            // The pointer is only captured once the simulation starts, so the
            // menu can be used normally.
            CursorState = CursorState.Normal;


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
            if (gameState != GameState.Playing)
            {
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                menu.Render(gameState, Size.X, Size.Y);

                Context.SwapBuffers();
            }
            else
            {
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                shader.Use();

                //view and projection 
                Matrix4 view = Matrix4.LookAt(position, position + cameraFront, worldUp);
                Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), Size.X / (float)Size.Y, 0.1f, 100.0f);

                shader.SetMatrix4("view", view);
                shader.SetMatrix4("projection", projection);

                GL.BindVertexArray(_vao);



                // stars
                texture2.Use(TextureUnit.Texture0);
                Matrix4 Stars = Matrix4.CreateScale(200f);
                shader.SetMatrix4("model", Stars);
                GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);


                // the SUN
                texture.Use(TextureUnit.Texture0);
                Matrix4 sunModel = Matrix4.CreateScale(1.5f);
                shader.SetMatrix4("model", sunModel);
                GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);


                for (int planetIndex = 0; planetIndex < semiMajorAxes.Length; planetIndex++)
                {
                    if (planetIndex == 0)
                    {
                        texture3.Use(TextureUnit.Texture0);
                    }
                    if (planetIndex == 1)
                    {
                        texture4.Use(TextureUnit.Texture0);
                    }
                    if (planetIndex == 2)
                    {
                        texture5.Use(TextureUnit.Texture0);
                    }
                    if (planetIndex == 3)
                    {
                        texture6.Use(TextureUnit.Texture0);
                    }
                    if (planetIndex == 4)
                    {
                        texture7.Use(TextureUnit.Texture0);
                    }
                    if (planetIndex == 5)
                    {
                        texture8.Use(TextureUnit.Texture0);
                    }
                    if (planetIndex == 6)
                    {
                        texture9.Use(TextureUnit.Texture0);
                    }
                    if (planetIndex == 7)
                    {
                        texture10.Use(TextureUnit.Texture0);
                    }
                    float time = SimulationTime;
                    float scale = planetScales[planetIndex];

                    Vector3 orbitPosition = GetPlanetPosition(planetIndex, time);


                    //modl matrix

                    Matrix4 scaleMatrix = Matrix4.CreateScale(scale);

                    Matrix4 rotation = Matrix4.CreateRotationY(time * rotationSpeeds[planetIndex]);

                    Matrix4 translation = Matrix4.CreateTranslation(orbitPosition);

                    Matrix4 model = scaleMatrix * rotation * translation;

                    shader.SetMatrix4("model", model);

                    GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
                }

                Context.SwapBuffers();

            }

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

                if (textRenderer != null)
                {
                    textRenderer.Resize(Size.X, Size.Y);
                }
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
            #region Simulation helpers

            /// <summary>
            /// Elapsed simulation time. The offset keeps the planets away from
            /// their shared starting line, so they are not all in a row.
            /// </summary>
            private float SimulationTime
            {
                get { return (float)_timer.Elapsed.TotalSeconds + 300; }
            }

            /// <summary>
            /// Works out where a planet is on its ellipse at the given time by
            /// solving Kepler's equation, then tilts the result onto the
            /// planet's own orbital plane.
            /// </summary>
            private Vector3 GetPlanetPosition(int planetIndex, float time)
            {
                float semiMajorAxis = semiMajorAxes[planetIndex];
                float eccentricity = eccentricities[planetIndex];

                // Inner planets move faster, so period scales the whole orbit.
                float orbitalPeriod = orbitalPeriods[planetIndex] * 15.0f;

                float meanMotion = MathF.PI * 2.0f / orbitalPeriod;
                float meanAnomaly = meanMotion * time;

                // Kepler's equation cannot be rearranged for the eccentric
                // anomaly, so it is solved by Newton-Raphson iteration. Five
                // passes is more than enough at these eccentricities, and a
                // fixed count keeps every frame the same length.
                float eccentricAnomaly = meanAnomaly;

                for (int i = 0; i < 5; i++)
                {
                    eccentricAnomaly = eccentricAnomaly - (eccentricAnomaly - eccentricity * MathF.Sin(eccentricAnomaly) - meanAnomaly) / (1.0f - eccentricity * MathF.Cos(eccentricAnomaly));
                }

                float x = semiMajorAxis * (MathF.Cos(eccentricAnomaly) - eccentricity);
                float z = semiMajorAxis * MathF.Sqrt(1.0f - eccentricity * eccentricity) * MathF.Sin(eccentricAnomaly);

                float inclination = MathHelper.DegreesToRadians(inclinations[planetIndex]);

                // tilt orbit plane
                return new Vector3(x, z * MathF.Sin(inclination), z * MathF.Cos(inclination));
            }

            /// <summary>
            /// Called when the user chooses Start. Places the camera beside
            /// Earth looking at it, captures the pointer, and resets the mouse
            /// baseline so the view does not jump on the first movement.
            /// </summary>
            private void EnterSimulation()
            {
                Vector3 earthPosition = GetPlanetPosition(EarthIndex, SimulationTime);

                // Stand off far enough that Earth is fully in view.
                Vector3 offset = new Vector3(0.0f, 0.5f, 2.5f);
                position = earthPosition + offset;

                Vector3 towardsEarth = Vector3.Normalize(earthPosition - position);
                cameraFront = towardsEarth;

                // Keep pitch and yaw in step with the new direction, otherwise
                // the next mouse movement would snap the view back.
                pitch = MathHelper.RadiansToDegrees(MathF.Asin(towardsEarth.Y));
                yaw = MathHelper.RadiansToDegrees(MathF.Atan2(towardsEarth.Z, towardsEarth.X));

                firstMove = true;
                CursorState = CursorState.Grabbed;
            }

            #endregion
            #region CreateSphere
            private float[] CreateSphere(float radius, int stacks, int sectors)
            {
                List<float> vertices = new List<float>();

                for (int i = 0; i <= stacks; i++)
                {
                    // i == 0 is the north pole, i == stacks is the south pole.
                    float stackAngle = MathF.PI / 2 - i * MathF.PI / stacks;

                    // The poles sit on the Y axis so that they line up with the
                    // world's up vector and with the CreateRotationY axial spin.
                    float yPosition = radius * MathF.Sin(stackAngle);
                    float ringRadius = radius * MathF.Cos(stackAngle);

                    for (int sectorIndex = 0; sectorIndex <= sectors; sectorIndex++)
                    {
                        float sectorAngle = sectorIndex * 2 * MathF.PI / sectors;

                        float xPosition = ringRadius * MathF.Cos(sectorAngle);
                        float zPosition = ringRadius * MathF.Sin(sectorAngle);

                        //position 
                        vertices.Add(xPosition);
                        vertices.Add(yPosition);
                        vertices.Add(zPosition);

                        //texture coordinate section 
                        float textureU = (float)sectorIndex / sectors;

                        // The image is flipped when it is loaded, so v = 1 is the top
                        // row of the source map, which is where the north pole belongs.
                        float textureV = 1.0f - (float)i / stacks;

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
