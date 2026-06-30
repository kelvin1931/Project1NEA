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

        private string[] options = { "Start Simulation", "Controls", "Credits", "Exit" };




        public bool StartGame 
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
            // shaders, textures and buttons etc
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
                        // Exit will be handled by Game.cs
                        break;
                }
            }
        }




        public void Render()
        {
            //do rendering here
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