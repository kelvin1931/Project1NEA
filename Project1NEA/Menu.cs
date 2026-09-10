using OpenTK.Windowing.GraphicsLibraryFramework;
using Microsoft.VisualBasic;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using Project1NEA;
using System.Diagnostics;
using System.Reflection;
using static Project1NEA.Shaders;
using static System.Net.Mime.MediaTypeNames;

namespace Project1NEA
{
    public class Menu
    {
        private int selectedOption = 0;
        private int vao;
        private int vbo;
        private Shader shader;

        // Vertical placement of the option bars in normalised device coordinates.
        private const float optionSpacing = 0.22f;
        private const float firstOptionY = 0.33f;

        private string[] options = { "Start Simulation", "Controls", "Credits", "Exit" };




        public bool StartGame 
        { get; private set; } = false;

        public bool ExitRequested
        { get; private set; } = false;




        private void InitUI()
        {
            float[] quad =
                {
                // pos
                -0.5f,  0.1f,
                0.5f,  0.1f,
                0.5f, -0.1f,

                0.5f, -0.1f,
                -0.5f, -0.1f,
                -0.5f,  0.1f
            };

            vao = GL.GenVertexArray();
            vbo = GL.GenBuffer();

            GL.BindVertexArray(vao);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, quad.Length * sizeof(float), quad, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
        }




        public void Load()
        {
            shader = new Shader("menu.vert", "menu.frag");
            InitUI();
        }





        public void Update(KeyboardState keyboard)
        {
            if (keyboard.IsKeyPressed(Keys.Up))
                selectedOption--;

            if (keyboard.IsKeyPressed(Keys.Down))
                selectedOption++;

            selectedOption = Math.Clamp(selectedOption, 0, options.Length - 1);

            if (keyboard.IsKeyPressed(Keys.Enter))
            {
                switch (selectedOption)
                {
                    case 0:
                        StartGame = true;
                        break;

                    case 1:
                        // Controls screen
                        break;

                    case 2:
                        // Credits screen
                        break;

                    case 3:
                        ExitRequested = true;
                        break;
                }
            }
        }




        public void Render()
        {
            shader.Use();
            GL.BindVertexArray(vao);

            for (int optionIndex = 0; optionIndex < options.Length; optionIndex++)
            {
                float y = firstOptionY - optionIndex * optionSpacing;

                // The highlighted option is drawn wider and brighter than the rest.
                bool isSelected = optionIndex == selectedOption;

                shader.SetVector2("offset", new Vector2(0.0f, y));
                shader.SetVector2("scale", isSelected ? new Vector2(1.1f, 0.9f) : new Vector2(1.0f, 0.75f));
                shader.SetVector4("colour", isSelected
                    ? new Vector4(0.95f, 0.75f, 0.25f, 1.0f)
                    : new Vector4(0.25f, 0.25f, 0.35f, 1.0f));

                GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
            }
        }




        public int SelectedOption
        {
            get { return selectedOption; }
        }




        public void ResetStartGame()
        {
            StartGame = false;
        }



    }
}