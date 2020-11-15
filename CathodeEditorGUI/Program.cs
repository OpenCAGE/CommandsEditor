using System;
using System.Windows.Forms;

namespace CathodeEditorGUI
{
    public static class SharedData
    {
        public static string pathToAI = "";
    }

    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (Environment.GetCommandLineArgs().Length > 0)
            {
                SharedData.pathToAI = Environment.GetCommandLineArgs()[0];
            }

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new CathodeEditorGUI());
        }
    }
}
