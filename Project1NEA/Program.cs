using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenTK.Mathematics;

namespace BuildObject
{
    public class Game : GameWindow
    {
        public Game(int width, int height, string name) : base(GameWindowSettings.Default, new NativeWindowSettings()
        {
            Size = new Vector2i(width, height),
            Title = name
        })
        { }
        
    }
}
