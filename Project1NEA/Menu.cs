using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Project1NEA
{ 
    public class Menu
    {
        private readonly string[] options = { "Start Simulation", "Controls", "Credits", "Exit" };
        private readonly string[] confirmOptions = { "Yes", "No" };


        private int selectedOption = 0;
        private int confirmSelection = DefaultConfirmSelection;

        private const int DefaultConfirmSelection = 1;

        private TextRenderer text;

       
        private GameState currentInformationState = GameState.Controls;

        private static readonly Vector4 TitleColour = new Vector4(0.95f, 0.96f, 1.00f, 1.0f);
        private static readonly Vector4 SelectedColour = new Vector4(0.98f, 0.78f, 0.28f, 1.0f);
        private static readonly Vector4 NormalColour = new Vector4(0.62f, 0.66f, 0.78f, 1.0f);
        private static readonly Vector4 HintColour = new Vector4(0.45f, 0.48f, 0.58f, 1.0f);

        public bool ExitRequested { get; private set; } = false;

        private readonly string[] controlsLines =
        {
            "W  A  S  D        Move around the system",
            "Mouse             Look around",
            "Space  /  Shift   Move up  /  move down",
            "Left Ctrl         Move faster",
            "Right Shift       Reset the camera position",
            "Escape            Return to the main menu",
        };

        private readonly string[] creditsLines =
        {
            "Solar System Simulator",
            "A-Level Computer Science NEA",
            "",
            "Created by       [ Kelvin James]",
            "Planet textures  [ Google Images ]",
            "Orbital data     [ Google Search ]",
        };

        public void Load(TextRenderer textRenderer)
        {
            text = textRenderer;
        }

        public GameState Update(KeyboardState keyboard, GameState current)
        {
            switch (current)
            {
                case GameState.MainMenu:
                    return UpdateMainMenu(keyboard);

                case GameState.Controls:
                case GameState.Credits:
                    return UpdateInformationScreen(keyboard);

                case GameState.ConfirmExit:
                    return UpdateConfirmExit(keyboard);
            }

            return current;
        }

        private GameState UpdateMainMenu(KeyboardState keyboard)
        {
            if (keyboard.IsKeyPressed(Keys.Up))
            {
                selectedOption--;
            }

            if (keyboard.IsKeyPressed(Keys.Down))
            {
                selectedOption++;
            }

            selectedOption = Math.Clamp(selectedOption, 0, options.Length - 1);

            if (keyboard.IsKeyPressed(Keys.Escape))
            {
                confirmSelection = DefaultConfirmSelection;
                return GameState.ConfirmExit;
            }

            if (keyboard.IsKeyPressed(Keys.Enter))
            {
                switch (selectedOption)
                {
                    case 0:
                        return GameState.Playing;

                    case 1:
                        return GameState.Controls;

                    case 2:
                        return GameState.Credits;

                    case 3:
                        confirmSelection = DefaultConfirmSelection;
                        return GameState.ConfirmExit;
                }
            }

            return GameState.MainMenu;
        }

        private GameState UpdateInformationScreen(KeyboardState keyboard)
        {
            if (keyboard.IsKeyPressed(Keys.Escape) || keyboard.IsKeyPressed(Keys.Enter)
                || keyboard.IsKeyPressed(Keys.Backspace))
            {
                return GameState.MainMenu;
            }

            return currentInformationState;
        }

        private GameState UpdateConfirmExit(KeyboardState keyboard)
        {
            if (keyboard.IsKeyPressed(Keys.Left) || keyboard.IsKeyPressed(Keys.Up))
            {
                confirmSelection--;
            }

            if (keyboard.IsKeyPressed(Keys.Right) || keyboard.IsKeyPressed(Keys.Down))
            {
                confirmSelection++;
            }

            confirmSelection = Math.Clamp(confirmSelection, 0, confirmOptions.Length - 1);

            if (keyboard.IsKeyPressed(Keys.Escape))
            {
                return GameState.MainMenu;
            }

            if (keyboard.IsKeyPressed(Keys.Enter))
            {
                if (confirmSelection == 0)
                {
                    ExitRequested = true;
                    return GameState.ConfirmExit;
                }

                return GameState.MainMenu;
            }

            return GameState.ConfirmExit;
        }

        public void Render(GameState current, int width, int height)
        {
            switch (current)
            {
                case GameState.MainMenu:
                    RenderMainMenu(width, height);
                    break;

                case GameState.Controls:
                    currentInformationState = GameState.Controls;
                    RenderInformationScreen("CONTROLS", controlsLines, width, height);
                    break;

                case GameState.Credits:
                    currentInformationState = GameState.Credits;
                    RenderInformationScreen("CREDITS", creditsLines, width, height);
                    break;

                case GameState.ConfirmExit:
                    RenderConfirmExit(width, height);
                    break;
            }
        }

        private void RenderMainMenu(int width, int height)
        {
            float centreX = width / 2.0f;

            float titleScale = 0.95f;
            float optionScale = 0.62f;
            float spacing = text.LineHeight(optionScale) * 1.35f;

            text.DrawCentred("SOLAR SYSTEM SIMULATOR", centreX, height * 0.16f, titleScale, TitleColour);

            float firstOptionY = height * 0.40f;

            for (int optionIndex = 0; optionIndex < options.Length; optionIndex++)
            {
                bool isSelected = optionIndex == selectedOption;
                float y = firstOptionY + optionIndex * spacing;

               
                string line = isSelected ? "> " + options[optionIndex] + " <" : options[optionIndex];

                text.DrawCentred(line, centreX, y, optionScale,
                    isSelected ? SelectedColour : NormalColour);
            }

            text.DrawCentred("Arrow keys to move, Enter to select", centreX, height * 0.86f, 0.38f, HintColour);
        }

        private void RenderInformationScreen(string title, string[] lines, int width, int height)
        {
            float centreX = width / 2.0f;
            float bodyScale = 0.42f;
            float spacing = text.LineHeight(bodyScale) * 1.15f;

            text.DrawCentred(title, centreX, height * 0.16f, 0.85f, TitleColour);


            float widest = 0.0f;
            foreach (string line in lines)
            {
                widest = MathF.Max(widest, text.MeasureWidth(line, bodyScale));
            }

            float left = centreX - widest / 2.0f;
            float y = height * 0.36f;

            foreach (string line in lines)
            {
                text.Draw(line, left, y, bodyScale, NormalColour);
                y += spacing;
            }

            text.DrawCentred("Press Enter or Escape to go back", centreX, height * 0.86f, 0.38f, HintColour);
        }

        private void RenderConfirmExit(int width, int height)
        {
            float centreX = width / 2.0f;

            text.DrawCentred("Are you sure you want to exit?", centreX, height * 0.38f, 0.58f, TitleColour);

            float optionScale = 0.62f;
            float gap = 160.0f;
            float y = height * 0.52f;

            for (int optionIndex = 0; optionIndex < confirmOptions.Length; optionIndex++)
            {
                bool isSelected = optionIndex == confirmSelection;
                string line = isSelected ? "> " + confirmOptions[optionIndex] + " <" : confirmOptions[optionIndex];

                float x = centreX + (optionIndex == 0 ? -gap : gap);

                text.DrawCentred(line, x, y, optionScale,
                    isSelected ? SelectedColour : NormalColour);
            }

            text.DrawCentred("Left and right to choose, Enter to confirm", centreX, height * 0.86f, 0.38f, HintColour);
        }

        public int SelectedOption
        {
            get { return selectedOption; }
        }
    }
}
