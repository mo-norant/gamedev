using System;
using Unity;

namespace Gamedev
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {

        [STAThread]
        static void Main()
        {

            using (var game = new Game1())
                game.Run();
        }
    }
}
