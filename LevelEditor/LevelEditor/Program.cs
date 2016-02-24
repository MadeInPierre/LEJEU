using System;
using System.Windows.Forms;

namespace LevelEditor
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string LoadPath = "";
            if (args.Length != 0) LoadPath = args[0];
            

            Game1 game = new Game1(LoadPath);
            game.Run();
        }
    }
#endif
}
