using System.Runtime.CompilerServices;
using LearnOpenTK;
using OpenTK;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;


namespace PGrafica1
{
    class Program
    {

        static void Main(string[] args)
        {
            var nativeWindowSettings = new NativeWindowSettings()
            {
                ClientSize = new Vector2i(800, 600),
                Title = "Hola mundo Opengl",
            };
            using (Game game = new Game(GameWindowSettings.Default, nativeWindowSettings))
            {
                game.Run();// iniciamos la ventana
            }
            
        }
    }
}
